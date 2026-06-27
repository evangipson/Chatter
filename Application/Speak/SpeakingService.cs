using System.Text;
using Microsoft.Extensions.AI;
using NAudio.Wave;
using Application.Agent;
using Application.Clients;
using Domain.Constants;
using Domain.Models;

namespace Application.Speak;

/// <inheritdoc cref="ISpeakingService"/>
public class SpeakingService(IAgentRunner agentRunner) : ISpeakingService
{
    private static readonly WaveFormat _waveFormat = new(SpeakingConstants.WaveRate, SpeakingConstants.WaveBits, SpeakingConstants.WaveChannels);

    public async IAsyncEnumerable<string> StreamAndSpeakAsync(ToolContext toolContext, List<ChatMessage> history, bool speak)
    {
        // start streaming the response from the chat client
        StringBuilder speechBuffer = new();

        // get the agent's response from running the toolchain
        var agentResponse = await agentRunner.RunAsync(toolContext, history);

        // get the response from the chat client
        foreach (char c in agentResponse.Response)
        {
            // return the character from the response
            yield return c.ToString();

            // if this is a speaking response, generate text-to-speech audio
            if (speak)
            {
                // append the token to the speech buffer
                speechBuffer.Append(c);
                var currentText = speechBuffer.ToString();

                // find if there's any complete sentence punctuation in our speech buffer
                var punctuationIndex = currentText.AsSpan().IndexOfAny(ChatConstants.PunctuationSearch);
                if (punctuationIndex > -1)
                {
                    // extract the full sentence up to and including the punctuation
                    await SpeakAsync(currentText[..(punctuationIndex + 1)].Trim());

                    // keep any leftover incomplete text in the buffer for the next sentence
                    speechBuffer.Remove(0, punctuationIndex + 1);
                }
            }
        }

        // flush out any remaining words left in the speaking buffer after stream finishes
        if (speak && speechBuffer.Length > 0)
        {
            await SpeakAsync(speechBuffer.ToString().Trim());
        }
    }

    private static async Task SpeakAsync(string text)
    {
        // generate audio
        using WaveOutEvent outputDevice = new();
        var generatedSpeech = SpeakingClients.Client.Generate(text, 1.0f, 0);
        var sampleBuffer = GetSampleBuffer(generatedSpeech.Samples);

        // create a new buffer
        BufferedWaveProvider buffer = new(_waveFormat)
        {
            BufferLength = sampleBuffer.Length,
            BufferDuration = TimeSpan.FromMinutes(1)
        };

        // load and play converted 16-bit PCM
        buffer.AddSamples([..sampleBuffer], 0, sampleBuffer.Length);
        outputDevice.Init(buffer);
        outputDevice.Play();
        while (buffer.BufferedBytes > 0)
        {
            await Task.Delay(100);
        }
    }

    private static ReadOnlySpan<byte> GetSampleBuffer(ReadOnlySpan<float> samples)
    {
        byte[] buffer = new byte[samples.Length * 2];
        for (int i = 0; i < samples.Length; i++)
        {
            var sample = (short)(Math.Clamp(samples[i], -1.0f, 1.0f) * short.MaxValue);
            BitConverter.TryWriteBytes(buffer.AsSpan(i * 2), sample);
        }

        return buffer;
    }
}

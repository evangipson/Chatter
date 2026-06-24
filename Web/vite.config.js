import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig({
  plugins: [react()],
  build: {
    outDir: path.resolve(__dirname, '../API/wwwroot'),
    emptyOutDir: true
  },
  server: {
    proxy: {
      '/chat': { target: 'http://localhost:5015', secure: false },
      '/conversations': { target: 'http://localhost:5015', secure: false },
      '/workspaces': { target: 'http://localhost:5015', secure: false },
    }
  }
});
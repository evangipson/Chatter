import { useEffect } from 'react';

/**
 * Autoscrolls `value` to `belowValueRef` so the bottom-most area is shown.
 * @param {React.RefObject<HTMLDivElement>} belowValueRef The reference for the element below `value`.
 * @param {string} value The value to auto-scroll to.
 */
export function useAutoScroll(belowValueRef, value) {
    useEffect(() => belowValueRef?.current?.scrollIntoView(), [value]);
}
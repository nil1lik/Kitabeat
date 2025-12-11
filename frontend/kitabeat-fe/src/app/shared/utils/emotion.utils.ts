/**
 * Maps emotion labels to PrimeNG icon class names.
 */
const EMOTION_ICONS: Record<string, string> = {
    joy: 'pi-face-smile',
    sadness: 'pi-cloud',
    anger: 'pi-bolt',
    fear: 'pi-exclamation-triangle',
    surprise: 'pi-star',
    love: 'pi-heart',
    neutral: 'pi-minus-circle',
};

/**
 * Maps emotion labels to PrimeNG tag severity values.
 */
const EMOTION_SEVERITIES: Record<string, EmotionSeverity> = {
    joy: 'success',
    sadness: 'info',
    anger: 'danger',
    fear: 'warn',
    surprise: 'contrast',
    love: 'danger',
    neutral: 'secondary',
};

export type EmotionSeverity = 'success' | 'info' | 'warn' | 'danger' | 'secondary' | 'contrast';

/**
 * Gets the PrimeNG icon class for an emotion label.
 */
export function getEmotionIcon(emotionLabel: string): string {
    return EMOTION_ICONS[emotionLabel.toLowerCase()] || 'pi-tag';
}

/**
 * Gets the PrimeNG tag severity for an emotion label.
 */
export function getEmotionSeverity(emotionLabel: string): EmotionSeverity {
    return EMOTION_SEVERITIES[emotionLabel.toLowerCase()] || 'info';
}

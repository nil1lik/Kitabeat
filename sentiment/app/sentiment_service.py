"""
Sentiment analysis service logic.
"""
from typing import List, Tuple
from .model_loader import sentiment_pipeline
from .schemas import EmotionScore

# Number of top emotions to return
TOP_K_EMOTIONS = 5


def analyze_text(text: str) -> Tuple[str, float, List[EmotionScore]]:
    """
    Analyze text for emotional content.
    
    Args:
        text: The text to analyze.
        
    Returns:
        Tuple of (primary_label, primary_score, list of emotions).
    """
    if not text.strip():
        return "neutral", 0.0, []
    
    # Get predictions from the model
    outputs = sentiment_pipeline(text)[0]
    
    # Sort by score (descending)
    sorted_outputs = sorted(outputs, key=lambda x: x["score"], reverse=True)
    
    # Get primary emotion
    primary = sorted_outputs[0]
    primary_label = primary["label"].lower()
    primary_score = float(primary["score"])
    
    # Get top K emotions
    top_emotions = [
        EmotionScore(
            label=output["label"].lower(),
            score=float(output["score"])
        )
        for output in sorted_outputs[:TOP_K_EMOTIONS]
    ]
    
    return primary_label, primary_score, top_emotions

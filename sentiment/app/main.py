"""
Beatbooks Emotion API - FastAPI application for sentiment analysis.
"""
from fastapi import FastAPI
from .schemas import SentimentRequest, SentimentResponse
from .sentiment_service import analyze_text


app = FastAPI(
    title="Beatbooks Emotion API",
    version="0.3.0",
    description="API for analyzing emotional content in text using GoEmotions model.",
)


@app.post("/analyze", response_model=SentimentResponse)
def analyze_sentiment(request: SentimentRequest) -> SentimentResponse:
    """
    Analyze the emotional content of a given text.
    
    Returns the primary emotion and top 5 emotions with their scores.
    """
    primary_label, primary_score, emotions = analyze_text(request.text)
    
    return SentimentResponse(
        primary_label=primary_label,
        primary_score=primary_score,
        emotions=emotions,
    )


@app.get("/health")
def health_check():
    """Health check endpoint."""
    return {"status": "healthy"}
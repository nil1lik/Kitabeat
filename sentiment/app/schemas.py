"""
Request and response schemas for the sentiment analysis API.
"""
from pydantic import BaseModel
from typing import List


class SentimentRequest(BaseModel):
    """Request body for sentiment analysis."""
    text: str


class EmotionScore(BaseModel):
    """Single emotion with its confidence score."""
    label: str
    score: float


class SentimentResponse(BaseModel):
    """Response containing sentiment analysis results."""
    primary_label: str
    primary_score: float
    emotions: List[EmotionScore]

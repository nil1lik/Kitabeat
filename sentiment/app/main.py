from fastapi import FastAPI
from pydantic import BaseModel

app = FastAPI(
    title="Beatbooks Sentiment API",
    version="0.1.0",
    description="Kitap açıklamaları için duygu analizi servisi (iskelet).",
)


class SentimentRequest(BaseModel):
    text: str


class SentimentResponse(BaseModel):
    sentiment: str  # "positive", "neutral", "negative"
    score: float    # 0.0 - 1.0 arası bir güven skoru


@app.get("/health")
def health_check():
    return {"status": "ok", "service": "kitabeat-sentiment"}


@app.post("/analyze", response_model=SentimentResponse)
def analyze_sentiment(request: SentimentRequest):
    """
    Şimdilik sadece çok basit, placeholder bir mantık:
    - içinde 'iyi' geçiyorsa: positive
    - içinde 'kötü' geçiyorsa: negative
    - aksi halde: neutral

    Sonra burayı gerçek ML modeli ile değiştireceğiz.
    """

    text_lower = request.text.lower()

    if "iyi" in text_lower or "harika" in text_lower:
        sentiment = "positive"
        score = 0.9
    elif "kötü" in text_lower or "berbat" in text_lower:
        sentiment = "negative"
        score = 0.9
    else:
        sentiment = "neutral"
        score = 0.5

    return SentimentResponse(sentiment=sentiment, score=score)

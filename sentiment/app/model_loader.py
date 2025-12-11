"""
Model loading utilities for the sentiment analysis service.
"""
from transformers import (
    AutoTokenizer, 
    AutoModelForSequenceClassification, 
    TextClassificationPipeline
)

MODEL_NAME = "bhadresh-savani/bert-base-go-emotion"


def _load_model():
    """Load and configure the sentiment analysis model."""
    tokenizer = AutoTokenizer.from_pretrained(MODEL_NAME)
    model = AutoModelForSequenceClassification.from_pretrained(MODEL_NAME)
    
    return TextClassificationPipeline(
        model=model,
        tokenizer=tokenizer,
        task="text-classification",
        return_all_scores=True,
        function_to_apply="sigmoid",
    )


# Initialize the pipeline at module load time
sentiment_pipeline = _load_model()
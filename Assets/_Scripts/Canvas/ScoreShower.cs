using UnityEngine;

public class ScoreShower : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _scoreText;

    private void OnEnable()
    {
        FigureDestroyer.OnScoreValueChanged += HandleScoreValueChanged;
    }

    private void HandleScoreValueChanged(int currentScore)
    { 
        _scoreText.text = currentScore.ToString();
    }

    private void OnDisable()
    {
        FigureDestroyer.OnScoreValueChanged -= HandleScoreValueChanged;
    }
}

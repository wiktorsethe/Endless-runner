using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI[] currentScoreText;
    public TextMeshProUGUI bestScoreText;

    private float _currentScore = 0;
    private float _bestScore = 0;

    void Start()
    {
        _bestScore = PlayerPrefs.GetFloat("BestScore", 0);
        UpdateScoreUI();
    }

    void Update()
    {
        _currentScore += Time.deltaTime * 10;

        UpdateScoreUI();

        if (_currentScore > _bestScore)
        {
            _bestScore = _currentScore;
            PlayerPrefs.SetFloat("BestScore", _bestScore);
            PlayerPrefs.Save();
        }
    }

    private void UpdateScoreUI()
    {
        if (currentScoreText != null)
        {
            foreach (var scoreText in currentScoreText)
                scoreText.text = $"Score: {_currentScore:F0}";
        }

        if (bestScoreText != null)
            bestScoreText.text = $"Best Score: {_bestScore:F0}";
    }

    public void ResetScore()
    {
        _currentScore = 0;
    }
}
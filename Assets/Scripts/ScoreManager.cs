using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] currentScoreText;
    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private Transform playerTransform;

    private float _currentScore = 0;
    private float _bestScore = 0;
    private const float StartX = -11.3f;

    void Start()
    {
        _bestScore = PlayerPrefs.GetFloat("BestScore", 0);
        UpdateScoreUI();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            _currentScore = (playerTransform.position.x - StartX) * 10;

            if (_currentScore < 0)
                _currentScore = 0;

            if (_currentScore > _bestScore)
            {
                _bestScore = _currentScore;
                PlayerPrefs.SetFloat("BestScore", _bestScore);
                PlayerPrefs.Save();
            }

            UpdateScoreUI();
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
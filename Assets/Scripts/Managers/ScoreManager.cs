using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] TMP_Text scoreText;
    private int score = 0;

    public void IncreaseScore(int points)
    {
        if (gameManager.GameOver()) return;
        score += points;
        scoreText.text = score.ToString();
    }
}

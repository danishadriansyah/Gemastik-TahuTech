using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Mengupdate tampilan teks skor, target, dan panel game over.
/// Logika timer sudah dipindahkan ke TimerUI.cs
/// </summary>
public class ScoreUI : MonoBehaviour
{
    [Header("Referensi Teks")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI targetScoreText;
    // Referensi ke timerText sudah dihapus

    [Header("Panel Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;


    void Start()
    {
        // Pastikan panel game over tidak aktif di awal
        if (gameOverPanel != null) gameOverPanel.SetActive(false);

        // Daftar untuk mendengarkan event.
        ScoreManager.instance.OnScoreChanged += UpdateScoreText;
        GameManager.instance.OnGameOver += ShowGameOverScreen;
        // Pendaftaran ke event OnTimeChanged sudah dihapus

        // Atur teks awal.
        UpdateScoreText(ScoreManager.instance.currentScore);
        targetScoreText.text = "Target: " + GameManager.instance.targetScore;
    }

    void OnDestroy()
    {
        // Penting: Berhenti mendengarkan saat objek hancur.
        if (ScoreManager.instance != null) ScoreManager.instance.OnScoreChanged -= UpdateScoreText;
        if (GameManager.instance != null)
        {
            GameManager.instance.OnGameOver -= ShowGameOverScreen;
        }
    }

    void UpdateScoreText(int newScore)
    {
        scoreText.text = "Skor: " + newScore.ToString();
    }

    // Method UpdateTimerText() sudah dihapus

    void ShowGameOverScreen(string message)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = message;
        }
    }
}
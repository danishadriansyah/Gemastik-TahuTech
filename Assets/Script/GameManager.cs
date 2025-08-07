using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Singleton yang mengelola state utama permainan (timer, target, menang/kalah).
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Pengaturan Level")]
    public int targetScore = 500;
    public float levelTimeInSeconds = 120f; // Waktu level dalam detik (2 menit)

    [Header("Status Level")]
    private float currentTime;
    private bool isGameOver = false;

    // Event yang akan dipanggil untuk update UI
    public event Action<float> OnTimeChanged;
    public event Action<string> OnGameOver; // Mengirim pesan Menang/Kalah

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        // Atur ulang permainan saat dimulai
        currentTime = levelTimeInSeconds;
        isGameOver = false;
        Time.timeScale = 1f; // Pastikan game berjalan normal

        // Mulai mendengarkan perubahan skor
        ScoreManager.instance.OnScoreChanged += CheckWinCondition;
    }

    void Update()
    {
        // Jika game sudah selesai, jangan lakukan apa-apa
        if (isGameOver) return;

        // Kurangi waktu
        currentTime -= Time.deltaTime;
        OnTimeChanged?.Invoke(currentTime); // Kirim update waktu ke UI

        // Cek kondisi kalah (waktu habis)
        if (currentTime <= 0)
        {
            currentTime = 0;
            EndGame(false); // Player kalah
        }
    }

    void CheckWinCondition(int newScore)
    {
        // Jika game sudah selesai, abaikan
        if (isGameOver) return;

        // Cek kondisi menang (target skor tercapai)
        if (newScore >= targetScore)
        {
            EndGame(true); // Player menang
        }
    }

    void EndGame(bool didWin)
    {
        isGameOver = true;
        Time.timeScale = 0f; // Menghentikan semua pergerakan di game

        if (didWin)
        {
            Debug.Log("KAMU MENANG!");
            OnGameOver?.Invoke("Kamu Menang!");
        }
        else
        {
            Debug.Log("WAKTU HABIS! KAMU KALAH!");
            OnGameOver?.Invoke("Waktu Habis!");
        }
    }

    void OnDestroy()
    {
        // Penting: Berhenti mendengarkan saat objek hancur
        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.OnScoreChanged -= CheckWinCondition;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Singleton untuk mengelola skor atau uang pemain.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    // Event yang akan dipanggil setiap kali skor berubah.
    public event Action<int> OnScoreChanged;

    public int currentScore { get; private set; }

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public void AddScore(int amount)
    {
        currentScore += amount;
        // Memberi tahu semua yang "mendengarkan" bahwa skor telah berubah.
        OnScoreChanged?.Invoke(currentScore);
        Debug.Log("Skor ditambahkan: " + amount + ". Total sekarang: " + currentScore);
    }
}
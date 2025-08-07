using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Mengelola dan mengupdate tampilan timer permainan.
/// Pasang script ini pada TimerPanel.
/// </summary>
public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Start()
    {
        // Mendaftar untuk mendengarkan event perubahan waktu dari GameManager.
        GameManager.instance.OnTimeChanged += UpdateTimerText;
    }

    void OnDestroy()
    {
        // Penting: Berhenti mendengarkan saat objek hancur untuk menghindari error.
        if (GameManager.instance != null)
        {
            GameManager.instance.OnTimeChanged -= UpdateTimerText;
        }
    }

    void UpdateTimerText(float time)
    {
        // Format waktu agar menjadi menit:detik (contoh: 01:30)
        time = Mathf.Max(time, 0); // Jangan biarkan waktu menjadi negatif
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}


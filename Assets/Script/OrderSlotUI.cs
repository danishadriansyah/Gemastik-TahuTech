using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Gunakan ini jika Anda memakai TextMeshPro untuk teks yang lebih bagus

/// <summary>
/// Mengelola tampilan untuk satu slot pesanan di UI.
/// Pasang script ini pada setiap prefab/GameObject pesanan.
/// </summary>
public class OrderSlotUI : MonoBehaviour
{
    [Header("Referensi UI")]
    public Image dishIcon; // Gambar dari masakan yang dipesan
    public TextMeshProUGUI dishNameText; // Nama dari masakan yang dipesan

    //[Header("Status")]
    public RecipeData currentRecipe { get; private set; }
    public bool isOccupied { get; private set; }

    void Start()
    {
        // Pastikan slot kosong di awal
        ClearOrder();
    }

    /// <summary>
    /// Mengatur slot ini untuk menampilkan pesanan baru.
    /// </summary>
    public void SetOrder(RecipeData newRecipe)
    {
        currentRecipe = newRecipe;
        isOccupied = true;

        // Tampilkan visualnya
        dishIcon.sprite = newRecipe.cookedDish.itemIcon;
        dishNameText.text = newRecipe.cookedDish.itemName;
        dishIcon.enabled = true;
        dishNameText.enabled = true;
    }

    /// <summary>
    /// Membersihkan slot ini setelah pesanan selesai.
    /// </summary>
    public void ClearOrder()
    {
        currentRecipe = null;
        isOccupied = false;

        // Sembunyikan visualnya
        dishIcon.enabled = false;
        dishNameText.text = "";
    }
}

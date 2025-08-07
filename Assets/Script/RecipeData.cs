using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Template untuk membuat aset Resep di dalam Project.
/// Klik kanan di Project > Create > Cooking > Recipe.
/// </summary>
[CreateAssetMenu(fileName = "New Recipe", menuName = "Cooking/Recipe")]
public class RecipeData : ScriptableObject
{
    [Header("Informasi Resep")]
    // Daftar bahan yang dibutuhkan untuk resep ini.
    public List<ItemData> ingredients;
    // Hasil masakan setelah resep ini berhasil dibuat.
    public ItemData cookedDish;
    // Waktu yang dibutuhkan untuk memasak (dalam detik).
    public float cookingTime = 5f;

    [Header("Nilai")]
    public int scoreValue = 10;
}


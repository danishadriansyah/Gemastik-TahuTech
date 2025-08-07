using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton yang mengelola pembuatan dan penyelesaian pesanan.
/// </summary>
public class OrderManager : MonoBehaviour
{
    public static OrderManager instance;

    [Header("Pengaturan Pesanan")]
    // Masukkan semua resep yang mungkin muncul sebagai pesanan di sini.
    public List<RecipeData> allPossibleRecipes;
    // Masukkan semua slot UI pesanan dari Hierarchy ke sini.
    public List<OrderSlotUI> orderSlots;
    // Waktu antar pesanan baru (dalam detik).
    public float orderInterval = 10f;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        // Mulai rutinitas untuk memunculkan pesanan baru.
        StartCoroutine(OrderGenerationRoutine());
    }

    private IEnumerator OrderGenerationRoutine()
    {
        // Loop ini akan berjalan selamanya.
        while (true)
        {
            // Tunggu selama interval waktu yang ditentukan.
            yield return new WaitForSeconds(orderInterval);

            // Coba buat pesanan baru.
            CreateNewOrder();
        }
    }

    private void CreateNewOrder()
    {
        // Cari slot yang kosong.
        OrderSlotUI emptySlot = null;
        foreach (var slot in orderSlots)
        {
            if (!slot.isOccupied)
            {
                emptySlot = slot;
                break; // Dapatkan slot kosong pertama dan berhenti mencari.
            }
        }

        // Jika ada slot yang kosong, buat pesanan baru.
        if (emptySlot != null)
        {
            // Pilih resep secara acak dari daftar.
            int randomIndex = Random.Range(0, allPossibleRecipes.Count);
            RecipeData randomRecipe = allPossibleRecipes[randomIndex];

            // Atur slot tersebut untuk menampilkan resep yang dipilih.
            emptySlot.SetOrder(randomRecipe);
        }
        else
        {
            Debug.Log("Semua slot pesanan penuh!");
        }
    }

    /// <summary>
    /// Dipanggil oleh ServeStation untuk memeriksa apakah masakan yang disajikan cocok.
    /// </summary>
    public void CheckServedDish(ItemData servedDish)
    {
        foreach (var slot in orderSlots)
        {
            if (slot.isOccupied && slot.currentRecipe.cookedDish == servedDish)
            {
                Debug.Log("Pesanan " + servedDish.itemName + " selesai!");

                // --- TAMBAHAN BARU ---
                // Panggil ScoreManager untuk menambahkan skor.
                ScoreManager.instance.AddScore(slot.currentRecipe.scoreValue);

                // Bersihkan slot tersebut.
                slot.ClearOrder();
                return;
            }
        }
        Debug.Log("Penyajian salah! " + servedDish.itemName + " tidak ada di pesanan.");
    }
}

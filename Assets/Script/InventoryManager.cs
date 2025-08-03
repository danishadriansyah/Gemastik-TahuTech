using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Singleton yang mengelola semua data inventory player.
/// Script ini tidak perlu ditempatkan pada player, tapi pada GameObject manager yang kosong.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    // Singleton pattern: membuat instance statis agar bisa diakses dari mana saja.
    public static InventoryManager instance;

    // Event yang akan dipanggil setiap kali inventory berubah (item ditambah/dihapus).
    public event Action OnInventoryChanged;

    // Daftar yang menyimpan semua item yang dimiliki player.
    public List<ItemData> items = new List<ItemData>();

    void Awake()
    {
        // Setup Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            // Jangan hancurkan objek ini saat pindah scene.
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Menambahkan item ke dalam daftar inventory.
    /// </summary>
    public void AddItem(ItemData item)
    {
        items.Add(item);
        // Memberi tahu semua yang "mendengarkan" bahwa inventory telah berubah.
        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Menghapus item dari daftar inventory.
    /// </summary>
    public void RemoveItem(ItemData item)
    {
        items.Remove(item);
        // Memberi tahu semua yang "mendengarkan" bahwa inventory telah berubah.
        OnInventoryChanged?.Invoke();
    }
}

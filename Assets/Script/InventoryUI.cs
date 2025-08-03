using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Mengelola tampilan visual dari inventory.
/// Pasang script ini pada Panel utama UI Inventory.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    [Header("Referensi UI")]
    // Parent object dari semua slot inventory.
    public Transform itemsParent;
    // Prefab untuk satu slot inventory.
    public GameObject inventorySlotPrefab;

    void Start()
    {
        // Mendaftar ke event OnInventoryChanged.
        // Setiap kali event itu dipanggil, method UpdateUI juga akan terpanggil.
        InventoryManager.instance.OnInventoryChanged += UpdateUI;
        UpdateUI(); // Panggil sekali di awal untuk menampilkan item awal (jika ada)
    }

    void OnDestroy()
    {
        // Penting: Berhenti mendengarkan event saat objek dihancurkan untuk menghindari error.
        InventoryManager.instance.OnInventoryChanged -= UpdateUI;
    }

    /// <summary>
    /// Memperbarui tampilan UI inventory berdasarkan data dari InventoryManager.
    /// </summary>
    void UpdateUI()
    {
        // 1. Hapus semua slot lama sebelum membuat yang baru.
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // 2. Buat slot baru untuk setiap item di inventory.
        foreach (ItemData item in InventoryManager.instance.items)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, itemsParent);
            // Cari komponen Image di dalam prefab slot.
            Image icon = slotGO.transform.Find("ItemIcon").GetComponent<Image>();
            // Atur icon-nya.
            icon.sprite = item.itemIcon;
        }
    }
}

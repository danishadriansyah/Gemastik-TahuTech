using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Singleton untuk mengelola UI panel stasiun.
/// Pasang di GameObject Manager yang kosong.
/// </summary>
public class StationUIManager : MonoBehaviour
{
    public static StationUIManager instance;

    [Header("Referensi UI")]
    public GameObject stationUIPanel; // Panel utama UI stasiun
    public Transform itemsParent;     // Parent untuk menampung slot-slot item
    public GameObject stationSlotPrefab; // Prefab untuk satu slot item (HARUS punya Button)

    private ItemStation currentStation; // Menyimpan stasiun mana yang sedang dibuka

    void Awake()
    {
        // Setup Singleton
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        // Pastikan panel tertutup di awal permainan.
        stationUIPanel.SetActive(false);
    }

    /// <summary>
    /// Membuka panel UI dan menampilkan item dari stasiun yang diberikan.
    /// </summary>
    public void OpenStationUI(ItemStation station)
    {
        currentStation = station;
        stationUIPanel.SetActive(true);
        UpdateStationUI();
    }

    /// <summary>
    /// Menutup panel UI stasiun.
    /// </summary>
    public void CloseStationUI()
    {
        stationUIPanel.SetActive(false);
        currentStation = null;
    }

    /// <summary>
    /// Meng-update tampilan UI berdasarkan isi stasiun saat ini.
    /// </summary>
    void UpdateStationUI()
    {
        // Hapus semua slot lama
        foreach (Transform child in itemsParent)
        {
            Destroy(child.gameObject);
        }

        // Buat slot baru untuk setiap item di stasiun
        foreach (ItemData item in currentStation.availableItems)
        {
            GameObject slotGO = Instantiate(stationSlotPrefab, itemsParent);

            // Atur icon
            Image icon = slotGO.transform.Find("ItemIcon").GetComponent<Image>();
            icon.sprite = item.itemIcon;

            // Tambahkan fungsi pada tombol untuk mengambil item
            Button takeButton = slotGO.GetComponent<Button>();
            takeButton.onClick.RemoveAllListeners(); // Selalu hapus listener lama
            takeButton.onClick.AddListener(() => TakeItem(item));
        }
    }

    /// <summary>
    /// Fungsi yang dipanggil saat tombol item di panel di-klik.
    /// </summary>
    void TakeItem(ItemData item)
    {
        Debug.Log("Mengambil: " + item.itemName);
        // Menambahkan item ke inventory PRIBADI player.
        InventoryManager.instance.AddItem(item);

        // Di sini kamu bisa menambahkan logika lain, misalnya:
        // - Mengurangi jumlah item di stasiun.
        // - Menutup UI setelah mengambil item.
        // Untuk sekarang, kita hanya menambahkannya ke inventory player.
    }
}

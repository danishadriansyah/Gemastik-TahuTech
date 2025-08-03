using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pasang script ini pada objek yang berfungsi sebagai wadah atau stasiun (misal: peti, kulkas, dll).
/// </summary>
public class ItemStation : MonoBehaviour
{
    [Header("Isi Wadah")]
    // Daftar item yang tersedia di dalam stasiun ini.
    // Kamu bisa isi daftar ini dari Inspector Unity.
    public List<ItemData> availableItems = new List<ItemData>();

    // Method ini akan dipanggil oleh player saat berinteraksi.
    public void Interact()
    {
        // Memerintahkan UI Manager untuk membuka panel dan menampilkan isi dari stasiun INI.
        StationUIManager.instance.OpenStationUI(this);
    }
}

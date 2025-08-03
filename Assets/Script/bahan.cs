using UnityEngine;

/// <summary>
/// Script penanda untuk semua objek "bahan" yang bisa diambil oleh player.
/// Cukup tambahkan script ini ke setiap GameObject bahan.
/// </summary>
public class bahan : MonoBehaviour
{
    // Script ini tidak memerlukan logika di dalamnya,
    // fungsinya hanya sebagai penanda (component marker).
    // Kamu bisa menambahkan variabel di sini nanti jika perlu,
    // misalnya nama item, deskripsi, dll.
    public string namaItem = "Bahan Mentah";
}

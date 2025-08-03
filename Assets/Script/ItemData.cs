using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Menggunakan ScriptableObject untuk membuat template data item di dalam Project Assets.
/// Untuk membuat item baru: Klik kanan di folder Project > Create > Inventory > Item.
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Informasi Item")]
    public string itemName = "New Item";
    public Sprite itemIcon = null;
    [TextArea(3, 10)]
    public string itemDescription = "Item description here.";

    // Kamu bisa menambahkan properti lain di sini,
    // seperti harga, tipe item, efek, dll.
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pasang script ini pada setiap GameObject di scene yang bisa diambil.
/// </summary>
public class ItemPickup : MonoBehaviour
{
    // Hubungkan aset ItemData yang sesuai di Inspector.
    public ItemData itemData;

    // Method ini akan dipanggil oleh script player saat berinteraksi.
    public void Interact()
    {
        // Menambahkan item ke inventory manager.
        InventoryManager.instance.AddItem(itemData);

        // Menghancurkan GameObject dari scene setelah diambil.
        Destroy(gameObject);
    }
}

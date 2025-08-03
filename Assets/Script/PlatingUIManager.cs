using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Mengelola UI untuk memilih masakan yang akan ditaruh di piring.
/// </summary>
public class PlatingUIManager : MonoBehaviour
{
    public static PlatingUIManager instance;

    [Header("Referensi UI")]
    public GameObject platingPanel;
    public Transform cookedDishesParent;
    public GameObject inventorySlotPrefab; // Kita bisa pakai ulang prefab slot inventory

    private PlateStation currentPlate;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        platingPanel.SetActive(false);
    }

    public void OpenPlatingUI(PlateStation plate)
    {
        currentPlate = plate;
        platingPanel.SetActive(true);
        UpdateCookedDishesUI();
    }

    public void ClosePlatingUI()
    {
        platingPanel.SetActive(false);
    }

    // Menampilkan HANYA masakan jadi dari inventory player.
    void UpdateCookedDishesUI()
    {
        foreach (Transform child in cookedDishesParent) Destroy(child.gameObject);

        foreach (var item in InventoryManager.instance.items)
        {
            // Asumsi: Masakan jadi punya deskripsi tertentu atau tipe item.
            // Untuk sekarang, kita tampilkan semua item. Nanti bisa difilter.
            // Contoh filter: if (!item.isIngredient) { ... }

            GameObject slotGO = Instantiate(inventorySlotPrefab, cookedDishesParent);
            slotGO.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.itemIcon;

            Button itemButton = slotGO.AddComponent<Button>();
            itemButton.onClick.AddListener(() => OnDishSelected(item));
        }
    }

    void OnDishSelected(ItemData dish)
    {
        currentPlate.FillPlate(dish);
        ClosePlatingUI();
    }
}

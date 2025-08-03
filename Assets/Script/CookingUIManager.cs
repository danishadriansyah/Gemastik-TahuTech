using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// Singleton untuk mengelola UI memasak.
/// </summary>
public class CookingUIManager : MonoBehaviour
{
    public static CookingUIManager instance;

    [Header("Referensi UI")]
    public GameObject cookingPanel;
    public Transform playerInventoryParent; // Slot untuk menampilkan item dari inventory player
    public Transform recipeSlotsParent;     // Slot untuk menaruh bahan resep
    public GameObject inventorySlotPrefab;  // Prefab slot dari inventory
    public Button cookButton;

    private CookingStation currentStation;

    void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        cookingPanel.SetActive(false);
        cookButton.onClick.AddListener(OnCookButtonPressed);
    }

    public void OpenCookingUI(CookingStation station)
    {
        currentStation = station;
        cookingPanel.SetActive(true);
        UpdatePlayerInventoryUI();
        UpdateRecipeSlotsUI();
    }

    public void CloseCookingUI()
    {
        currentStation.ClearIngredients(); // Batalkan jika UI ditutup
        currentStation = null;
        cookingPanel.SetActive(false);
    }

    // Menampilkan item-item yang dimiliki player.
    void UpdatePlayerInventoryUI()
    {
        foreach (Transform child in playerInventoryParent) Destroy(child.gameObject);

        foreach (var item in InventoryManager.instance.items)
        {
            GameObject slotGO = Instantiate(inventorySlotPrefab, playerInventoryParent);
            slotGO.transform.Find("ItemIcon").GetComponent<Image>().sprite = item.itemIcon;

            // Tambahkan tombol agar bisa diklik untuk dimasukkan ke resep.
            Button itemButton = slotGO.AddComponent<Button>();
            itemButton.onClick.AddListener(() => AddIngredientToRecipe(item));
        }
    }

    // Menampilkan bahan yang sudah dimasukkan ke slot resep.
    void UpdateRecipeSlotsUI()
    {
        // Implementasi ini bisa lebih kompleks,
        // untuk sekarang kita biarkan kosong dan fokus pada fungsionalitas.
        // Kamu bisa menampilkan slot kosong sesuai resep yang mungkin.
    }

    void AddIngredientToRecipe(ItemData item)
    {
        currentStation.AddIngredient(item);
        // Di sini kamu bisa update UI slot resep untuk menampilkan item yang baru ditambahkan.
        Debug.Log(item.itemName + " ditambahkan ke slot resep.");
    }

    void OnCookButtonPressed()
    {
        currentStation.StartCooking();
        cookingPanel.SetActive(false); // Tutup UI setelah menekan masak
    }
}

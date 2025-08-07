using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mengelola logika untuk sebuah piring yang bisa diisi, dipegang, dan disajikan.
/// Versi ini tidak lagi menangani respawn-nya sendiri.
/// </summary>
public class PlateStation : MonoBehaviour
{
    [Header("Sprite State")]
    public Sprite emptySprite; // Sprite piring kosong
    public Sprite filledSprite; // Sprite piring terisi

    [Header("Status")]
    private ItemData heldDish; // Makanan apa yang ada di piring
    private bool isFilled = false;
    private Vector3 originalPosition; // Posisi awal untuk respawn
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.position; // Simpan posisi awal
        ResetPlate();
    }

    /// <summary>
    /// Logika utama saat player berinteraksi dengan piring.
    /// </summary>
    public void Interact(PlayerInteract player)
    {
        if (!isFilled)
        {
            PlatingUIManager.instance.OpenPlatingUI(this);
        }
        else if (isFilled && !player.IsHoldingObject())
        {
            player.HoldObject(this.gameObject);
            GetComponent<Collider2D>().enabled = false;
        }
    }

    /// <summary>
    /// Mengisi piring dengan masakan dari UI.
    /// </summary>
    public void FillPlate(ItemData dish)
    {
        isFilled = true;
        heldDish = dish;
        spriteRenderer.sprite = filledSprite; // Ganti sprite menjadi terisi
        InventoryManager.instance.RemoveItem(dish); // Hapus masakan dari inventory
    }

    /// <summary>
    /// Method publik yang dipanggil dari luar untuk mereset dan mengaktifkan piring.
    /// </summary>
    public void ResetAndReactivate()
    {
        transform.position = originalPosition;
        ResetPlate();
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Mereset piring ke keadaan kosong.
    /// </summary>
    private void ResetPlate()
    {
        isFilled = false;
        heldDish = null;
        spriteRenderer.sprite = emptySprite;
        GetComponent<Collider2D>().enabled = true;
    }

    public ItemData GetHeldDish()
    {
        return heldDish;
    }
}

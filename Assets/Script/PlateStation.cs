using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Mengelola logika untuk sebuah piring yang bisa diisi, dipegang, dan disajikan.
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
        // Jika piring kosong, buka UI untuk menaruh masakan.
        if (!isFilled)
        {
            // Buka UI Plating, hanya tampilkan masakan jadi dari inventory.
            PlatingUIManager.instance.OpenPlatingUI(this);
        }
        // Jika piring terisi dan player tidak sedang memegang apa-apa.
        else if (isFilled && !player.IsHoldingObject())
        {
            // Player mengambil piring ini.
            player.HoldObject(this.gameObject);
            // Nonaktifkan collider agar tidak bisa di-klik saat dipegang.
            GetComponent<Collider2D>().enabled = false;
        }
    }

    /// <summary>
    /// Mengisi piring dengan masakan dari UI.
    /// </summary>
    public void FillPlate(ItemData dish)
    {
        // Pastikan yang dimasukkan adalah masakan jadi (bukan bahan mentah).
        // Anda bisa menambahkan pengecekan tipe item di ItemData jika perlu.
        isFilled = true;
        heldDish = dish;
        spriteRenderer.sprite = filledSprite; // Ganti sprite menjadi terisi
        InventoryManager.instance.RemoveItem(dish); // Hapus masakan dari inventory
    }

    /// <summary>
    /// Memulai proses untuk respawn.
    /// </summary>
    public void StartRespawn()
    {
        StartCoroutine(RespawnAfterDelay(2f));
    }

    private IEnumerator RespawnAfterDelay(float delay)
    {
        // Tunggu selama durasi yang ditentukan.
        yield return new WaitForSeconds(delay);

        // Pindahkan piring kembali ke posisi awal.
        transform.position = originalPosition;
        // Reset semua statusnya.
        ResetPlate();
        // Aktifkan kembali agar bisa digunakan.
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
}

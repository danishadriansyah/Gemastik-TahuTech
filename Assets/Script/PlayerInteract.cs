using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Versi baru yang bisa memegang objek dan berinteraksi dengan lebih banyak stasiun.
/// </summary>
public class PlayerInteract : MonoBehaviour
{
    [Header("Pengaturan Interaksi")]
    public float interactionRange = 2f;
    public KeyCode inventoryKey = KeyCode.I;
    public Transform holdPoint; // Referensi ke HoldPoint di depan player
    public LayerMask interactionLayer;

    [Header("Referensi UI")]
    public GameObject inventoryUIPanel;

    private Camera mainCamera;
    private GameObject heldObject = null; // Objek yang sedang dipegang

    void Start()
    {
        mainCamera = Camera.main;
        if (inventoryUIPanel != null) inventoryUIPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleInteractionClick();
        }

        if (Input.GetKeyDown(inventoryKey))
        {
            if (inventoryUIPanel != null) inventoryUIPanel.SetActive(!inventoryUIPanel.activeSelf);
        }
    }

    private void HandleInteractionClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, interactionLayer);

        // DEBUG: Cek apakah sinar mengenai sesuatu
        if (hit.collider == null)
        {
            Debug.Log("DEBUG: Klik mouse tidak mengenai objek apapun yang punya Collider2D.");
            return;
        }

        // DEBUG: Tampilkan nama objek yang diklik
        Debug.Log("DEBUG: Klik mengenai objek: " + hit.collider.name);

        // Jika player sedang memegang sesuatu...
        if (heldObject != null)
        {
            // Cek apakah yang diklik adalah area penyajian.
            ServeStation serveStation = hit.collider.GetComponent<ServeStation>();
            if (serveStation != null)
            {
                serveStation.Serve(heldObject);
                ReleaseObject(); // Lepaskan objek dari tangan player
            }
        }
        // Jika player TIDAK memegang apa-apa...
        else
        {
            // Cek jarak dulu, jika terlalu jauh, hentikan.
            float distance = Vector2.Distance(transform.position, hit.collider.transform.position);
            if (distance > interactionRange)
            {
                Debug.Log("DEBUG: Objek " + hit.collider.name + " terlalu jauh.");
                return;
            }

            // --- Pengecekan Berurutan ---
            Debug.Log("DEBUG: Memulai pengecekan komponen pada " + hit.collider.name);

            // Coba cari komponen PlateStation, jika ada, jalankan dan berhenti.
            PlateStation plateStation = hit.collider.GetComponent<PlateStation>();
            if (plateStation != null)
            {
                Debug.Log("DEBUG: Komponen PlateStation ditemukan! Menjalankan interaksi piring.");
                plateStation.Interact(this);
                return;
            }

            // Jika bukan piring, coba cari CookingStation.
            CookingStation cookingStation = hit.collider.GetComponent<CookingStation>();
            if (cookingStation != null)
            {
                Debug.Log("DEBUG: Komponen CookingStation ditemukan! Menjalankan interaksi memasak.");
                cookingStation.Interact();
                return;
            }

            // Jika bukan juga, coba cari ItemStation (bahan).
            ItemStation itemStation = hit.collider.GetComponent<ItemStation>();
            if (itemStation != null)
            {
                Debug.Log("DEBUG: Komponen ItemStation ditemukan! Menjalankan interaksi bahan.");
                itemStation.Interact();
                return;
            }

            Debug.Log("DEBUG: Tidak ada komponen interaksi yang ditemukan pada " + hit.collider.name);
        }
    }

    // Fungsi untuk memegang objek
    public void HoldObject(GameObject objectToHold)
    {
        heldObject = objectToHold;
        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
    }

    // Fungsi untuk melepaskan objek
    public void ReleaseObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.SetParent(null);
            heldObject = null;
        }
    }

    // Cek apakah player sedang memegang sesuatu
    public bool IsHoldingObject()
    {
        return heldObject != null;
    }
}
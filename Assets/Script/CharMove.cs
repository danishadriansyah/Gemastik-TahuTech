using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMove : MonoBehaviour
{
    // Kecepatan gerak karakter, bisa diubah melalui Inspector Unity
    public float moveSpeed = 5f;

    // Variabel untuk menyimpan referensi ke komponen Rigidbody2D, Animator, dan SpriteRenderer
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer; // Variabel untuk Sprite Renderer

    // Variabel untuk menyimpan input gerakan dari player
    private Vector2 moveInput;

    // Start dipanggil sebelum frame pertama update
    void Start()
    {
        // Mengambil komponen Rigidbody2D yang terpasang pada GameObject ini
        rb = GetComponent<Rigidbody2D>();
        // Mengambil komponen Animator yang terpasang pada GameObject ini
        animator = GetComponent<Animator>();
        // Mengambil komponen SpriteRenderer yang terpasang pada GameObject ini
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update dipanggil setiap frame
    void Update()
    {
        // --- Input Handling ---
        // Membaca input horizontal (tombol A dan D, atau panah kiri/kanan)
        float moveX = Input.GetAxisRaw("Horizontal");
        // Membaca input vertikal (tombol W dan S, atau panah atas/bawah)
        float moveY = Input.GetAxisRaw("Vertical");

        // Menyimpan input ke dalam sebuah Vector2
        moveInput = new Vector2(moveX, moveY);

        // --- Animator Control ---
        // Mengirim data input ke Animator untuk mengontrol animasi
        // Pastikan Anda memiliki parameter "Horizontal", "Vertical", dan "Speed" di Animator Anda
        animator.SetFloat("Horizontal", moveInput.x);
        animator.SetFloat("Vertical", moveInput.y);
        animator.SetFloat("Speed", moveInput.sqrMagnitude); // sqrMagnitude lebih efisien daripada magnitude

        // --- Sprite Flipping ---
        // Membalik sprite berdasarkan arah input horizontal
        if (moveInput.x > 0)
        {
            // Jika bergerak ke kanan, jangan balik sprite
            spriteRenderer.flipX = false;
        }
        else if (moveInput.x < 0)
        {
            // Jika bergerak ke kiri, balik sprite secara horizontal
            spriteRenderer.flipX = true;
        }
    }

    // FixedUpdate dipanggil pada interval waktu yang tetap, cocok untuk fisika
    void FixedUpdate()
    {
        // --- Movement ---
        // Menggerakkan karakter menggunakan Rigidbody2D
        // Menggunakan moveInput.normalized agar kecepatan tetap konsisten saat bergerak diagonal
        // Time.fixedDeltaTime memastikan gerakan mulus dan tidak terpengaruh oleh frame rate
        rb.MovePosition(rb.position + moveInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}

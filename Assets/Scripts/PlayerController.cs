using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;

    // Sprite per arah
    public Sprite[] idleFrames;
    public Sprite[] runUpFrames;
    public Sprite[] runDownFrames;
    public Sprite[] runLeftFrames;
    public Sprite[] runRightFrames;

    // Suara langkah
    public AudioClip[] footstepClips;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private AudioSource audioSource;
    private Vector2 moveInput;

    private float frameRate = 0.15f;
    private float frameTimer;
    private int currentFrame;

    private enum Direction { Idle, Up, Down, Left, Right }
    private Direction currentDirection = Direction.Idle;

    private bool forceIdle = false;

    public void ForceIdleState(bool idle)
    {
        forceIdle = idle;
        if (idle)
        {
            moveInput = Vector2.zero;
            rb.velocity = Vector2.zero;
            currentDirection = Direction.Idle;
        }
    }

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        HandleInput();
        Animate();
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    void HandleInput()
    {
        if (forceIdle)
        {
            moveInput = Vector2.zero;
            currentDirection = Direction.Idle;
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(moveX, moveY).normalized;

        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveY) > Mathf.Abs(moveX))
                currentDirection = moveY > 0 ? Direction.Up : Direction.Down;
            else
                currentDirection = moveX > 0 ? Direction.Right : Direction.Left;
        }
        else
        {
            currentDirection = Direction.Idle;
        }
    }

    void Animate()
    {
        frameTimer += Time.deltaTime;

        Sprite[] currentFrames = GetCurrentFrames();
        if (currentFrames.Length == 0) return;

        if (frameTimer >= frameRate)
        {
            frameTimer = 0f;
            currentFrame++;

            if (currentFrame >= currentFrames.Length)
                currentFrame = 0;

            sr.sprite = currentFrames[currentFrame];

            // 🔊 Mainkan suara langkah jika sedang berjalan
            if (currentDirection != Direction.Idle && footstepClips.Length > 0)
            {
                int clipIndex = currentFrame % footstepClips.Length;

                // Putar hanya jika belum diputar di frame ini
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = footstepClips[clipIndex];
                    audioSource.Play();
                }
            }
        }
    }

    Sprite[] GetCurrentFrames()
    {
        switch (currentDirection)
        {
            case Direction.Up: return runUpFrames;
            case Direction.Down: return runDownFrames;
            case Direction.Left: return runLeftFrames;
            case Direction.Right: return runRightFrames;
            case Direction.Idle: return idleFrames;
            default: return idleFrames;
        }
    }

}



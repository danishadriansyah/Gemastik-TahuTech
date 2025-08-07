using UnityEngine;
using UnityEngine.SceneManagement;
using DialogueSystem;

public class ExitTrigger : MonoBehaviour
{
    public PlayerController playerController;
    public ChairTrigger chairTrigger; // Referensi ke ChairTrigger untuk mengecek dialognya sudah selesai

    [Header("Debug")]
    public bool showTriggerArea = true;
    public Color triggerAreaColor = new Color(0, 0, 1, 0.2f); // Biru untuk membedakan dengan trigger lain

    private bool hasTriggeredExit = false;
    private BoxCollider2D triggerCollider;

    private void Start()
    {
        // Get BoxCollider2D component
        triggerCollider = GetComponent<BoxCollider2D>();
        if (triggerCollider == null)
        {
            Debug.LogError("ExitTrigger membutuhkan BoxCollider2D! Menambahkan secara otomatis...");
            triggerCollider = gameObject.AddComponent<BoxCollider2D>();
            triggerCollider.isTrigger = true;
        }

        // Subscribe ke event dialog end
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.onDialogEnd.AddListener(OnDialogEnd);
        }
        else
        {
            Debug.LogWarning("DialogManager tidak ditemukan di scene!");
        }

        // Validasi komponen
        if (playerController == null)
        {
            Debug.LogError("PlayerController belum di-assign pada ExitTrigger!");
        }
        if (chairTrigger == null)
        {
            Debug.LogError("ChairTrigger belum di-assign pada ExitTrigger!");
        }
    }

    private void OnDrawGizmos()
    {
        if (!showTriggerArea) return;

        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.color = triggerAreaColor;
            Vector3 center = transform.position + (Vector3)boxCollider.offset;
            Vector3 size = boxCollider.size;
            Gizmos.DrawCube(center, size);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"ExitTrigger entered by: {other.gameObject.name}");
        Debug.Log($"hasTriggeredExit: {hasTriggeredExit}");
        Debug.Log($"chairTrigger is null: {chairTrigger == null}");
        if (chairTrigger != null)
        {
            Debug.Log($"chairTrigger.dialogStarted: {chairTrigger.dialogStarted}");
        }
        Debug.Log($"Has Player tag: {other.CompareTag("Player")}");

        // Cek apakah player sudah menyelesaikan dialog di kursi
        if (!hasTriggeredExit && chairTrigger != null && chairTrigger.dialogStarted && other.CompareTag("Player"))
        {
            Debug.Log("Exit dialog trigger activated!");
            hasTriggeredExit = true;

            playerController.ForceIdleState(true);

            if (DialogManager.Instance != null)
            {
                DialogManager.Instance.StartDialog(new DialogueLine[]
                {
                    new DialogueLine { speakerName = "Denis", sentence = "Aku harus segera berangkat ke Jogja!" }
                });
            }
        }
    }

    private void OnDialogEnd()
    {
        if (hasTriggeredExit)
        {
            // Load scene berikutnya
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            playerController.ForceIdleState(false);
        }
    }

    private void OnDestroy()
    {
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.onDialogEnd.RemoveListener(OnDialogEnd);
        }
    }
}

using UnityEngine;
using DialogueSystem;

public class ChairTrigger : MonoBehaviour
{
    public PlayerController playerController;
    
    [Header("Dialog Settings")]
    public DialogueLine[] dialogueLines;

    private DialogueLine CreateDialogueLine(string speaker, string text)
    {
        DialogueLine line = new DialogueLine();
        line.speakerName = speaker;
        line.sentence = text;
        return line;
    }

    [Header("Debug")]
    public bool showTriggerArea = true;
    public Color triggerAreaColor = new Color(1, 0, 0, 0.2f);

    public bool dialogStarted = false; // Made public so ExitTrigger can check its state
    private BoxCollider2D triggerCollider;

    private void Start()
    {
        // Get BoxCollider2D component
        triggerCollider = GetComponent<BoxCollider2D>();
        if (triggerCollider == null)
        {
            Debug.LogError("ChairTrigger membutuhkan BoxCollider2D! Menambahkan secara otomatis...");
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

        // Validasi komponen yang dibutuhkan
        if (playerController == null)
        {
            Debug.LogError("PlayerController belum di-assign pada ChairTrigger!");
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
        Debug.Log($"Trigger entered by: {other.gameObject.name} with tag: {other.tag}");
        
        if (!dialogStarted && other.CompareTag("Player"))
        {
            Debug.Log("Dialog trigger activated!");
            dialogStarted = true;

            // Set animasi player ke idle
            Animator playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("MoveX", 0);
                playerAnimator.SetFloat("MoveY", 0);
                playerAnimator.SetBool("isMoving", false);
            }

            playerController.ForceIdleState(true);

            // Mulai dialog
            if (DialogManager.Instance != null)
            {
                if (dialogueLines == null || dialogueLines.Length == 0)
                {
                    // Dialog default jika belum diset
                    dialogueLines = new DialogueLine[]
                    {
                        new DialogueLine { speakerName = "Denis (Monolog)", sentence = "Sejarah Kuliner Indonesia... Tiga kata yang terasa lebih berat dari tumpukan buku di depanku..." },
                        new DialogueLine { speakerName = "Denis", sentence = "(Mengangkat ponsel) Halo, Nadia? Ini aku, Denis. Maaf ganggu malam-malam." },
                        new DialogueLine { speakerName = "Nadia", sentence = "Denis! Tumben sekali telepon. Ada apa? Kamu baik-baik saja di Jakarta?" },
                        new DialogueLine { speakerName = "Denis", sentence = "Aku... baik. Dengar, Nad, aku lagi butuh bantuan..." },
                        new DialogueLine { speakerName = "Nadia", sentence = "Denis... ada sesuatu yang harus kamu tahu. Kenapa kamu tidak pulang waktu itu?" },
                        new DialogueLine { speakerName = "Nadia", sentence = "Denis... Kakek Kasiman... sudah berpulang. Seminggu yang lalu." },
                        new DialogueLine { speakerName = "Denis", sentence = "Apa...? Tidak mungkin... Aku... aku baru saja mau bertanya resep padanya." },
                        new DialogueLine { speakerName = "Nadia", sentence = "Maafkan aku, Denis." },
                        new DialogueLine { speakerName = "Denis (Monolog)", sentence = "Resep... bukan masakan yang aku cari. Tapi resep untuk pulang. Dan sekarang... aku sudah kehilangan arah." }
                    };
                }
                DialogManager.Instance.StartDialog(dialogueLines);
            }
        }
    }

    private void OnDialogEnd()
    {
        Debug.Log("Chair dialog ended");
        // Dialog tetap dianggap sudah dimulai agar ExitTrigger bisa mengecek bahwa dialog kursi sudah selesai
        playerController.ForceIdleState(false);
    }

    private void OnDestroy()
    {
        // Unsubscribe dari event
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.onDialogEnd.RemoveListener(OnDialogEnd);
        }
    }
}

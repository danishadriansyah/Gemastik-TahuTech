using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using DialogueSystem;

public class DialogManager : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject dialogPanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public float typingSpeed = 0.05f;

    [Header("Events")]
    public UnityEvent onDialogStart;
    public UnityEvent onDialogEnd;

    private DialogueLine[] dialogueLines;
    private int currentLine = 0;
    private bool isTyping = false;
    private bool dialogActive = false;

    private static DialogManager instance;
    public static DialogManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        dialogPanel.SetActive(false);
    }

    void Update()
    {
        if (!dialogActive)
            return;

        // Jika pemain menekan tombol continue (Space atau Click)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Skip typing animation
                StopAllCoroutines();
                DialogueLine currentDialogue = dialogueLines[currentLine];
                dialogText.text = currentDialogue.sentence;
                isTyping = false;
            }
            else
            {
                NextLine();
            }
        }
    }

    public void StartDialog(DialogueLine[] lines)
    {
        dialogueLines = lines;
        currentLine = 0;
        dialogActive = true;
        dialogPanel.SetActive(true);
        
        onDialogStart.Invoke();
        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (currentLine < dialogueLines.Length)
        {
            isTyping = true;
            DialogueLine currentDialogue = dialogueLines[currentLine];
            nameText.text = currentDialogue.speakerName;
            StartCoroutine(TypeLine(currentDialogue.sentence));
        }
        else
        {
            EndDialog();
        }
    }

    private void NextLine()
    {
        currentLine++;
        ShowCurrentLine();
    }

    private IEnumerator TypeLine(string line)
    {
        dialogText.text = "";
        foreach (char c in line.ToCharArray())
        {
            dialogText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    public void EndDialog()
    {
        dialogActive = false;
        dialogPanel.SetActive(false);
        onDialogEnd.Invoke();
    }
}

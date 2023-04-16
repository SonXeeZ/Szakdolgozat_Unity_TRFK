using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance { get; set; }
    public GameObject dialoguePanel;
    public string npcName;
    public List<string> dialogueLines = new List<string>();
    [SerializeField] public Button continueButton;

    TMPro.TextMeshProUGUI dialogueText, nameText;
    int dialogueIndex;

    // Use this for initialization
    void Awake () {
        dialoguePanel = GameObject.Find("Dialogue");
        continueButton = dialoguePanel.GetComponentInChildren<Button>();
        dialogueText = dialoguePanel.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        nameText = dialoguePanel.transform.Find("Name").GetChild(0).GetComponentInChildren<TMPro.TextMeshProUGUI>();
        continueButton.onClick.AddListener(delegate { ContinueDialogue(); } );
        dialoguePanel.SetActive(false);


	    if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
	}
	
    public void AddNewDialogue(string[] lines, string npcName)
    {
        Debug.Log("Adding new dialogue.");
        dialogueIndex = 0;
        dialogueLines = new List<string>();
        foreach(string line in lines)
        {
            dialogueLines.Add(line);
        }
        this.npcName = npcName;

        CreateDialogue();
    }

    public void CreateDialogue()
    {
        dialogueText.text = dialogueLines[dialogueIndex];
        nameText.text = npcName;
        dialoguePanel.SetActive(true);
        Debug.Log("Dialogue Created.");
    }

    public void ContinueDialogue()
    {
        Debug.Log("Continuing Dialogue.");
        if (dialogueIndex < dialogueLines.Count-1)
        {
            dialogueIndex++;
            dialogueText.text = dialogueLines[dialogueIndex];
        }
        else
        {
            dialoguePanel.SetActive(false);
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;

    [SerializeField] Text dialogName;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject choicesPanel;
    [SerializeField] int letterPerSecond;

    [SerializeField] GameObject choicePrefab;
    public event Action OnShowDialog;
    public event Action OnHideDialog;
    public static DialogManager Instance { get; private set; }
    Dialog dialog;
    bool isPrinting;
    private void Awake()
    {
        Instance = this;
    }
    int currentNode = 0;
    int currentChoiceIndex = 0;

    public void handleUpdate()
    {
        if (choicesPanel.activeSelf)
        {
            HandleChoiceInput();
            return;
        }

        if (Input.GetKeyDown(KeyCode.T) && !isPrinting)
        {
            ++currentNode;
            if (currentNode < dialog.Lines.Count)
            {
                HandleNode(dialog.Lines[currentNode]);
            }
            else
            {
                currentNode = 0;
                dialogBox.SetActive(false);
                choicesPanel.SetActive(false);
                OnHideDialog?.Invoke();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentNode = 0;
            dialogBox.SetActive(false);
            choicesPanel.SetActive(false);
            OnHideDialog?.Invoke();
        }
    }


    public IEnumerator showDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;

        dialogBox.SetActive(true);
        dialogName.text = dialog.Name;
        HandleNode(dialog.Lines[currentNode]);

    }


    public IEnumerator TypeDialog(string line)
    {
        isPrinting = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / Mathf.Max(letterPerSecond, 1));

        }
        isPrinting = false;
    }
    public void HandleNode(Node node)
    {

        if (node is ChoiceNode choiceNode)
        {
            StartCoroutine(TypeDialog(node.Content));
            ShowChoices(choiceNode);
        }
        else
        {
            StartCoroutine(TypeDialog(node.Content));
        }

    }

    private void ShowChoices(ChoiceNode node)
    {
        choicesPanel.SetActive(true);

        // Clear old choices
        foreach (Transform child in choicesPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Add new choices
        for (int i = 0; i < node.Choices.Count; i++)
        {
            var choiceObj = Instantiate(choicePrefab, choicesPanel.transform);
            var choiceText = choiceObj.GetComponent<Text>();
            choiceText.text = node.Choices[i].ChoiceText;

            if (i == currentChoiceIndex)
                choiceText.color = Color.red; // Highlighted choice
            else
                choiceText.color = Color.black;

            choiceObj.name = $"Choice_{i}";
        }
    }



    private void HandleChoiceInput()
    {
        // Check if the current node is a ChoiceNode
        if (dialog.Lines[currentNode] is ChoiceNode choiceNode)
        {
            // Navigate choices
            if (Input.GetKeyDown(KeyCode.A))
            {
                currentChoiceIndex = (currentChoiceIndex - 1 + choiceNode.Choices.Count) % choiceNode.Choices.Count;
                UpdateChoices(choiceNode);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                currentChoiceIndex = (currentChoiceIndex + 1) % choiceNode.Choices.Count;
                UpdateChoices(choiceNode);
            }
            else if (Input.GetKeyDown(KeyCode.T))
            {
                // Execute the selected choice action
                                choicesPanel.SetActive(false);
                choiceNode.Choices[currentChoiceIndex].OnSelect?.Invoke();

                // Hide the choices panel and dialog box
                currentNode = 0;

                dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
                
            }
        }
        else
        {
            Debug.LogWarning("Current node is not a ChoiceNode. Choices cannot be displayed.");
        }
    }
    private void UpdateChoices(ChoiceNode choiceNode)
    {
        for (int i = 0; i < choicesPanel.transform.childCount; i++)
        {
            var child = choicesPanel.transform.GetChild(i);
            var choiceText = child.GetComponent<UnityEngine.UI.Text>();

            // Highlight the currently selected choice
            if (i == currentChoiceIndex)
                choiceText.color = Color.red; // Highlighted choice
            else
                choiceText.color = Color.black;
        }
    }

}

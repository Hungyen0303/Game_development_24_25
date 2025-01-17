using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;
    [SerializeField] int letterPerSecond;
    public event Action OnShowDialog;
    public event Action OnHideDialog;
    public static DialogManager Instance { get; private set; }
    Dialog dialog;
    bool isPrinting;
    private void Awake()
    {
        Instance = this;
    }
    int currentline = 0;

    public void handleUpdate()
    {
        if (Input.GetKeyDown(KeyCode.T) && !isPrinting)
        {
            Debug.Log(currentline);
            ++currentline;
            if (currentline < dialog.Lines.Count)
            {
                StartCoroutine(TypeDialog(dialog.Lines[currentline]));
            }
            else
            {
                currentline =0;
                dialogBox.SetActive(false);
                OnHideDialog?.Invoke();
            }
        }
    }

    
    public IEnumerator showDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();
        OnShowDialog?.Invoke();
        this.dialog = dialog;

        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(dialog.Lines[0]));
    }
    public IEnumerator TypeDialog(string line)
    {
        isPrinting = true;
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / letterPerSecond);
        }
        isPrinting = false;
    }
}

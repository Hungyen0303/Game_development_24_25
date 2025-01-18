using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoannaController : MonoBehaviour, Interactable
{

    [SerializeField] TMPro.TextMeshProUGUI nameText;

    public Dialog dialog;
    private void Awake()
    {
        dialog = new Dialog
        {
            Name = "Joanna",
            Lines = new List<Node>
            {
                new DialogueNode("Chào mừng, hỡi hiệp sĩ vô danh."),
                new DialogueNode("Hãy để ta giúp đỡ ngài mở khóa tiềm năng."),
                new ChoiceNode("Hãy lựa chọn hướng cường hóa:",
                    new List<(string, Action)>
                    {
                        ("Sức mạnh", () => increaseATK()),
                        ("Thể chất", () => increaseHP()),   
                        ("Tốc độ", () => increaseSPD())
                    })
            }
        };
    }


    private static Action increaseSPD()
    {

        return () =>
        {
            Node node = new DialogueNode("Nguyện gió Tây dẫn lối Ngài.");
            DialogManager.Instance.HandleNode(node);
            Debug.Log("Tăng HP cho nhân vật!");
        };
    }

    private static Action increaseHP()
    {
        return () =>
        {
            DialogManager.Instance.HandleNode(new DialogueNode("Mong mọi dãy núi bảo hộ Ngài"));
            Debug.Log("Tăng HP cho nhân vật!");
        };
    }

    private static Action increaseATK()
    {

        return () =>
        {
            Node node = new DialogueNode("Mọi vũ khí trở nên sắc lẹm trong tay Ngài");
            DialogManager.Instance.HandleNode(node);
            Debug.Log("Tăng HP cho nhân vật!");
        };
    }

    private void Start()
    {
        if (dialog.Name != null)
        {
            nameText.text = dialog.Name;
        }
        else {
            Debug.Log(dialog);
        }
    }

    public void Interact()
    {
        StartCoroutine(DialogManager.Instance.showDialog(dialog));
    }


}


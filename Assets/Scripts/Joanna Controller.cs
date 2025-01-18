using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JoannaController : MonoBehaviour, Interactable
{
    public static PlayerController player;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    private int limit = 3;
    public Dialog dialog;
    public Dialog dialog2;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        dialog = new Dialog
        {
            Name = "Joanna",
            Lines = new List<Node>
            {
                new DialogueNode("Chào mừng, hỡi hiệp sĩ vô danh."),
                new DialogueNode("Hãy để ta giúp đỡ ngươi mở khóa tiềm năng. Ngài có 3 lần lựa chọn"),
                new ChoiceNode("Hãy chọn thuộc tính cần cường hóa:",
                    new List<(string, Action)>
                    {
                        ("Sức mạnh", static () => increaseATK()),
                        ("Thể chất", static () => increaseHP()),
                        ("Tốc độ", static () => increaseSPD())
                    })
            }
        };
        dialog = new Dialog
        {
            Name = "Joanna",
            Lines = new List<Node>
            {
                new DialogueNode("Ta không còn gì để dạy ngài nữa")
                }
        };
    }


    private static Action increaseSPD()
    {
        player.airWalkSpeed *= 1.1f;
        player.walkSpeed *= 1.1f;
        player.runSpeed *= 1.1f;
        Node node = new DialogueNode("Cuồng phong sẽ dẫn lối ngươi đi");
        DialogManager.Instance.HandleNode(node);
        return static () =>
        {        };
    }

    private static Action increaseHP()
    {
        player.damagable.MaxHealth += 20;
        player.damagable.health += 20;
        Node node = new DialogueNode("Kế thừa ý chí bảo hộ");
        DialogManager.Instance.HandleNode(node);
        return static () =>
        {

        };
    }

    private static Action increaseATK()
    {
        player.damage +=5;
        Node node = new DialogueNode("Thông thạo nhiều loại vũ khí luôn tốt hơn là không biết gì cả ");
        DialogManager.Instance.HandleNode(node);
        return static () =>
        {        };
    }

    private void Start()
    {
        if (dialog.Name != null)
        {
            nameText.text = dialog.Name;
        }
        else
        {
            Debug.Log(dialog);
        }
    }

    public void Interact()
    {
        if (limit > 0)
        {
            StartCoroutine(DialogManager.Instance.showDialog(dialog));
            limit--;
        }
        else
        {
            StartCoroutine(DialogManager.Instance.showDialog(dialog2));
        }
    }


}


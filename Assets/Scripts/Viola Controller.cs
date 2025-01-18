using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViolaController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] Dialog dialog2;
    public static PlayerController player;
    int count = 1;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        dialog = new Dialog
        {
            Name = "Viola",
            Lines = new List<Node>
            {
                new DialogueNode("Làm tốt lắm."),
                new DialogueNode("Mặc dù đây chỉ là một khu huấn luyện, nhưng nhìn chung bản năng chiến đấu của ngươi vẫn còn như xưa."),
                new DialogueNode("Hãy thử đối đầu với thử thách khó hơn xem nào"),
                              new DialogueNode("..."),
              new DialogueNode("Phục hồi!"),

            }
        };
        dialog2 = new Dialog
        {
            Name = "Viola",
            Lines = new List<Node>
            {
                new DialogueNode("Còn chưa xuất phát sao?"),
            }
        };
    }
    private void Start()
    {
        if (nameText != null)
        {
            nameText.text = dialog.Name;
        }
    }

    public void Interact()
    {

        if (count > 0)
        {
            StartCoroutine(DialogManager.Instance.showDialog(dialog));
            Heal();
            count--;
        }
        else
        {
            StartCoroutine(DialogManager.Instance.showDialog(dialog2));
        }


    }
    public void Heal()
    {
        player.damagable.health = player.damagable.MaxHealth;
    }
}

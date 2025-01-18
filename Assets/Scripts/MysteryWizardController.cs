using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryWizardController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    private void Awake()
    {
        dialog = new Dialog
        {
            Name = "Joanna",
            Lines = new List<Node>
            {
                new DialogueNode("Xin chào, hiệp sĩ lang thang."),
                new DialogueNode("Trước khi ngươi tiếp tục cuộc hành trình, hãy để ta hướng dẫn ngươi vài chiêu cơ bản."),
                new DialogueNode("Phím A, S, W, D là để di chuyển - tiến tới, lùi lại, hoặc né tránh hiểm nguy."),
                new DialogueNode("Phím T để tương tác các nhân viên khác. Phím F cho phép ngươi bắn tên từ xa."),
                new DialogueNode("Và cuối cùng, dùng chuột trái để tung những đòn đánh cận chiến đầy uy lực"),
                                new DialogueNode("Chúc may mắn"),
                 
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

    public void Interact(){
        StartCoroutine(DialogManager.Instance.showDialog(dialog));
        
    }
}

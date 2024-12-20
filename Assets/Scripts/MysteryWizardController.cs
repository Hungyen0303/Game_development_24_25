using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryWizardController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    public void Interact(){
        // Debug.Log("interactNPC1");
        StartCoroutine(DialogManager.Instance.showDialog(dialog));
    }
}

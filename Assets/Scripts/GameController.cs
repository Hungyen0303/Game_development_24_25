using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public enum GameState
{
    Battle, Dialog
}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    GameState gameState ;
    private void Start()
    {
        DialogManager.Instance.OnShowDialog += () =>
        {
            gameState = GameState.Dialog;
        };
        DialogManager.Instance.OnHideDialog += () =>
        {
            if(gameState==GameState.Dialog)
            gameState = GameState.Battle;
        };
    }

    private void Update()
    {
        if (gameState == GameState.Battle)
        {
            playerController.handleUpdate();
        }
        else if (gameState == GameState.Dialog)
        {
            DialogManager.Instance.handleUpdate();
        }
    }
}

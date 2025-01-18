using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneController.instance.LoadLevel(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

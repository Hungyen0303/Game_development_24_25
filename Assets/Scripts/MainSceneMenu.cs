using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneMenu : MonoBehaviour
{
    [SerializeField] GameObject newGame;
    [SerializeField] GameObject continueGame;

    private void Start()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            newGame.SetActive(false);
            continueGame.SetActive(true);
            Debug.Log("Level: " + PlayerPrefs.GetInt("level"));
        }
        else
        {
            newGame.SetActive(true);
            continueGame.SetActive(false);
        }
    }
    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();
        SceneController.instance.LoadLevel(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        int level = PlayerPrefs.GetInt("level");
        SceneController.instance.LoadLevel(level);
    }
}

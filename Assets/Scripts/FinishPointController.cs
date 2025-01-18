using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishPointController : MonoBehaviour
{
    public int levelIndex;
    // win screen
    [SerializeField] GameObject winScreen;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && levelIndex <= 3)
        {
            SceneController.instance.LoadLevel(levelIndex);
        }
        else if (collision.CompareTag("Player") && levelIndex == 4)
        {
            winScreen.SetActive(true);
        }
    }
}

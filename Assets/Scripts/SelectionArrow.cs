using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionAround : MonoBehaviour
{
    [SerializeField] private RectTransform[] opitons;
    private int currentIndex;
    private RectTransform selection;
    [SerializeField] private AudioClip changeSound;
    [SerializeField] private AudioClip selectSound;
    private AudioSource audioSource;

    private void Awake()
    {
        selection = GetComponent<RectTransform>();
        audioSource = GetComponent<AudioSource>();
        currentIndex = 0;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            audioSource.PlayOneShot(selectSound);
            opitons[currentIndex].GetComponent<Button>().onClick.Invoke();
            Debug.Log("Selected: " + currentIndex);
        }
    }

    private void ChangeSelection(int _change)
    {
        if (currentIndex + _change >= 0 && currentIndex + _change < opitons.Length)
        {
            currentIndex += _change;
            audioSource.PlayOneShot(changeSound);
        }
        selection.position = new Vector3(selection.position.x, opitons[currentIndex].position.y, selection.position.z);
    }
}

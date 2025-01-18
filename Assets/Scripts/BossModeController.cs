using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossModeController : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject finishPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            boss.SetActive(true);
        }
    }

    // Check boss is alive
    private void Update()
    {
        if (boss == null)
        {
            finishPoint.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropWeapon : MonoBehaviour
{
    public GameObject[] weaponPrefabs;
    public void OnDeath()
    {
        if (weaponPrefabs.Length > 0)
        {
            int randomIndex = Random.Range(0, weaponPrefabs.Length);
            Instantiate(weaponPrefabs[randomIndex], transform.position, Quaternion.identity);
            Debug.Log("Here!");
        }
        Destroy(gameObject);
    }
}

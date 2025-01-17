using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform launchPoint;

    public GameObject ProjectilePrefab;
    public void FireProjectile()
    {
        GameObject projecile = Instantiate(ProjectilePrefab, launchPoint.position, ProjectilePrefab.transform.rotation);
        Vector3 oriScale = projecile.transform.localScale;
        projecile.transform.localScale = new Vector3(
            oriScale.x * transform.localScale.x > 0 ? 1 : -1,
            oriScale.y,
            oriScale.z);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

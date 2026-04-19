using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rotator : MonoBehaviour
{
    public bool randomYRotation = true;

    private void Start()
    {
        if (randomYRotation)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.y = Random.Range(0, 360);
            transform.rotation = Quaternion.Euler(rot);
        }
    }
}
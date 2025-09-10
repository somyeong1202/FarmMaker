using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Transform playerPos;

    Vector3 vector;
    private void Update()
    {
        vector = playerPos.position;
        vector.y = 0;

        transform.position = vector;
    }
}

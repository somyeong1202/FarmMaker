using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewerRot : MonoBehaviour
{
    public Transform playerTrans;
    public float rotSpeed;
    float rotY;
    float rotX;
    private void Update()
    {
        transform.position = playerTrans.position;

        float X = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        rotX += rotSpeed * X * Time.deltaTime;
        rotY += rotSpeed * y * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -90, 40); //범위 제한

        transform.localEulerAngles = new Vector3(-rotY,rotX,0); //마우스가 아래로 향하면 시점이 위로 가고 마우스가 위를 향하면 시점이 아래로 향하기 위해 -를 붙임

    }
}

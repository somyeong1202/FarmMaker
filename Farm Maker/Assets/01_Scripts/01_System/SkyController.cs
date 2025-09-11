using System.Collections;
using System.Collections.Generic;
using Ultrabolt.SkyEngine;
using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Transform playerPos;
    SkyCore skyCore;
    public float skySpeed;
    bool isSleepTime = false;
    private int sleepDayCount;
    Vector3 vector;

    private void Start()
    {
        skyCore = GetComponent<SkyCore>();
    }
    private void Update()
    {
        vector = playerPos.position;
        vector.y = transform.position.y;

        transform.position = vector;

        if(skyCore.lastTimeState == GameTime.Morning && isSleepTime && sleepDayCount < skyCore.dayCount)
        {
            skyCore.timeSpeed = skySpeed;

            PlayerController.ps.WakeUp();
            isSleepTime = false;
        }

        if (isSleepTime)
            return;

        if((PlayerController.ps.state == PlayerController.State.sleep || PlayerController.ps.state == PlayerController.State.KO))
        {
            skyCore.timeSpeed = 10;
            sleepDayCount = skyCore.dayCount;
            isSleepTime = true;
        }
    }
}

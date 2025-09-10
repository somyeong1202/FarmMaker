using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cc;
    public float moveSpeed;

    //회전 스피드(좌우) 상하 회전은 카메라에 적용 예정
    public float rotSpeed;

    //점프력
    public float jumpPower;
    private float yVelocity;
    private bool jumpAble = true;
    float gravity = -15f;

    public Transform CamPos;
    public GameObject camera;

    float Rot = 0; //회전값
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        camera.transform.parent = CamPos;
        camera.transform.localPosition = Vector3.zero;
    }
    private void Update()
    {
        // 1. 사용자의 입력을 받음
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // 2. 이동 방향을 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 플레이어의 회전값을 기준으로 이동 방향 전환
        dir = transform.TransformDirection(dir);

        // 2-2. 만일, 점프 중이었고, 다시 바닥에 착지했다면
        if (cc.collisionFlags == CollisionFlags.Below && jumpAble == false)
        {
            yVelocity = 0;
            jumpAble = true;
        }

        if (Input.GetKey(KeyCode.Space) && jumpAble)
        {
            yVelocity = jumpPower;
            jumpAble = false;
        }

       
        yVelocity += gravity * Time.deltaTime;

        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

        float rotX = Input.GetAxis("Mouse X");

        Rot += rotX * rotSpeed * Time.deltaTime;

        // 2. 회전 방향으로 물체를 회전시킴
        transform.eulerAngles = new Vector3(0, Rot, 0);
    }
}

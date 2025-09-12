using System.Collections;
using System.Collections.Generic;
using Ultrabolt.SkyEngine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController ps;

    private void Awake() //변수 playerState을 싱글톤으로 만들기 위한 것
    {
        if (ps == null)
            ps = this;
    }

    public enum State
    {
        idle,
        walk,
        run,
        sleep,
        KO
    }

    public State state;

    CharacterController cc;
    public float basicMoveSpeed; //기본 이동 속도
    private float moveSpeed; //현재 이동 속도

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

    //플레이어 체력
    HpController hp;

    //플레이어 기력
    public float maxStamina;
    private float stamina;

    //스태미너 소모되는 량;
    private float useStamina;

    //스태미너 바
    public Slider StaminaBar;

    //스태미너 피로 상태
    private bool isTired;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
        camera.transform.parent = CamPos;
        camera.transform.localPosition = Vector3.zero;

        hp = GetComponent<HpController>();

        moveSpeed = basicMoveSpeed;

        stamina = maxStamina;
        //stamina = -14.5f;

        state = State.idle;

        useStamina = 0.01f;

        StaminaBar.maxValue = maxStamina;
        StaminaBar.value = stamina;
    }

    private void Update()
    {
        if (state == State.sleep || state == State.KO) //플레이어가 잠을 자고 있거나 기절한 상태인 경우
        {
            return;
        }

        // 1. 사용자의 입력을 받음
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if( h == 0 && v == 0)
        {
            state = State.idle;
        }
        else if( state == State.idle )
        {
            state = State.walk;
        }
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
        else
            yVelocity += gravity * Time.deltaTime; //중력 적용, 떨어지는 속도

        //점프
        if (Input.GetKey(KeyCode.Space) && jumpAble)
        {
            yVelocity = jumpPower;
            jumpAble = false;
        }

        //달리기
        if(Input.GetKey(KeyCode.LeftShift) && state != State.run && stamina > 0)
        {
            moveSpeed *= 1.5f;
            state = State.run;
        }
        else if ((Input.GetKeyUp(KeyCode.LeftShift) || stamina <= 0) && !isTired) //달리기 멈춤
        {
            moveSpeed = basicMoveSpeed;
            state = State.idle;
        }

        if(state == State.run)
        {
            stamina -= 0.2f * Time.deltaTime;
        }


        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
        Debug.Log($"스피드 : {moveSpeed}");

        float rotX = Input.GetAxis("Mouse X");

        Rot += rotX * rotSpeed * Time.deltaTime;

        // 2. 회전 방향으로 물체를 회전시킴
        transform.eulerAngles = new Vector3(0, Rot, 0);


        if(stamina < 0 && !isTired)
        {
            moveSpeed /= 2;
            useStamina = 0.1f;
            isTired = true;
        }
        else if(isTired && stamina > 0)
        {
            moveSpeed = basicMoveSpeed;
            useStamina = 0.01f;
            isTired = false;
        }    

        stamina -= useStamina * Time.deltaTime; //실시간 스태미나 소모
        Debug.Log(stamina);
        StaminaBar.value = stamina;

        if(stamina <= -15f) //스태미너가 -15가 되었을 경우
        {
            state = State.KO;
            hp.Recovery();
        }
    }

    public void Sleeping()
    {
        state = State.sleep;
        hp.Recovery();
    }

    public void EatFood(float food)
    {
        stamina += food;
    }

    public void WakeUp()
    {
        if (state == State.KO)
        {
            Debug.Log("절반");
            stamina = maxStamina / 2;
        }
        else
        {
            stamina = maxStamina;
        }
        state = State.idle;
    }
}

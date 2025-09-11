using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    public float MaxHp;
    private float Hp;

    private void Start()
    {
        Hp = MaxHp;
    }

    public void Damaged(float dmg)
    {
        Hp -= dmg;

        if (Hp > 0)
            return;


    }

    public void LevelUp()
    {
        MaxHp += 5;
        Hp = MaxHp;
    }

    public IEnumerator Recovery() //회복 코루틴
    {
        while(Hp < MaxHp)
        {
            yield return new WaitForSeconds(0.1f);
            Hp += 0.1f;

        }

        if(Hp > MaxHp)
            Hp = MaxHp;
    }
}

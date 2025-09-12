using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpController : MonoBehaviour
{
    public float MaxHp;
    private float Hp;
    public Slider HpBar;
    private void Start()
    {
        Hp = MaxHp;
        HpBar.maxValue = MaxHp;
        HpBar.value = Hp;
    }

    public void Damaged(float dmg)
    {
        Hp -= dmg;
        HpBar.value = Hp;
        if (Hp > 0)
            return;


    }

    public void LevelUp()
    {
        MaxHp += 5;
        Hp = MaxHp;
        HpBar.maxValue = MaxHp;
        HpBar.value = Hp;
    }

    public IEnumerator Recovery() //회복 코루틴
    {
        while(Hp < MaxHp)
        {
            yield return new WaitForSeconds(0.1f);
            Hp += 0.1f;
            HpBar.value += 0.1f;
        }

        if(Hp > MaxHp)
            Hp = MaxHp;
    }
}

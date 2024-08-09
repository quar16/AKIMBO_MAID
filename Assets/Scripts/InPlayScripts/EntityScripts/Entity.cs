using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP;
    public int nowHP;

    public bool Damageable { get { return maxHP != 0; } }
    public int prefabId;


    public virtual void GetDamage(int damage)
    {
        ChangeHP(-damage);
    }

    public void ChangeHP(int delta)
    {
        if (Damageable == false) return;

        nowHP = Mathf.Clamp(nowHP + delta, 0, maxHP);

        if (nowHP <= 0)
        {
            OnZeroHP();
        }
    }

    public virtual void OnZeroHP() { }

    public virtual void Init(List<float> customData) { }

    public Vector3 CenterPoint { get { return transform.position + new Vector3(0.5f, 0.5f, 0); } }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum Option
{
    None = 0,
    IsFront = 1 << 0,
    IsBroken = 1 << 1,
    IsFire = 1 << 2
}

public class Obstacle_Laser : Obstacle
{
    float chargeTime;
    float fireTime;
    int range;

    Option isFront = Option.IsFront;
    Option isBroken = Option.None;
    Option isFire = Option.None;

    IEnumerator fireCycle;

    public List<Sprite> spriteList = new();
    public SpriteRenderer spriteRenderer;

    public Transform laserBeamT;
    public Animator laserBeam;
    public SpriteRenderer laserRady;

    public override void Init(List<float> customData)
    {
        isFront = customData[0] == 1 ? Option.IsFront : Option.None;

        if (isFront != Option.IsFront)
            transform.position += Vector3.up * 2;

        SetSprite();

        chargeTime = customData[1];
        fireTime = customData[2];
        range = (int)customData[3];

        fireCycle = FireCycle();
        StartCoroutine(fireCycle);
    }

    IEnumerator FireCycle()
    {
        if (isFront != Option.IsFront)
            laserBeamT.localEulerAngles = new Vector3(0, 0, 90);

        laserBeam.GetComponent<SpriteRenderer>().size = new Vector2(range, 1);
        laserRady.size = new Vector2(range, 1);

        while (nowHP > 0)
        {
            float time = Time.time;
            while (time + chargeTime > Time.time)
            {
                float t = (Time.time - time) / chargeTime;
                laserRady.color = new Color(1, 0, 0, t);
                laserRady.transform.localScale = new Vector3(1, 1 - t, 1);
                yield return PlayTime.ScaledNull;
            }
            laserBeam.SetTrigger("Fire");
            yield return PlayTime.ScaledWaitForSeconds(fireTime);
            laserBeam.SetTrigger("Stop");
        }
    }

    public override void GetDamage(int damage)
    {
        base.GetDamage(damage);

        if (nowHP < maxHP * 0.5f)
        {
            isBroken = Option.IsBroken;
            SetSprite();
        }
    }

    public override void OnZeroHP()
    {
        StopCoroutine(fireCycle);

        OppositionEntityManager.Instance.DespawnEntity(this);
    }

    void SetSprite()
    {
        Option spriteIndex = 0;

        spriteIndex |= isFront;
        spriteIndex |= isBroken;
        spriteIndex |= isFire;

        spriteRenderer.sprite = spriteList[(int)spriteIndex];
    }
}

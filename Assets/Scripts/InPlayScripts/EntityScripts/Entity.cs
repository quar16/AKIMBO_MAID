using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int maxHP;
    public int nowHP;

    public bool Damageable { get { return maxHP != 0; } }
    public int prefabId;

    public bool isFlash = true;
    FlashEffect flashEffect = new();
    public int detectionDistance;

    protected void Awake()
    {
        if (isFlash)
            flashEffect.Init(this, GetComponentInChildren<SpriteRenderer>());

        if (detectionDistance != 0)
            StartCoroutine(PlayerDetection());
    }

    IEnumerator PlayerDetection()
    {
        yield return PlayTime.ScaledNull;

        Transform myT = transform;
        Transform playerT = PlayerManager.Instance.player.transform;

        yield return new WaitUntil(() =>
        Mathf.Abs(myT.position.x - playerT.position.x) < detectionDistance);

        PlayerDetect();
    }

    public virtual void PlayerDetect()
    {

    }

    public virtual void GetDamage(int damage)
    {
        ChangeHP(-damage);
    }

    public void ChangeHP(int delta)
    {
        if (Damageable == false) return;

        if (isFlash)
            flashEffect.Flash();

        nowHP = Mathf.Clamp(nowHP + delta, 0, maxHP);

        if (nowHP <= 0)
        {
            OnZeroHP();
        }
    }

    public virtual void OnZeroHP() { }

    public virtual void Init(List<float> customData) { }

    public Vector3 centerPivot = new Vector3(0.5f, 0.5f, 0);
    public Vector3 CenterPoint { get { return transform.position + centerPivot; } }
}

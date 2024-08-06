using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum CoolTimeMode { RUSH, COOLDOWN }

public class Enemy_Boss1 : Enemy
{

    float attackCoolTime = 0;
    public float AttackCoolTime { get { return attackCoolTime; } set { attackCoolTime = value; } }

    Animator animator;

    private IState currentState;
    public bool isInCombat = false;

    public float minX, maxX;

    public WarnProjectile dashProjectile;
    public WarnProjectile aroundProjectile;
    public WarnProjectile jumpProjectile;
    public WarnProjectile trackingProjectile;
    public WarnProjectile teleportProjectile;

    public void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5))
            BossInit();
    }

    public void SetClamp(float _minX, float _maxX)
    {
        minX = _minX;
        maxX = _maxX;

    }

    public void BossInit()
    {
        dashProjectile.transform.parent = null;
        aroundProjectile.transform.parent = null;
        jumpProjectile.transform.parent = null;
        trackingProjectile.transform.parent = null;
        teleportProjectile.transform.parent = null;

        StartCoroutine(StateRunning());
    }

    IEnumerator StateRunning()
    {
        ChangeState<State_Stand>();

        while (nowHP > 0)
        {
            currentState.Execute();
            currentState.ChangeState();

            AttackCoolTime -= Time.deltaTime;
            yield return PlayTime.ScaledNull;
        }

        ChangeState<State_Dead>();
    }

    public override void GetDamage(int damage)
    {
        if (!isInCombat)
            ChangeState<State_Guard>();
        else
            base.GetDamage(damage);
    }

    public void ChangeState<T>() where T : IState
    {
        if (currentState != null)
            currentState.Exit();

        currentState = (T)System.Activator.CreateInstance(typeof(T));
        currentState.Boss = this;
        currentState.Enter();
    }

    public void SetAnimator(string clipName)
    {
        animator.Play(clipName);
    }

    CoolTimeMode coolTimeMode = CoolTimeMode.COOLDOWN;
    int coolTimeModeCount = 0;

    public void SetAttackCoolTime(float time = 0)
    {
        if (time != 0)
        {
            AttackCoolTime = time;
        }
        else
        {
            if (coolTimeMode == CoolTimeMode.RUSH)
                AttackCoolTime = Random.Range(0, 0.3f);
            else
                AttackCoolTime = Random.Range(1, 2f);

            coolTimeModeCount--;

            if (coolTimeModeCount <= 0)
            {
                coolTimeModeCount = Random.Range(2, 6);
                coolTimeMode = 1 - coolTimeMode;
            }
        }
    }

}

public class IState
{
    public Enemy_Boss1 Boss { set { boss = value; } }
    protected Enemy_Boss1 boss;

    public virtual void Enter() { }
    public virtual void Execute() { }
    public virtual void ChangeState() { }
    public virtual void Exit() { }

    protected bool IsPlayerDistanceIn(float min, float max)
    {
        float distance = Vector3.Distance(boss.transform.position, PlayerManager.Instance.player.transform.position);

        return (min <= distance) && (distance <= max);
    }
    protected bool IsPlayerDistanceXIn(float min, float max)
    {
        float distance = Mathf.Abs(boss.transform.position.x - PlayerManager.Instance.player.transform.position.x);

        return (min <= distance) && (distance <= max);
    }

    protected bool IsPlayerGround
    {
        get
        {
            return PlayerManager.Instance.playerState != PlayerState.JUMP &&
                   PlayerManager.Instance.playerState != PlayerState.JUMP2;
        }
    }

    protected Vector3 PlayerDirection
    {
        get
        {
            if (PlayerManager.Instance.player.transform.position.x < boss.transform.position.x)
            {
                boss.transform.localScale = new Vector3(1, 1, 1);
                return Vector3.left;
            }
            else
            {
                boss.transform.localScale = new Vector3(-1, 1, 1);

                return Vector3.right;
            }
        }
    }

    protected bool AttackCheck()
    {
        if (boss.AttackCoolTime <= 0)
        {
            boss.isInCombat = true;
            if (Random.Range(0, 2) == 0)
            {
                if (IsPlayerDistanceIn(0, 2))
                    boss.ChangeState<State_Around_Atk>();
                else
                    boss.ChangeState<State_Tracking_Atk>();
            }
            else
            {
                if (IsPlayerGround)
                {

                    if (IsPlayerDistanceXIn(3, 10))
                    {
                        if (boss.nowHP < boss.maxHP * 0.5f && Random.Range(0, 2) == 0)
                            boss.ChangeState<State_Fury_Dash_Atk>();
                        else
                            boss.ChangeState<State_Dash_Atk>();
                    }
                    else
                        boss.ChangeState<State_Teleport_Atk>();
                }
                else if (IsPlayerDistanceXIn(0, 4))
                    boss.ChangeState<State_Jump_Atk>();
                else
                    boss.ChangeState<State_Tracking_Atk>();
            }
            return true;
        }
        return false;
    }
}

public class State_Stand : IState
{
    public override void Enter()
    {
        boss.SetAnimator("Boss_Idle");
    }
    public override void ChangeState()
    {
        if (!AttackCheck() && IsPlayerDistanceXIn(3.5f, 100))
            boss.ChangeState<State_Walk>();
    }
}

public class State_Walk : IState
{
    float speed = 0.016f;
    public override void Enter()
    {
        boss.SetAnimator("Boss_Walk");
    }
    public override void Execute()
    {
        boss.transform.position += PlayTime.Scale * speed * PlayerDirection;
    }
    public override void ChangeState()
    {
        if (!AttackCheck() && IsPlayerDistanceXIn(0, 2.5f))
            boss.ChangeState<State_Stand>();
    }
}

public class State_Guard : IState
{
    float time = 0.1f;
    public override void Enter()
    {
        boss.SetAnimator("Guard");
    }
    public override void Execute()
    {
        time -= Time.deltaTime;
    }
    public override void ChangeState()
    {
        if (time < 0)
            boss.ChangeState<State_Stand>();
    }
}

public class State_Dead : IState
{
    int bossDeadNarrativeIndex = 2;

    public override void Enter()
    {
        boss.SetAnimator("Dead");
        NarrativeManager.Instance.NarrativeCall(StageManager.Instance.NarrativeDataPath(bossDeadNarrativeIndex));

    }
}

public class State_Dash_Atk : IState
{
    IEnumerator executing;
    public override void Enter()
    {
        boss.SetAnimator("Atk_Ready");
        executing = Executing();
        boss.StartCoroutine(executing);
    }

    IEnumerator Executing()
    {
        Vector3 targetPos = boss.transform.position + PlayerDirection * 10;
        targetPos.x = Mathf.Clamp(targetPos.x, boss.minX, boss.maxX);
        Vector3 projectilePos = (boss.transform.position + targetPos) * 0.5f;

        boss.dashProjectile.Init(projectilePos, PlayerDirection == Vector3.left);
        boss.dashProjectile.Warn(1);

        yield return PlayTime.ScaledWaitForSeconds(1f);
        boss.SetAnimator("Boss_Fade_Out");
        yield return PlayTime.ScaledWaitForSeconds(0.1f);
        boss.transform.position = targetPos;
        boss.dashProjectile.Shoot(0.1f);

        boss.SetAnimator("Boss_Fade_In");
        yield return PlayTime.ScaledWaitForSeconds(0.5f);

        boss.isInCombat = false;
    }

    public override void ChangeState()
    {
        if (boss.isInCombat == false)
            boss.ChangeState<State_Stand>();
    }

    public override void Exit()
    {
        boss.StopCoroutine(executing);
        boss.SetAttackCoolTime();
    }
}

public class State_Fury_Dash_Atk : IState
{
    IEnumerator executing;
    public override void Enter()
    {
        executing = Executing();
        boss.StartCoroutine(executing);
    }

    IEnumerator Executing()
    {
        boss.SetAnimator("Atk_Ready");
        yield return PlayTime.ScaledWaitForSeconds(1f);

        Vector3 targetPos;
        Vector3 projectilePos;

        for (int i = 0; i < 4; i++)
        {
            targetPos = boss.transform.position + PlayerDirection * 4;
            targetPos.x = Mathf.Clamp(targetPos.x, boss.minX, boss.maxX);
            projectilePos = (boss.transform.position + targetPos) * 0.5f;

            boss.dashProjectile.Init(projectilePos, PlayerDirection == Vector3.left);
            boss.dashProjectile.Warn(0.2f);

            boss.SetAnimator("Atk_Ready");
            yield return PlayTime.ScaledWaitForSeconds(0.2f);

            boss.SetAnimator("Boss_Fade_Out");
            yield return PlayTime.ScaledWaitForSeconds(0.1f);
            boss.transform.position = targetPos;
            boss.dashProjectile.Shoot(0.1f);

            boss.SetAnimator("Boss_Fade_In");
            yield return PlayTime.ScaledWaitForSeconds(0.1f);
        }

        yield return PlayTime.ScaledWaitForSeconds(1f);

        boss.isInCombat = false;
    }

    public override void ChangeState()
    {
        if (boss.isInCombat == false)
            boss.ChangeState<State_Stand>();
    }

    public override void Exit()
    {
        boss.StopCoroutine(executing);
        boss.SetAttackCoolTime();
    }
}

public class State_Teleport_Atk : IState
{
    IEnumerator executing;

    public override void Enter()
    {
        executing = Executing();
        boss.StartCoroutine(executing);
    }
    IEnumerator Executing()
    {
        float targetPosX = PlayerManager.Instance.player.transform.position.x;
        Vector3 targetPos = boss.transform.position;
        targetPos.x = targetPosX;

        boss.teleportProjectile.Init(targetPos, PlayerDirection == Vector3.left);

        boss.SetAnimator("Boss_Fade_Out");
        yield return PlayTime.ScaledWaitForSeconds(0.1f);

        boss.transform.position = targetPos;
        boss.SetAnimator("Boss_Fade_In");
        boss.teleportProjectile.Warn(0.7f);
        yield return PlayTime.ScaledWaitForSeconds(0.1f);
        boss.SetAnimator("Atk_Ready");
        yield return PlayTime.ScaledWaitForSeconds(0.6f);

        boss.SetAnimator("Atk");
        boss.teleportProjectile.Shoot(0.5f);

        yield return PlayTime.ScaledWaitForSeconds(1f);

        boss.isInCombat = false;
    }
    public override void ChangeState()
    {
        if (boss.isInCombat == false)
            boss.ChangeState<State_Stand>();
    }

    public override void Exit()
    {
        boss.StopCoroutine(executing);
        boss.SetAttackCoolTime();
    }
}

public class State_Jump_Atk : IState
{
    IEnumerator executing;

    public override void Enter()
    {
        executing = Executing();
        boss.StartCoroutine(executing);
    }
    IEnumerator Executing()
    {
        Vector3 targetPos = PlayerManager.Instance.player.transform.position;
        Vector3 startPos = boss.transform.position;
        float groundY = startPos.y;

        boss.jumpProjectile.Init(targetPos, PlayerDirection == Vector3.left);

        boss.SetAnimator("Atk_Ready");
        boss.jumpProjectile.Warn(1);

        boss.SetAnimator("Jump");

        float time = Time.time;
        float duration = 0.2f;

        while (time + duration > Time.time)
        {
            float t = (Time.time - time) / duration;
            boss.transform.position = Vector3.Lerp(startPos, targetPos, Mathf.Sin(t * 0.5f * Mathf.PI));

            yield return PlayTime.ScaledNull;
        }
        boss.SetAnimator("Jump_Atk");
        boss.jumpProjectile.Shoot(0.2f);
        yield return PlayTime.ScaledWaitForSeconds(0.2f);

        boss.SetAnimator("Fall");
        float gravityForce = 0.0015f;
        float verticalSpeed = 0;

        targetPos = boss.transform.position;
        targetPos.y = groundY;
        while (boss.transform.position.y > groundY)
        {
            yield return PlayTime.ScaledNull;

            verticalSpeed -= gravityForce * PlayTime.Scale;
            boss.transform.position += Vector3.up * verticalSpeed;
        }

        boss.transform.position = targetPos;

        boss.isInCombat = false;
    }
    public override void ChangeState()
    {
        if (boss.isInCombat == false)
            boss.ChangeState<State_Stand>();
    }

    public override void Exit()
    {
        boss.StopCoroutine(executing);
        boss.SetAttackCoolTime();
    }
}

public class State_Around_Atk : IState
{
    IEnumerator executing;

    public override void Enter()
    {
        executing = Executing();
        boss.StartCoroutine(executing);
    }
    IEnumerator Executing()
    {
        Vector3 targetPos = boss.transform.position;

        boss.aroundProjectile.Init(targetPos, PlayerDirection == Vector3.left);
        boss.aroundProjectile.Warn(0.2f);
        boss.SetAnimator("Atk_Ready");
        yield return PlayTime.ScaledWaitForSeconds(0.2f);

        boss.SetAnimator("Atk");
        boss.aroundProjectile.Shoot(0.2f);
        yield return PlayTime.ScaledWaitForSeconds(0.5f);

        boss.isInCombat = false;
    }
    public override void ChangeState()
    {
        if (boss.isInCombat == false)
            boss.ChangeState<State_Stand>();
    }

    public override void Exit()
    {
        boss.StopCoroutine(executing);
        boss.SetAttackCoolTime();
    }
}

public class State_Tracking_Atk : IState
{
    IEnumerator executing;

    public override void Enter()
    {
        executing = Executing();
        boss.StartCoroutine(executing);
    }
    IEnumerator Executing()
    {
        Vector3 targetPos = PlayerManager.Instance.player.transform.position;

        boss.trackingProjectile.Init(targetPos, PlayerDirection == Vector3.left);
        boss.trackingProjectile.Warn(0.6f);
        boss.SetAnimator("Atk_Ready");
        yield return PlayTime.ScaledWaitForSeconds(0.6f);

        boss.SetAnimator("Atk");
        boss.trackingProjectile.Shoot(0.2f);
        yield return PlayTime.ScaledWaitForSeconds(0.5f);

        boss.isInCombat = false;
    }
    public override void ChangeState()
    {
        if (boss.isInCombat == false)
            boss.ChangeState<State_Stand>();
    }

    public override void Exit()
    {
        boss.StopCoroutine(executing);
        boss.SetAttackCoolTime();
    }
}
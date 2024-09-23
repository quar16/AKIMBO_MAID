using System.Collections;
using UnityEngine;


public enum Direction { NONE, LEFT, RIGHT }

public class PlayerMoveController : MonoBehaviour
{
    Transform playerT;

    public Animator animator;
    public PlayerGun playerGun;

    public float runSpeed = 0.03f;

    public float slideSpeed = 0.05f;

    public float jumpPower = 0.08f;
    public float horizontalSpeedLerpInAir = 0.1f;
    public float gravityForce = 0.0015f;

    public LayerMask groundLayer = 1;      // 바닥인 레이어를 설정합니다.
    public float raycastDistance = 0.01f;   // Raycast 거리를 설정합니다.

    public float fireRate = 0.1f;
    float nextFireTime = 0;

    bool isInputJump;
    bool isInputFire;
    bool isInputSlide;
    Direction isInputMoving;
    Direction lastDirectionInput;

    bool isActivate;
    bool isGrounded;

    public Direction playerDirection;

    public PlayerState PlayerState
    {
        get { return PlayerManager.Instance.playerState; }
        set { PlayerManager.Instance.playerState = value; }
    }

    float horizontalSpeed;
    float verticalSpeed;

    bool isClamp = false;
    float minX, maxX;

    public void Init()
    {
        isActivate = true;
    }

    public void Dead()
    {
        isActivate = false;
        isInputMoving = Direction.NONE;
        animator.Play("Player_Dead");
    }

    public void SetClamp(float _minX, float _maxX)
    {
        isClamp = true;
        minX = _minX;
        maxX = _maxX;
    }
    public void ReleaseClamp()
    {
        isClamp = false;
    }

    public void StartNarrative()
    {
        isInputMoving = Direction.NONE;
        playerDirection = Direction.NONE;

        isInputJump = false;
        isInputSlide = false;
        isInputFire = false;
    }

    public void InputDirection(Direction direction)
    {
        isInputMoving = direction;
    }
    public void InputJump()
    {
        isInputJump = true;
    }

    private void Start()
    {
        playerT = transform;
    }

    private void Update()
    {
        if (Time.timeScale == 0) return;

        FallCalc();

        PlayerMove();

        if (GameManager.Instance.gameMode == GameMode.NARRATIVE)
            return;

        if (!isActivate) return;

        InputCheck();
        Fire();
    }

    private void InputCheck()
    {
        switch (GameManager.Instance.gameMode)
        {
            case GameMode.RUN:
                {
                    isInputMoving = Direction.RIGHT;
                    playerDirection = Direction.RIGHT;

                    break;
                }
            case GameMode.NARRATIVE:
                {
                    break;
                }
            case GameMode.BOSS:
                {
                    if (Input.GetKeyDown(KeyCode.RightArrow))
                        lastDirectionInput = Direction.RIGHT;
                    else if (Input.GetKeyDown(KeyCode.LeftArrow))
                        lastDirectionInput = Direction.LEFT;

                    switch (Input.GetKey(KeyCode.RightArrow), Input.GetKey(KeyCode.LeftArrow))
                    {
                        case (true, true):
                            isInputMoving = lastDirectionInput;
                            break;
                        case (true, false):
                            isInputMoving = Direction.RIGHT;
                            break;
                        case (false, true):
                            isInputMoving = Direction.LEFT;
                            break;
                        case (false, false):
                            isInputMoving = Direction.NONE;
                            break;
                    }

                    break;
                }
        }

        isInputJump = Input.GetKeyDown(KeyCode.Space);
        isInputSlide = Input.GetKeyDown(KeyCode.LeftShift);
        isInputFire = Input.GetKey(KeyCode.Z);
    }

    private void PlayerMove()
    {
        HorizontalMove();

        switch (PlayerState)
        {
            case PlayerState.IDLE:
            case PlayerState.RUN:
                {
                    if (isInputJump)
                        StartCoroutine(Jump());
                    else if (isInputSlide)
                        StartCoroutine(Slide());
                    break;
                }
            case PlayerState.JUMP:
                {
                    if (isInputJump)
                        StartCoroutine(Jump2());
                    break;
                }
        }

        if (horizontalSpeed < 0)
        {
            playerDirection = Direction.LEFT;
            playerT.localScale = new Vector3(-1, 1, 1);
        }
        else if (horizontalSpeed > 0)
        {
            playerDirection = Direction.RIGHT;
            playerT.localScale = new Vector3(1, 1, 1);
        }
        Vector3 targetPos = playerT.position + new Vector3(horizontalSpeed, verticalSpeed, 0) * PlayTime.Scale;

        if (isClamp)
            targetPos.x = Mathf.Clamp(targetPos.x, minX, maxX);

        playerT.position = targetPos;

    }

    private void HorizontalMove()
    {
        switch (PlayerState)
        {
            case PlayerState.IDLE:
            case PlayerState.RUN:
                {
                    if (isInputMoving == Direction.NONE)
                    {
                        animator.SetFloat("IsMove", -1);
                        PlayerState = PlayerState.IDLE;
                        horizontalSpeed = 0;
                    }
                    else
                    {
                        animator.SetFloat("IsMove", 1);
                        PlayerState = PlayerState.RUN;

                        if (isInputMoving == Direction.RIGHT)
                            horizontalSpeed = runSpeed;
                        if (isInputMoving == Direction.LEFT)
                            horizontalSpeed = -runSpeed;
                    }
                    break;
                }
            case PlayerState.JUMP:
            case PlayerState.JUMP2:
                {
                    float targeSpeed = 0;

                    if (isInputMoving == Direction.RIGHT)
                        targeSpeed = runSpeed;
                    if (isInputMoving == Direction.LEFT)
                        targeSpeed = -runSpeed;

                    horizontalSpeed = Mathf.Lerp(horizontalSpeed, targeSpeed, horizontalSpeedLerpInAir);

                    break;
                }
        }
    }

    private IEnumerator Jump()
    {
        animator.Play("Jump_Start");
        animator.SetFloat("JumpLevel", -1);

        PlayerState = PlayerState.JUMP;

        verticalSpeed = jumpPower;

        yield return new WaitUntil(() => isGrounded == false);
        yield return new WaitWhile(() => isGrounded == false);

        animator.SetTrigger("JumpEnd");

        if (PlayerState == PlayerState.JUMP)
            PlayerState = PlayerState.IDLE;
    }

    private IEnumerator Jump2()
    {
        animator.Play("Jump_Start");
        animator.SetFloat("JumpLevel", 1);

        PlayerState = PlayerState.JUMP2;

        verticalSpeed = jumpPower;

        yield return new WaitUntil(() => isGrounded == false);
        yield return new WaitWhile(() => isGrounded == false);

        animator.SetTrigger("JumpEnd");

        PlayerState = PlayerState.IDLE;
    }

    public float slideRange = 8;
    private IEnumerator Slide()
    {
        animator.SetTrigger("SlideStart");

        PlayerState = PlayerState.SLIDE;

        float startX = playerT.position.x;
        float endX = playerT.position.x + slideRange * (playerDirection == Direction.RIGHT ? 1 : -1);
        if (isClamp)
            endX = Mathf.Clamp(endX, minX, maxX);
        float t = 0;

        while (t < 0.99f)
        {
            t = (playerT.position.x - startX) / (endX - startX);
            float speed = Mathf.Lerp(slideSpeed, runSpeed, t);

            speed = Mathf.Clamp(speed, 0, Mathf.Abs(endX - playerT.position.x));
            if (playerDirection == Direction.RIGHT)
                horizontalSpeed = speed;
            if (playerDirection == Direction.LEFT)
                horizontalSpeed = -speed;

            yield return PlayTime.ScaledNull;
        }

        PlayerState = PlayerState.IDLE;

        animator.SetTrigger("SlideEnd");
    }


    private void FallCalc()
    {
        // 아래쪽으로 Raycast를 쏘아 바닥을 감지합니다.
        RaycastHit2D hitBottom = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);
        RaycastHit2D hitCeil = Physics2D.Raycast(transform.position + Vector3.up * 0.866f, Vector2.down, raycastDistance, groundLayer);

        // Raycast가 바닥과 충돌했는지를 검사합니다.
        isGrounded = hitBottom.collider != null;

        bool isCeiled = hitCeil.collider != null;

        if (isCeiled && verticalSpeed > 0)
        {
            verticalSpeed = 0;
        }

        if (isGrounded == false)//collision check
        {
            verticalSpeed -= gravityForce * PlayTime.Scale;
        }
        else
        {
            Vector2 playerPos = playerT.transform.position;
            playerPos.y = hitBottom.collider.bounds.size.y * 0.5f + hitBottom.collider.offset.y + hitBottom.transform.position.y;
            playerT.transform.position = playerPos;
            verticalSpeed = 0;
        }
    }

    private void Fire()
    {
        if (isInputFire && (Time.time >= nextFireTime))
        {
            nextFireTime = Time.time + fireRate;
            playerGun.Shoot();
        }
    }
}

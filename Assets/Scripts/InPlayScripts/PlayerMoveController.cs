using System.Collections;
using System.Data;
using UnityEngine;


public enum Direction { NONE, LEFT, RIGHT }

public class PlayerMoveController : MonoBehaviour
{
    Transform playerT;

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

    bool isGrounded;

    public Direction playerDirection;

    public PlayerState PlayerState
    {
        get { return PlayerManager.Instance.playerState; }
        set { PlayerManager.Instance.playerState = value; }
    }

    public float horizontalSpeed;
    public float VerticalSpeed;

    private void Start()
    {
        playerT = transform;
    }

    void Update()
    {
        InputCheck();

        PlayerMove();

        Fire();

        FallCalc();
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

        playerT.position += new Vector3(horizontalSpeed, VerticalSpeed, 0);

    }

    public void HorizontalMove()
    {
        switch (PlayerState)
        {
            case PlayerState.IDLE:
            case PlayerState.RUN:
                {
                    if (isInputMoving == Direction.NONE)
                    {
                        PlayerState = PlayerState.IDLE;
                        horizontalSpeed = 0;
                    }
                    else
                    {
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

    public IEnumerator Jump()
    {
        PlayerState = PlayerState.JUMP;

        VerticalSpeed = jumpPower;

        yield return new WaitUntil(() => isGrounded == false);
        yield return new WaitWhile(() => isGrounded == false);

        if (PlayerState == PlayerState.JUMP)
            PlayerState = PlayerState.IDLE;
    }

    public IEnumerator Jump2()
    {
        PlayerState = PlayerState.JUMP2;

        VerticalSpeed = jumpPower;

        yield return new WaitUntil(() => isGrounded == false);
        yield return new WaitWhile(() => isGrounded == false);

        PlayerState = PlayerState.IDLE;
    }

    public int slideFrame = 10;
    public IEnumerator Slide()
    {
        PlayerState = PlayerState.SLIDE;


        for (int i = 0; i <= slideFrame; i++)
        {
            float speed = Mathf.Lerp(slideSpeed, runSpeed, i / (float)slideFrame);

            if (playerDirection == Direction.RIGHT)
                horizontalSpeed = speed;
            if (playerDirection == Direction.LEFT)
                horizontalSpeed = -speed;

            yield return null;
        }

        PlayerState = PlayerState.IDLE;
    }


    public void FallCalc()
    {
        // 아래쪽으로 Raycast를 쏘아 바닥을 감지합니다.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, raycastDistance, groundLayer);

        // Raycast가 바닥과 충돌했는지를 검사합니다.
        isGrounded = hit.collider != null;

        if (isGrounded == false)//collision check
        {
            VerticalSpeed -= gravityForce;
        }
        else
        {
            Vector2 playerPos = playerT.transform.position;
            playerPos.y = hit.collider.bounds.size.y * 0.5f + hit.collider.offset.y + hit.transform.position.y;
            playerT.transform.position = playerPos;
            VerticalSpeed = 0;
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

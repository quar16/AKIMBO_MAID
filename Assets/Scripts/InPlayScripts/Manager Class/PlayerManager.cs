using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public PlayerMoveController playerMoveController;
    public PlayerGun playerGun;
    public GameObject player;
    public Animator animator;
    public PlayerState playerState;


    public void Init()
    {
        player.transform.position = Vector3.up;
        animator.Play("Player_Idle");
        playerState = PlayerState.IDLE;
        PlayerHealthManager.Instance.ChangeHealth(10);
        playerMoveController.Init();
    }

    public void CleanUp()
    {
        playerMoveController.CleanUp();
        playerGun.Reload();
    }

}

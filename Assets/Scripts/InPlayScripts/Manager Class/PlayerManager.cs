using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoSingleton<PlayerManager>
{
    public PlayerMoveController playerMoveController;
    public Animator Animator { get { return playerMoveController.animator; } }
    public PlayerGun playerGun;
    public GameObject player;
    public PlayerState playerState;


    public void Init()
    {
        player.transform.position = Vector3.up;
        playerMoveController.animator.Play("Player_Idle");
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

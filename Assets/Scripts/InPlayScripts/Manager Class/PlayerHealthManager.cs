using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoSingleton<PlayerHealthManager>
{
    [SerializeField]
    int health = 10;
    public int Health { get { return health; } }
    bool isDead = false;

    FlashEffect flashEffect = new();

    private void Start()
    {
        flashEffect.Init(this, GetComponentInChildren<SpriteRenderer>());
    }

    public void ChangeHealth(int delta)
    {
        if (health <= 0) return;

        health = Mathf.Clamp(health + delta, 0, 10);
        flashEffect.Flash();

        if (health <= 0)
            OnZeroHealth();
    }

    void OnZeroHealth()
    {
        if (isDead) return;
        isDead = true;
        StartCoroutine(DeadProcessing());
    }

    IEnumerator DeadProcessing()
    {
        PlayerManager.Instance.playerMoveController.Dead();

        yield return CameraController.Instance.DeadProcessingCamera();
        yield return PlayTime.ScaledWaitForSeconds(0.5f);
        SceneTransitionManager.Instance.RestartStage();
    }

}

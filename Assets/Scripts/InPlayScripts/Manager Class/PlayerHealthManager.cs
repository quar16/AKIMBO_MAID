using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoSingleton<PlayerHealthManager>
{
    [SerializeField]
    int health = 10;
    public int Health { get { return health; } }

    public void ChangeHealth(int delta)
    {
        health = Mathf.Clamp(health + delta, 0, 10);
    }

}

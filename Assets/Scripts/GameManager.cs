using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { RUN, BOSS, NARRATIVE }

public class GameManager : MonoSingleton<GameManager>
{
    public GameMode gameMode;

    private void Start()
    {
        Application.targetFrameRate = 60;

    }

}

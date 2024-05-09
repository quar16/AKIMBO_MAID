using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMode { RUN, BOSS }

public class GameManager : MonoSingleton<GameManager>
{
    public GameMode gameMode;


}

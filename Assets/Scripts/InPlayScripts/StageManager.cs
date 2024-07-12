using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoSingleton<StageManager>
{
    public StageDataScriptableObject tempStageData;

    public Image upperLetterBox;
    public Image lowerLetterBox;

    private void Start()
    {
        SceneInit(tempStageData, 0);
    }

    public void SceneInit(StageDataScriptableObject stageData, int startPoint = 0)
    {
        tempStageData = stageData;
        StartCoroutine(Initiate());
        OppositionEntityManager.Instance.StartEnemySpawnRoutine(stageData.entities);
    }

    public IEnumerator Initiate()
    {
        yield return NarrativePlay();

        GameManager.Instance.gameMode = GameMode.RUN;
    }


    public void CallBossRoom()
    {
        //StopCoroutine(moveBackGround);
        GameManager.Instance.gameMode = GameMode.BOSS;
        CameraController.Instance.UpdateCameraTarget("Player", false);
    }

    public IEnumerator NarrativePlay()
    {
        yield return LetterBoxIn();

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));

        yield return LetterBoxOut();
    }

    IEnumerator LetterBoxIn()
    {
        for (int i = 0; i <= 30; i++)
        {
            upperLetterBox.rectTransform.anchoredPosition = Vector2.up * Mathf.Lerp(0, 200, 1 - i / 30f);
            lowerLetterBox.rectTransform.anchoredPosition = Vector2.down * Mathf.Lerp(0, 200, 1 - i / 30f);
            yield return null;
        }
    }
    IEnumerator LetterBoxOut()
    {
        for (int i = 0; i <= 30; i++)
        {
            upperLetterBox.rectTransform.anchoredPosition = Vector2.up * Mathf.Lerp(0, 200, i / 30f);
            lowerLetterBox.rectTransform.anchoredPosition = Vector2.down * Mathf.Lerp(0, 200, i / 30f);
            yield return null;
        }
    }

}


/*
 외부에 저장된 스테이지 데이터를 받아와서 구현
 배경과 바닥, 원경은 따로 움직인다
 적이나 보스 스테이지는 플레이어의 위치를 기반으로 소환
 처음과 보스전 시작, 보스전에는 연출 효과를 추가한다 / 지금은 enter로 넘기기
 보스 소환 트리거가 작동하면 원경을 제외한 배경의 이동은 멈춘다
 배경은 삼교대로 돌린다

 스테이지 데이터
 연출 구간, 내용
 스테이지 배치
 보스전 위치, 정보
 
 
 
 */
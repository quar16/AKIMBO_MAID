using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : MonoBehaviour
{
    public Image upperLetterBox;
    public Image lowerLetterBox;

    public NarrativeData narrativeData;

    public void NarrativeCall(string narrativeName)
    {
        GameManager.Instance.gameMode = GameMode.NARRATIVE;
        //file search
        StartCoroutine(NarrativeFlow());

    }

    public IEnumerator NarrativeFlow()
    {
        yield return LetterBoxIn();

        foreach (var narrative in narrativeData.narratives)
        {
            NarrativeDistribute(narrative);

            if (narrative.isSequential == false)
            {
                yield return new WaitUntil(() => NarrativeProcess.IsActiveProcessorNotExist);

                if (narrative.isAuto == false)
                {
                    yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
                    yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.Return));
                }
            }
        }

        yield return LetterBoxOut();
    }

    public void NarrativeDistribute(Narrative narrative)
    {
        switch (narrative)
        {
            case CameraNarrative _:
                CameraNarrativeProcess cn_Process = new CameraNarrativeProcess(narrative);
                StartCoroutine(cn_Process.Processing());
                break;
            case CameraShakeNarrative _:
                break;
            case DialogueNarrative _:
                break;
            case FadeInOutNarrative _:
                break;
            case CutSceneNarrative _:
                break;
            case CharacterNarrative _:
                break;
            case TimeDelayNarrative _:
                break;
        }
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

public abstract class NarrativeProcess
{
    //내러티브 프로세스를 생성할 때 내러티브 데이터를 넘겨준다. 
    public NarrativeProcess(Narrative narrative) { }

    //글로벌 프로세스리스트, 등록된 프로세스가 없는지 확인할 수 있는 글로벌 함수를 제공한다.
    private static List<NarrativeProcess> activeNarrativeProcessors = new();
    public static bool IsActiveProcessorNotExist
    {
        get
        {
            return activeNarrativeProcessors.Count == 0;
        }
    }
    private void RegisterProcess()
    {
        activeNarrativeProcessors.Add(this);
    }
    private void UnregisterProcess()
    {
        activeNarrativeProcessors.Remove(this);
    }

    //내러티브 프로세스를 호출하면 자동으로 등록, 등록 해지가 처리된다.
    public IEnumerator Processing()
    {
        RegisterProcess();
        yield return ProcessNarrative();
        UnregisterProcess();
    }

    //프로세스를 상속받은 자식이 각각 다른 내용을 정의하는 부분.
    protected abstract IEnumerator ProcessNarrative();
}


public class CameraNarrativeProcess : NarrativeProcess
{
    CameraNarrative cameraNarrative;

    public CameraNarrativeProcess(Narrative narrative) : base(narrative)
    {
        cameraNarrative = (CameraNarrative)narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        yield break;
    }
}

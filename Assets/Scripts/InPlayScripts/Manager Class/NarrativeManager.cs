using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeManager : MonoSingleton<NarrativeManager>
{
    public TextAsset textAsset; // Unity 에디터에서 할당
    public Image upperLetterBox;
    public Image lowerLetterBox;

    public List<NarrativeData> narrativeDataList = new();
    public Dictionary<string, NarrativeData> narrativeDataDic = new();

    public NarrativeData narrativeData;

    bool isNarrative = false;
    public bool IsNarrative { get { return isNarrative; } }

    public string filePathT = "C:/Users/god_s/Documents/AKIMBO_MAID/Assets";

    public void NarrativeCall(TextAsset textAsset)
    {
        GameManager.Instance.gameMode = GameMode.NARRATIVE;

        PlayerManager.Instance.playerMoveController.StartNarrative();

        narrativeData = JsonConvert.DeserializeObject<NarrativeData>(textAsset.text, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
        StartCoroutine(NarrativeFlow());
    }

    public IEnumerator NarrativeFlow()
    {
        isNarrative = true;
        PlayerManager.Instance.Animator.SetBool("Narrative", true);
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

                DialogueManager.Instance.HideLastDialogueBox();
            }
        }

        yield return LetterBoxOut();
        isNarrative = false;
        PlayerManager.Instance.Animator.SetBool("Narrative", false);
    }

    public void NarrativeDistribute(Narrative narrative)
    {
        switch (narrative)
        {
            case CameraNarrative _:
                CameraNarrativeProcess cam_Process = new CameraNarrativeProcess(narrative);
                StartCoroutine(cam_Process.Processing());
                break;
            case CameraShakeNarrative _:
                CameraShakeNarrativeProcess camShk_Process = new CameraShakeNarrativeProcess(narrative);
                StartCoroutine(camShk_Process.Processing());
                break;
            case DialogueNarrative _:
                DialogueNarrativeProcess dil_Process = new DialogueNarrativeProcess(narrative);
                StartCoroutine(dil_Process.Processing());
                break;
            case FunctionNarrative _:
                FunctionNarrativeProcess func_Process = new FunctionNarrativeProcess(narrative);
                StartCoroutine(func_Process.Processing());
                break;
            case FadeInOutNarrative _:
                FadeInOutNarrativeProcess fio_Process = new FadeInOutNarrativeProcess(narrative);
                StartCoroutine(fio_Process.Processing());
                break;
            case CutSceneNarrative _:
                CutSceneNarrativeProcess cutsc_Process = new CutSceneNarrativeProcess(narrative);
                StartCoroutine(cutsc_Process.Processing());
                break;
            case CharacterNarrative _:
                CharacterNarrativeProcess chr_Process = new CharacterNarrativeProcess(narrative);
                StartCoroutine(chr_Process.Processing());
                break;
            case TimeDelayNarrative _:
                TimeDelayNarrativeProcess tmdly_Process = new TimeDelayNarrativeProcess(narrative);
                StartCoroutine(tmdly_Process.Processing());
                break;
        }
    }

    IEnumerator LetterBoxIn()
    {
        float time = Time.time;
        float duration = 0.5f;
        while (time + duration > Time.time)
        {
            float t = (Time.time - time) / duration;
            upperLetterBox.rectTransform.anchoredPosition = Vector2.up * Mathf.Lerp(0, 200, 1 - t);
            lowerLetterBox.rectTransform.anchoredPosition = Vector2.down * Mathf.Lerp(0, 200, 1 - t);
            yield return PlayTime.ScaledNull;
        }
    }
    IEnumerator LetterBoxOut()
    {
        float time = Time.time;
        float duration = 0.5f;
        while (time + duration > Time.time)
        {
            float t = (Time.time - time) / duration;
            upperLetterBox.rectTransform.anchoredPosition = Vector2.up * Mathf.Lerp(0, 200, t);
            lowerLetterBox.rectTransform.anchoredPosition = Vector2.down * Mathf.Lerp(0, 200, t);
            yield return PlayTime.ScaledNull;
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
    CameraNarrative narrative;

    public CameraNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (CameraNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        if (narrative.modifyCameraSize)
            CameraController.Instance.SetCameraSize(narrative.cameraSize);
        if (narrative.modifyOffset)
            CameraController.Instance.SetCameraOffset(narrative.offset.ToVector3());
        if (narrative.modifyTrackingPower)
            CameraController.Instance.SetCamTrackingPower(narrative.camTrackingPower);
        if (narrative.modifyCharacter)
        {
            if (narrative.targeted == ToggleTypes.On)
                CameraController.Instance.AddNamedCharacter(narrative.characterName, narrative.weight);
            else if (narrative.targeted == ToggleTypes.Off)
                CameraController.Instance.RemoveNamedCharacter(narrative.characterName);
            else if (narrative.targeted == ToggleTypes.None)
                CameraController.Instance.SetCameraTargetWeight(narrative.characterName, narrative.weight);
        }

        yield break;
    }
}

public class CameraShakeNarrativeProcess : NarrativeProcess
{
    CameraShakeNarrative narrative;

    public CameraShakeNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (CameraShakeNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        CameraController.Instance.CameraShake(
            narrative.shakePower.x,
            narrative.shakePower.y,
            narrative.duration,
            narrative.frameGap);

        yield return PlayTime.ScaledWaitForSeconds(narrative.duration);
    }
}

public class CharacterNarrativeProcess : NarrativeProcess
{
    CharacterNarrative narrative;

    public CharacterNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (CharacterNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        NamedCharacter character = NamedCharacter.GetNamedCharacter(narrative.characterName);

        if (narrative.animationState != null)
            character.animator.Play(narrative.animationState);

        if (narrative.narrativePoint != null)
        {
            Vector3 start = character.transform.position;
            Vector3 end = CharacterNarrativePoint.GetNarrativePoint(narrative.narrativePoint).transform.position;

            float maxDistance = Vector3.Distance(start, end);

            float moveTime = maxDistance / narrative.speedPerSec;

            float startTime = Time.time;

            while (Time.time < startTime + moveTime)
            {
                character.transform.position = Vector3.Lerp(start, end, (Time.time - startTime) / moveTime);
                yield return PlayTime.ScaledNull;
            }

            character.transform.position = end;
        }

        //이동 및 애니메이션 제어

        yield break;
    }
}

public class FadeInOutNarrativeProcess : NarrativeProcess
{
    FadeInOutNarrative narrative;

    public FadeInOutNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (FadeInOutNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        yield return SceneTransitionManager.Instance.CallFadeEffect(narrative.fadeType, narrative.inOutType);
    }
}

public class CutSceneNarrativeProcess : NarrativeProcess
{
    CutSceneNarrative narrative;

    public CutSceneNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (CutSceneNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        switch (narrative.toggle)
        {
            case ToggleTypes.None:
                yield return CutSceneGroup.Instance.CutSceneMove(narrative.imageName, narrative.position.ToVector3(), narrative.moveTime);
                break;
            case ToggleTypes.Off:
                yield return CutSceneGroup.Instance.CutSceneHide(narrative.imageName, narrative.moveTime);
                break;
            case ToggleTypes.On:
                yield return CutSceneGroup.Instance.CutSceneShow(narrative.imageName, narrative.position.ToVector3(), narrative.moveTime);
                break;
        }

        yield break;
    }
}

public class DialogueNarrativeProcess : NarrativeProcess
{
    DialogueNarrative narrative;

    public DialogueNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (DialogueNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        yield return DialogueManager.Instance.ShowDiaglogue(narrative.characterName, narrative.dialogueText);
    }
}

public class FunctionNarrativeProcess : NarrativeProcess
{
    FunctionNarrative narrative;

    public FunctionNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (FunctionNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        yield return NarrativeFunction.RunNarrativeFunction(narrative.functionID, narrative.name);
    }
}

public class TimeDelayNarrativeProcess : NarrativeProcess
{
    TimeDelayNarrative narrative;

    public TimeDelayNarrativeProcess(Narrative _narrative) : base(_narrative)
    {
        narrative = (TimeDelayNarrative)_narrative;
    }

    protected override IEnumerator ProcessNarrative()
    {
        yield return PlayTime.ScaledWaitForSeconds(narrative.delayTime);
    }
}

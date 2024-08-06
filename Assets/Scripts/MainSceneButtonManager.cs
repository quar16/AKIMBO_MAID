using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

enum LightState { BLINK, OFF }

public class MainSceneButtonManager : MonoBehaviour
{
    public GameObject shadow;
    public GameObject elevatorShadow;
    public Animator elevator_Anim;
    public Animator elevator_Shadow_Anim;
    public Animator elevator_Button_Anim;

    LightState lightState;

    public float shadowTimeS = 0.07f;
    public float shadowTimeE = 0.07f;
    public float lightTimeS = 0.14f;
    public float lightTimeE = 0.14f;
    public float gapS = 0.07f;
    public float gapE = 0.07f;
    public Color lightOff;


    private void Start()
    {
        StartCoroutine(LightFlickering());
    }

    IEnumerator LightFlickering()
    {
        while (lightState == LightState.BLINK)
        {

            int count = Random.Range(1, 4);

            for (int i = 0; i < count; i++)
            {
                ToggleShadow();
                yield return new WaitForSeconds(Random.Range(shadowTimeS, shadowTimeE));
                ToggleShadow();
                yield return new WaitForSeconds(Random.Range(lightTimeS, lightTimeE));
            }

            yield return new WaitForSeconds(Random.Range(gapS, gapE));
        }
        ToggleShadow();
    }

    void ToggleShadow()
    {
        if (lightState == LightState.BLINK)
        {
            shadow.SetActive(!shadow.activeSelf);
            elevatorShadow.SetActive(!elevatorShadow.activeSelf);
        }
    }

    public void NewPlayScene()
    {
        StartCoroutine(PlaySceneCo());
    }

    IEnumerator PlaySceneCo()
    {
        lightState = LightState.OFF;

        shadow.SetActive(true);
        elevatorShadow.SetActive(true);
        shadow.GetComponent<Image>().color = lightOff;
        elevatorShadow.GetComponent<Image>().color = lightOff;

        elevator_Button_Anim.enabled = true;

        Debug.Log(elevator_Button_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        yield return new WaitUntil(() => elevator_Button_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        elevator_Anim.enabled = true;
        elevator_Shadow_Anim.enabled = true;

        yield return new WaitUntil(() => elevator_Anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f);

        yield return new WaitForSeconds(1);

        StageManager.stageIndex = 0;

        SceneTransitionManager.Instance.TransitionToScene(SCENE.Main, SCENE.Play, FadeTypes.Default, FadeTypes.None);
    }

    public void LoadPlayScene(int stageIndex)
    {
        StageManager.stageIndex = stageIndex;

        SceneTransitionManager.Instance.TransitionToScene(SCENE.Main, SCENE.Play, FadeTypes.Default, FadeTypes.None);
    }

    public void OptionWindowOpen()
    {

    }
}

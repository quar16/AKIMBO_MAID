using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


public class OptionMenuManager : MonoSingleton<OptionMenuManager>
{
    public AudioMixer audioMixer; // 오디오 믹서 참조
    public Slider bgmSlider; // BGM 볼륨 조절 슬라이더
    public Slider sfxSlider; // SFX 볼륨 조절 슬라이더
    public GameObject optionsMenu; // 옵션 메뉴 UI
    public VolumeProfile playSceneVolumeProfile;
    DepthOfField depthOfField;

    bool onToggle = false;

    private void Start()
    {
        playSceneVolumeProfile.TryGet(out depthOfField);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OptionMenuToggle();
    }

    public void OptionMenuToggle()
    {
        if (onToggle) return;

        if (optionsMenu.activeSelf)
        {

            StartCoroutine(CloseOptionMenu());
        }
        else
        {
            SetTimeScale(0);
            optionsMenu.SetActive(true);
        }
    }

    void InitializeVolume()
    {
        // BGM 볼륨 슬라이더 초기화
        float bgmVolume;
        audioMixer.GetFloat("BGMVolume", out bgmVolume);
        bgmSlider.value = Mathf.Pow(10, bgmVolume / 20); // dB 값을 슬라이더 값으로 변환

        // SFX 볼륨 슬라이더 초기화
        float sfxVolume;
        audioMixer.GetFloat("SFXVolume", out sfxVolume);
        sfxSlider.value = Mathf.Pow(10, sfxVolume / 20); // dB 값을 슬라이더 값으로 변환
    }

    public void OnBGMVolumeChange()
    {
        // BGM 볼륨 조절
        float bgmVolume = Mathf.Log10(bgmSlider.value) * 20;
        audioMixer.SetFloat("BGMVolume", bgmVolume);
    }

    public void OnSFXVolumeChange()
    {
        // SFX 볼륨 조절
        float sfxVolume = Mathf.Log10(sfxSlider.value) * 20;
        audioMixer.SetFloat("SFXVolume", sfxVolume);
    }

    public void OnResumeGame()
    {
        StartCoroutine(CloseOptionMenu());
    }

    public void OnReturnToMainMenu()
    {
        StartCoroutine(ReturnToMainMenuCoroutine());
    }

    public void OnRestartStage()
    {
        StartCoroutine(RestartStageCoroutine());
    }


    private IEnumerator CloseOptionMenu()
    {
        onToggle = true;

        optionsMenu.SetActive(false);

        float elapsedTime = 0f;

        while (Time.timeScale < 1f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            SetTimeScale(Mathf.Clamp(Mathf.Pow(elapsedTime, 2), 0f, 1f));

            yield return null;
        }
        onToggle = false;
    }

    private IEnumerator RestartStageCoroutine()
    {
        optionsMenu.SetActive(false);
        SetTimeScale(1);
        yield return SceneTransitionManager.Instance.CallFadeEffect(FadeTypes.Default, IO.Out);

        SceneTransitionManager.Instance.RestartStage();
    }

    private IEnumerator ReturnToMainMenuCoroutine()
    {
        optionsMenu.SetActive(false);
        SetTimeScale(1);

        yield return SceneTransitionManager.Instance.CallFadeEffect(FadeTypes.Default, IO.Out);

        SceneTransitionManager.Instance.TransitionToScene(SCENE.Play, SCENE.Main, FadeTypes.None, FadeTypes.Default);
    }

    public void SetTimeScale(float value)
    {
        value = Mathf.Clamp01(value);

        Time.timeScale = value;
        depthOfField.focalLength.value = (1 - value) * 50;
    }
}

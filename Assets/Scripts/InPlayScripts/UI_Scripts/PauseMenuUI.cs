using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuUI : MonoBehaviour
{
    public GameObject pauseMenuUI;

    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        StartCoroutine(ResumeGameCoroutine());
    }

    private IEnumerator ResumeGameCoroutine()
    {
        float duration = 1f;
        float elapsed = 0f;

        pauseMenuUI.SetActive(false);

        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Pow(elapsed / duration, 2);
            yield return null;
        }

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void OpenOptions()
    {
        // Placeholder for opening options menu
    }

    public void GoToMainMenu()
    {
        SceneTransitionManager.Instance.TransitionToScene(SCENE.Play, SCENE.Main, FadeTypes.Default, FadeTypes.Default);
    }
}

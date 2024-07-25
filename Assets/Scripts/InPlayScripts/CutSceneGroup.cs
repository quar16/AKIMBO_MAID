using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneGroup : MonoSingleton<CutSceneGroup>
{
    Dictionary<string, RectTransform> cutSceneDictionary = new();

    public void Init()
    {
        foreach (RectTransform v in GetComponent<RectTransform>())
            cutSceneDictionary.Add(v.gameObject.name, v);
    }

    public void CleanUp()
    {
        Destroy(gameObject);
    }
    
    public IEnumerator CutSceneShow(string name, Vector3 pos, float fadeTime)
    {
        if (cutSceneDictionary.ContainsKey(name))
        {
            RectTransform image = cutSceneDictionary[name];
            image.anchoredPosition = pos;

            CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = image.gameObject.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0;

            image.gameObject.SetActive(true);

            float time = 0;

            while (time < fadeTime)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, time / fadeTime);
                time += Time.deltaTime;
                yield return null;
            }
            
            canvasGroup.alpha = 1;
        }
    }

    public IEnumerator CutSceneHide(string name, float fadeTime)
    {
        if (cutSceneDictionary.ContainsKey(name))
        {
            RectTransform image = cutSceneDictionary[name];

            CanvasGroup canvasGroup = image.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = image.gameObject.AddComponent<CanvasGroup>();

            float time = 0;

            while (time < fadeTime)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, time / fadeTime);
                time += Time.deltaTime;
                yield return null;
            }

            canvasGroup.alpha = 0;
            
            image.gameObject.SetActive(false);
        }
    }

    public IEnumerator CutSceneMove(string name, Vector3 pos, float moveTime)
    {
        if (cutSceneDictionary.ContainsKey(name))
        {
            RectTransform image = cutSceneDictionary[name];
            Vector3 startPos = image.anchoredPosition;
            float time = 0;

            while (time < moveTime)
            {
                image.anchoredPosition = Vector3.Lerp(startPos, pos, time / moveTime);

                time += Time.deltaTime;
                yield return null;
            }

            image.anchoredPosition = pos;
        }

    }

}

using System;
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

            float startTime = Time.time;

            while (Time.time < startTime + fadeTime)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, (Time.time - startTime) / fadeTime);
                yield return PlayTime.ScaledNull;
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


            float startTime = Time.time;

            while (Time.time < startTime + fadeTime)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeTime);
                yield return PlayTime.ScaledNull;
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

            float startTime = Time.time;

            while (Time.time < startTime + moveTime)
            {
                image.anchoredPosition = Vector3.Lerp(startPos, pos, (Time.time - startTime) / moveTime);
                yield return PlayTime.ScaledNull;
            }

            image.anchoredPosition = pos;
        }

    }

}

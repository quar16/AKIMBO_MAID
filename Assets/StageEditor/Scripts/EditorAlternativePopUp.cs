using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorAlternativePopUp : MonoSingleton<EditorAlternativePopUp>
{
    public GameObject popUpBody;
    public Text text;
    Action<bool> connectedFunc;

    public void ShowPopUp(string content, Action<bool> ResultFuc)
    {
        text.text = content;
        connectedFunc = ResultFuc;

        popUpBody.SetActive(true);
    }

    public void ChooseYes()
    {
        connectedFunc?.Invoke(true);
        popUpBody.SetActive(false);
    }

    public void ChooseNo()
    {
        connectedFunc?.Invoke(false);
        popUpBody.SetActive(false);
    }
}

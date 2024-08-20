using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoSingleton<DialogueManager>
{
    public RectTransform dialogueBoxParent;
    public DialogueBox dialogueBox;

    public GameObject talkBubble;

    DialogueBox lastBox;

    public IEnumerator ShowDiaglogue(string characterName, string dialogue)
    {
        ShowTalkBubble(characterName);

        lastBox = Instantiate(dialogueBox, dialogueBoxParent);

        yield return lastBox.ShowDialogue(dialogue);
    }

    public void ShowTalkBubble(string characterNames)
    {
        NamedCharacter chara = NamedCharacter.GetNamedCharacter(characterNames);

        Vector3 charaPosition = chara.transform.position;
        talkBubble.transform.position = charaPosition;

        if (charaPosition.x < Camera.main.transform.position.x)
            talkBubble.transform.localScale = new Vector3(-1, 1, 1);
        else
            talkBubble.transform.localScale = Vector3.one;

        talkBubble.SetActive(true);
    }

    public void HideLastDialogueBox()
    {
        talkBubble.SetActive(false);

        if (lastBox != null)
        {
            lastBox.HideBox();
            lastBox = null;
        }
    }

}

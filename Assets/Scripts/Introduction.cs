using System.Collections;
using UnityEngine;
using System;

public class Introduction : MonoBehaviour
{
    public static event Action StartGame;
    [SerializeField] ShowDialogue dialogue;
    [SerializeField] string introDialogueKey;

    void Start()
    {
        StartCoroutine(dialogue.showText(Dialogues.lines[introDialogueKey]));
        StartCoroutine(callEvent());
    }

    private IEnumerator callEvent()
    {
        yield return new WaitForSeconds(.5f);
        StartGame?.Invoke();
    }
}

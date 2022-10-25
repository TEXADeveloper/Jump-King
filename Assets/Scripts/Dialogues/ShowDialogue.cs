using UnityEngine;
using System.Collections;
using TMPro;

public class ShowDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField, Range(0f, 0.5f)] private float time;
    [SerializeField] private string[] keys;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player"))
        {
            StartCoroutine(showText(Dialogues.lines[keys[Random.Range(0, keys.Length - 1)]]));
        }
    }

    private IEnumerator showText(string phrase)
    {
        string oldPhrase = text.text;
        for (int i = oldPhrase.Length - 1 ; i >= 0 ; i--)
        {
            text.text = "";
            for (int j = 0 ; j < i ; j++)
                text.text += oldPhrase.ToCharArray()[j];
            yield return new WaitForSeconds(time);
        }


        foreach (char c in phrase)
        {
            text.text += c;
            yield return new WaitForSeconds(time);
        }
    }
}

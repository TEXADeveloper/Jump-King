using UnityEngine;
using System.Collections;
using TMPro;

public class ShowDialogue : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private GameObject backgroundImage;
    [SerializeField, Range(0f, 0.5f)] private float time;
    [SerializeField] private string[] keys;
    private bool inGame = false;

    void Start()
    {
        Introduction.StartGame += canStart;
    }

    private void canStart() => inGame = true;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals("Player") && inGame)
        {
            StartCoroutine(showText(Dialogues.lines[keys[Random.Range(0, keys.Length - 1)]]));
        }
    }

    public IEnumerator showText(string phrase)
    {
        backgroundImage.SetActive(true);
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

    void OnDisable()
    {
        Introduction.StartGame -= canStart;
    }
}

using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections;

public class Disapear : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform detection;
    private TilemapCollider2D tileCollider;
    private Animator anim;
    bool isDisappearing = false;

    void Start()
    {
        tileCollider = this.GetComponent<TilemapCollider2D>();
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDisappearing)
            tileCollider.enabled = !(player.position.y < detection.position.y);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag.Equals("Player") && !isDisappearing)
        {
            isDisappearing = !isDisappearing;
            StartCoroutine(startAlert());
        }
    }

    private IEnumerator startAlert()
    {
        yield return new WaitForSeconds(4f);
        anim.SetTrigger("Alert");
    }

    public void TriggerAnim(string name)
    {
        anim.SetTrigger(name);
    }

    public void ActivateCollider()
    {
        tileCollider.enabled = true;
        isDisappearing = false;
    }

    public void DeactivateCollider()
    {
        tileCollider.enabled = false;
    }
}

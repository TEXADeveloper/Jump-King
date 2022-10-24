using UnityEngine;

public class Disapear : MonoBehaviour
{
    [SerializeField] private float time = 5f;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.transform.tag.Equals("Player"))
            Destroy(this.gameObject, time);
    }
}

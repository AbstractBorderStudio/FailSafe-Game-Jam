using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BreakableObject : MonoBehaviour,IEnemy
{
    int bulletLayer;
    [SerializeField] private UnityEvent OnHitBreakable;
    // Start is called before the first frame update
    void Start()
    {
        bulletLayer = LayerMask.NameToLayer("Bullet");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.layer==6) //6 corrisponde a bullet
        {
            Debug.Log("colpito");
            OnHitBreakable.Invoke();
            collision.GetComponent<BulletMovement>().stopMove();
            collision.GetComponent<ParticleSystem>().Play();
            collision.GetComponent<SpriteRenderer>().enabled = false;
            collision.GetComponent<CircleCollider2D>().enabled = false;
            StartCoroutine(DestroyBreakable());
            StartCoroutine(DeactivateBullet(collision));

        }
    } 
    private IEnumerator DeactivateBullet(Collider2D collision)
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Destroy(collision.gameObject);
    }
    private IEnumerator DestroyBreakable()
    {
        yield return new WaitForSeconds(0.34f);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(gameObject);
    }
    private IEnumerator DestroyBreakableShield()
    {
        yield return new WaitForSeconds(0.15f);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        yield return new WaitForSeconds(2f);
        GameObject.Destroy(gameObject);
    }

    public void Repel()
    {
        
        StartCoroutine(DestroyBreakableShield());
    }
}

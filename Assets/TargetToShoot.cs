using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TargetToShoot : MonoBehaviour
{
    int bulletLayer;
    private bool isActive = false;
    [SerializeField] private UnityEvent OnHitTarget;
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

        if (collision.gameObject.layer == 6) //6 corrisponde a bullet
        {
            OnHitTarget.Invoke();
            collision.GetComponent<BulletMovement>().stopMove();
            collision.GetComponent<ParticleSystem>().Play();
            collision.GetComponent<SpriteRenderer>().enabled = false;
            collision.GetComponent<BoxCollider2D>().enabled = false;

            if (!isActive)
            {
                //StartCoroutine(ActivateTarget());
                isActive = true;
                GetComponentInChildren<ParticleSystem>().Play();
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
            }
            StartCoroutine(DeactivateBullet(collision));

        }
    }

    private IEnumerator DeactivateBullet(Collider2D collision)
    {
        yield return new WaitForSeconds(2.0f);
        GameObject.Destroy(collision.gameObject);
    }
   
}

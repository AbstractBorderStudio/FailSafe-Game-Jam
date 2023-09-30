using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private bool isShieldActive = false;
    private bool canUseShield = true;
    [SerializeField] float timeOutShield = 3f;
    [SerializeField] float timeProtectionShield = 2f;
    public CircleCollider2D shieldCollider;
    public ParticleSystem shieldRenderer;
    // Start is called before the first frame update
    void Start()
    {
        shieldCollider.enabled = false;
        shieldRenderer.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShieldActive && canUseShield)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartCoroutine(ActivateShield());
            }
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("detection");
        if (isShieldActive)
        {
            IEnemy a;
            if(collision.gameObject.TryGetComponent<IEnemy>(out a))
            {
                a.Repel();
            }

        }
    }


    private IEnumerator ActivateShield()
    {
        //la devo chiamare quando scudo scade di durata
        isShieldActive = true;
        //attiva collider e output grafico
        shieldCollider.enabled = true;
        shieldRenderer.Play();
        Debug.Log("Attivo Scudo");
        yield return new WaitForSeconds(timeProtectionShield);
        shieldRenderer.Stop();
        shieldCollider.enabled = false;
        yield return new WaitForSeconds(1.35f);
        isShieldActive = false;
        Debug.Log("Disttivo Scudo");
        StartCoroutine(WaitToActivateShield());

    }
    private IEnumerator WaitToActivateShield()
    {
        canUseShield = false;
        yield return new WaitForSeconds(timeOutShield);
        canUseShield = true;

    }
    public bool GetShieldIsActive()
    {
        return (isShieldActive);
    }
    }


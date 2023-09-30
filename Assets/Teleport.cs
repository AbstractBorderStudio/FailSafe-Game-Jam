using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public Teleport t1;
    public Teleport t2;
    [SerializeField] bool isPos1;
    [SerializeField] bool isPos2;
    public GameObject player;
    private bool canTeleport=true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("StarShip"))
        {
            if (isPos1&& canTeleport)
            {
                collision.gameObject.transform.position = pos2.position;
                StartCoroutine(teleportDeactivationForALittleBit(1));
            }
            else if (isPos2&& canTeleport)
            {
                collision.gameObject.transform.position = pos1.position;
                StartCoroutine(teleportDeactivationForALittleBit(2));
            }
        }
    }
    private IEnumerator teleportDeactivationForALittleBit(int a)
    {
        if (a == 1)
        {
            t2.canTeleport = false;
            yield return new WaitForSeconds(2f);
            //invoca su t2 funzione dan per depotenziare gravità
            t2.canTeleport = true;
        }
        if (a == 2)
        {
            t1.canTeleport = false;
            yield return new WaitForSeconds(2f);
            //invoca su t1 funzione dan per depotenziare gravità
            t1.canTeleport = true;
        }
        
    }
}

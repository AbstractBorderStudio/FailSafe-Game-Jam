using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetAttraction : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D playerBody;
    [SerializeField] float influenceRange;
    [SerializeField] float intensity;
    private float distanceToPlayer;
    Vector2 pullForce;
 
    void Start()
    {
        playerBody = player.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        distanceToPlayer = Vector2.Distance(player.position, transform.position);
        if (distanceToPlayer <= influenceRange)
        {
            pullForce = (transform.position - player.position).normalized / distanceToPlayer * intensity;
            playerBody.AddForce(pullForce, ForceMode2D.Force);
        }
    }
}

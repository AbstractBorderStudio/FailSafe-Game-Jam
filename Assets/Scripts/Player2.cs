using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Player2 : MonoBehaviour
{
    const int MAXPOINTS = 500;
    Transform transform;

    [SerializeField] 
    private float playerSpeed = 1.0f,
        orbitSpeed = 1.0f,
        orbitRange = 5.0f,
        orbitForce = 1.0f;
    
    Vector3 direction = Vector3.up;

    private void Start()
    {
        transform = GetComponent<Transform>();
    }

    private void Update()
    {
        transform.Translate(direction * playerSpeed * Time.deltaTime);

        PlanetAttraction();
    }

    private void PlanetAttraction()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");

        foreach (GameObject planet in planets)
        {
            direction = planet.transform.position - transform.position;
            if (Vector3.Magnitude(direction) < orbitRange)
            {
                GetComponent<Rigidbody2D>().AddForce(direction.normalized * orbitForce);
            }
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Player : MonoBehaviour
{
    const int MAXPOINTS = 500;
    Transform transform;

    [SerializeField] 
    private float playerSpeed = 1.0f,
        orbitSpeed = 1.0f,
        attractionRange = 1.0f;        
    float orbitRange;


    [SerializeField]
    private float traslationSpeed = 0.5f,
        rotationSpeed = 0.1f;
    
    private Transform currentPlanet;
    private bool isOrbiting = false;
    private float orbitAngle = 0;
    private Vector3 direction = Vector3.up;
    private LineRenderer lr;

    void Start()
    {
        transform = GetComponent<Transform>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    void Update()
    {   
        if (isOrbiting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isOrbiting = false;
            }
            if (currentPlanet != null)
            {
                float x = currentPlanet.position.x + orbitRange * Mathf.Cos(orbitAngle);
                float y = currentPlanet.position.y + orbitRange * Mathf.Sin(orbitAngle);

                Vector3 newPos = new Vector3(x, y, 0);
                
                transform.position = Vector3.Lerp(transform.position, newPos, traslationSpeed);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, transform.position - currentPlanet.position), rotationSpeed);
                direction = (transform.position - currentPlanet.position).normalized;

                orbitAngle += orbitSpeed * Time.deltaTime;
                if (orbitAngle > 360f) orbitAngle = -360f;
            }
            else
            {
                isOrbiting = false;
            }
        }
        else
        {
            transform.position += playerSpeed * direction * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
            
            GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");
            foreach (GameObject p in planets)
            {
                Vector3 dist = p.transform.position - transform.position;
                if (Vector3.Magnitude(dist) < attractionRange)
                    direction = Vector3.Lerp(direction, dist, 0.001f);
            }

        }
        
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, transform.position);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        currentPlanet = col.GetComponent<Transform>();
        orbitRange = col.GetComponent<CircleCollider2D>().radius;
        float impactAngle = Vector3.SignedAngle(Vector3.right, transform.position - currentPlanet.position, Vector3.forward);
        if (impactAngle < 0) impactAngle = 360f - impactAngle * -1f;
        orbitAngle = Mathf.Deg2Rad * impactAngle;
        isOrbiting = true;
    }

    [SerializeField]
    float attraction = 1.0f;

    public void Attract(Vector3 dir)
    {
        direction = Vector3.RotateTowards(direction, dir, 0.01f, 0.01f);
    }
}

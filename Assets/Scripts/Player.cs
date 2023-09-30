using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Transform transform;

    [SerializeField] 
    private float playerSpeed = 1.0f,
        orbitSpeed = 1.0f,
        orbitRange = 1.0f;
    
    private Transform currentPlanet;
    private bool isOrbiting = false;
    private float orbitAngle = 0;
    private Vector3 direction = Vector3.up;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 0;
    }

    

    // Update is called once per frame
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
                
                transform.position = Vector3.Lerp(transform.position, newPos, 0.05f);
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, transform.position - currentPlanet.position), 0.1f);
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
        }
        
        lr.positionCount++;
        lr.SetPosition(lr.positionCount - 1, transform.position);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        currentPlanet = col.GetComponent<Transform>();
        float impactAngle = Vector3.SignedAngle(Vector3.right, transform.position - currentPlanet.position, Vector3.forward);
        if (impactAngle < 0) impactAngle = 360f - impactAngle * -1f;
        orbitAngle = Mathf.Deg2Rad * impactAngle;
        isOrbiting = true;
    }
}

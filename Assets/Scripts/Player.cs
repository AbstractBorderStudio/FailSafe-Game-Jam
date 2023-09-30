using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

enum PlayerState
{
    None,
    Traveling,
    Orbiting,
    Dead
}

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField]
    PlayerState currentState;
    [SerializeField]
    float targetSpeed = 1.0f,
        gravityScale = 0.5f;
    Vector3 direction;
    Transform currentPlanet;
    [SerializeField]
    float orbitSpeed = 1.0f;
    float orbitRange, orbitAngle;
    float orientation = 1f;

    void Start()
    {
        direction = Vector3.up;
    }

    void Update()
    {
        switch (currentState)
        {
            case (PlayerState.Traveling):
                ComputeAttractionDirection();
                
                break;
            case (PlayerState.Orbiting):
                if (Input.GetKeyDown(KeyCode.Space)) 
                    currentState = PlayerState.Traveling;
                MoveInOrbit();
                break;
            case (PlayerState.Dead):
                return;
            default:
                break;
        }

        MoveForward();
        // change rotation depending on current movement direction
        UpdateRotation();
        // draw line
        UpdateLine();
    }

    #region Movement
    void MoveForward()
    {
        // move along direction
        if (currentState == PlayerState.Traveling)
            transform.position += direction.normalized * targetSpeed * Time.deltaTime;
        else if (currentState == PlayerState.Orbiting)
            transform.position += direction.normalized * targetSpeed * Time.deltaTime;
    }

    void ComputeAttractionDirection()
    {
        GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");
        foreach (GameObject planet in planets)
        {
            Vector3 dist = planet.transform.position - transform.position;
            direction += dist.normalized / Mathf.Pow(Vector3.Magnitude(dist), 2.0f) * gravityScale; 
        }
    }

    void UpdateRotation()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }    

    void MoveInOrbit()
    {
        Vector3 target = Quaternion.Euler(new Vector3(0, 0, -90f)) * (transform.position - currentPlanet.position).normalized;//> * orientation;
        direction = target;
        
        // float x = currentPlanet.position.x + orbitRange * Mathf.Cos(orbitAngle);
        // float y = currentPlanet.position.y + orbitRange * Mathf.Sin(orbitAngle);

        // Vector3 newPos = new Vector3(x, y, 0);

        // transform.position = Vector3.Lerp(transform.position, newPos, 0.1f);

        // direction = (transform.position - currentPlanet.position).normalized;
        
        // orbitAngle += orbitSpeed * Time.deltaTime;
        // if (orbitAngle > 360f) orbitAngle = -360f;
    }

    #endregion

    #region Draw
    private LineRenderer line;
    void UpdateLine()
    {
        if (!line)
        {
            line = GetComponent<LineRenderer>();
            line.positionCount = 0;
        }

        line.positionCount++;
        line.SetPosition(line.positionCount - 1, transform.position);
    }
    #endregion

    #region CollisionHandling
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "planet") // colliding with a planet orbit
        {
            currentState = PlayerState.Orbiting;
            currentPlanet = target.GetComponent<Transform>();
            orbitRange = target.GetComponent<CircleCollider2D>().radius;
            // float impactAngle = Vector3.SignedAngle(Vector3.right, transform.position - currentPlanet.position, Vector3.forward);
            // if (impactAngle < 0) impactAngle = 360f - impactAngle * -1f;
            // orbitAngle = Mathf.Deg2Rad * impactAngle;

            // calculate orientation
            float a = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
            // if (a > 0 && a <= 90f)
            // {
            //     orientation = -1f;
            // }
            // else if (a > 90f  && a < 180f)
            // {
            //     orientation = 1f;
            // }
            // else if (a < 0 && a >= -90f)
            // {
            //     orientation = -1f;
            // }
            // else if (a < -90f  && a >= 180f)
            // {
            //     orientation = 1f;
            // }
        }
        else if (target.tag == "obstacle") // colliding with an obstacle
        {
            Die();
        }
    }
    #endregion

    #region Events
    [SerializeField]
    private UnityEvent OnDeath;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private ParticleSystem smokeTrail, explosion;
    private void Die()
    {
        currentState = PlayerState.Dead;
        sprite.enabled = false;
        smokeTrail.Stop();
        explosion.Play();
        OnDeath.Invoke();
    }   
    public void RestartLevel()
    {
        Scene current  = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }
    #endregion
}


// [SerializeField]
//     private UnityEvent OnDeath;
//     const int MAXPOINTS = 500;
//     Transform transform;

//     [SerializeField] 
//     private float playerSpeed = 1.0f,
//         orbitSpeed = 1.0f;     
//     float orbitRange;


//     [SerializeField]
//     private float traslationSpeed = 0.5f,
//         rotationSpeed = 0.1f,
//         attraction = 1.0f;
//     [SerializeField]
//     private SpriteRenderer sprite;
//     [SerializeField]
//     private ParticleSystem explosion;
    
//     private Transform currentPlanet;
//     private bool isOrbiting = false;
//     private float orbitAngle = 0;
//     private Vector3 direction = Vector3.up;
//     private LineRenderer lr;
//     private bool dead = false;

//     [SerializeField]
//     ParticleSystem smokeTrail;

//     [SerializeField]
//     private CinemachineVirtualCamera pov, full;

//     void Start()
//     {
//         transform = GetComponent<Transform>();
//         lr = GetComponent<LineRenderer>();
//         lr.positionCount = 0;
//     }

//     void Update()
//     {   
//         if (dead)
//         {
//             return;
//         }
//         if (isOrbiting)
//         {
//             if (pov.Priority > full.Priority)
//             {
//                 pov.Priority = 5;
//                 full.Priority = 10;
//             }
//             if (smokeTrail.isEmitting)
//             {
//                 smokeTrail.Stop();
//             }
//             if (Input.GetKeyDown(KeyCode.Space))
//             {
//                 isOrbiting = false;
//             }
//             if (currentPlanet != null)
//             {
//                 float x = currentPlanet.position.x + orbitRange * Mathf.Cos(orbitAngle);
//                 float y = currentPlanet.position.y + orbitRange * Mathf.Sin(orbitAngle);

//                 Vector3 newPos = new Vector3(x, y, 0);
                
//                 transform.position = Vector3.Lerp(transform.position, newPos, traslationSpeed);
//                 transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, transform.position - currentPlanet.position), rotationSpeed);
//                 direction = (transform.position - currentPlanet.position).normalized;

//                 orbitAngle += orbitSpeed * Time.deltaTime;
//                 if (orbitAngle > 360f) orbitAngle = -360f;
//             }
//             else
//             {
//                 isOrbiting = false;
//             }
//         }
//         else
//         {
//             if (pov.Priority < full.Priority)
//             {
//                 pov.Priority = 10;
//                 full.Priority = 5;
//             }
//             if (!smokeTrail.isEmitting)
//             {
//                 smokeTrail.Play();
//             }
//             transform.position += playerSpeed * direction * Time.deltaTime;
//             transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);

//             Vector3 minDist = Vector3.right * 1000.0f;
            
//             GameObject[] planets = GameObject.FindGameObjectsWithTag("planet");
//             foreach (GameObject p in planets)
//             {
//                 Vector3 dist = p.transform.position - transform.position;
//                 if (Vector3.Magnitude(dist) < Vector3.Magnitude(minDist))
//                 {
//                     minDist = dist;
//                 }  
//             }
//             direction = Vector3.Lerp(direction, minDist, attraction/Mathf.Pow(Vector3.Magnitude(minDist),2f) * Time.deltaTime);
//         }
        
//         lr.positionCount++;
//         lr.SetPosition(lr.positionCount - 1, transform.position);
//     }

//     void OnTriggerEnter2D(Collider2D col)
//     {
//         if (col.tag == "planet")
//         {
//             currentPlanet = col.GetComponent<Transform>();
//             orbitRange = col.GetComponent<CircleCollider2D>().radius;
//             float impactAngle = Vector3.SignedAngle(Vector3.right, transform.position - currentPlanet.position, Vector3.forward);
//             if (impactAngle < 0) impactAngle = 360f - impactAngle * -1f;
//             orbitAngle = Mathf.Deg2Rad * impactAngle;
//             isOrbiting = true;
//         }
//         else if (col.tag == "obstacle")
//         {
//             Die();
//         }
//     }

//     private void Die()
//     {
//         dead = true;
//         sprite.enabled = false;
//         smokeTrail.Stop();
//         explosion.Play();
//         OnDeath.Invoke();
//     }   


//     public void RestartLevel()
//     {
//         Scene current  = SceneManager.GetActiveScene();
//         SceneManager.LoadScene(current.name);
//     }
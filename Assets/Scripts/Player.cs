using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

enum PlayerState
{
    None,
    Traveling,
    Orbiting,
    Win,
    Dead
}

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(LineRenderer))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnLeavePlanet;
    [SerializeField]
    PlayerState currentState;
    [SerializeField]
    float targetSpeed = 1.0f,
        gravityScaleMax = 0.5f;
    float gravityScale;
    Vector3 direction;
    public Vector3 Direction {
        get {return direction;} 
        private set{}
        }
    Transform currentPlanet; 
    float orientation = 1f;
    GameObject[] planets;
    
    [SerializeField]
    float gravityCooldown = 0.2f;
    float gravityCooldownCount = 0;

    void Start()
    {
        currentState = PlayerState.None;
        direction = Vector3.up;
        planets = GameObject.FindGameObjectsWithTag("planet");
        gravityScale = gravityScaleMax;
    }

    void Update()
    {
        MoveForward();

        switch (currentState)
        {
            case (PlayerState.None):
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    currentState = PlayerState.Traveling;
                    OnLeavePlanet.Invoke();
                }
                    
                break;
            case (PlayerState.Traveling):
                if (!smokeTrail.isEmitting) smokeTrail.Play();
                if (gravityCooldownCount > 0)
                {
                    gravityCooldownCount -= Time.deltaTime;
                }
                else
                {
                    gravityScale = gravityScaleMax;
                }
                ComputeAttractionDirection();
                break;
            case (PlayerState.Orbiting):
                if (smokeTrail.isEmitting) smokeTrail.Stop();
                if (Input.GetKeyDown(KeyCode.Space)) 
                    currentState = PlayerState.Traveling;
                    TimedDisableGravity();
                    OnLeavePlanet.Invoke();
                MoveInOrbit();
                break;
            case (PlayerState.Dead):
                if (Input.GetKeyDown(KeyCode.Space))
                    RestartLevel();
                return;
            case (PlayerState.Win):
                if (Input.GetKeyDown(KeyCode.Space))
                    RestartLevel();
                return;
            default:
                break;
        }

        
        // change rotation depending on current movement direction
        UpdateRotation();
        // draw line
        UpdateLine();
    }

    #region Movement
    void MoveForward()
    {
        Vector3 velocity = Vector3.zero;
        // move along direction
        if (currentState == PlayerState.Traveling)
            velocity = direction.normalized * targetSpeed * Time.deltaTime;
        else if (currentState == PlayerState.Orbiting)
            velocity = direction * Time.deltaTime;
        

        transform.position += velocity;
    }

    void ComputeAttractionDirection()
    {
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
        Vector3 target = Quaternion.Euler(new Vector3(0, 0, -90f)) * (transform.position - currentPlanet.position).normalized * orientation;
        direction = target * targetSpeed + (currentPlanet.position - transform.position).normalized / targetSpeed;
    }

    public void TimedDisableGravity()
    {   
        gravityScale = 0;
        gravityCooldownCount = gravityCooldown;
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
        Planet current;
        if (target.tag == "planet" && target.TryGetComponent<Planet>(out current)) // colliding with a planet orbit
        {
            if (currentState == PlayerState.Orbiting) return;
            currentState = PlayerState.Orbiting;
            currentPlanet = target.GetComponent<Transform>();

            // calculate orientation
            float a = Vector3.SignedAngle(Vector3.right, direction, Vector3.forward);
            if (a > 0 && a <= 90f)
            {
                orientation = -1f;
            }
            else if (a > 90f  && a < 180f)
            {
                orientation = 1f;
            }
            else if (a < 0 && a >= -90f)
            {
                orientation = -1f;
            }
            else if (a < -90f  && a >= -180f)
            {
                orientation = 1f;
            }

            current.Explored();

            foreach (GameObject planet in planets)
            {
                Planet p;
                if (planet.TryGetComponent<Planet>(out p))
                {
                    if (!p.IsExplored)
                    return;
                }
                else continue;
            }

            // if all planets are explored
            Win();
        }
        else if (target.tag == "obstacle") // colliding with an obstacle
        {
            Die();
        }
    }
    #endregion

    #region Events
    [SerializeField]
    private UnityEvent OnDeath, OnWin;
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

    public void Win()
    {
        Debug.Log("Hai vinto!");
        currentState = PlayerState.Win;
        smokeTrail.Stop();
        OnWin.Invoke();
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
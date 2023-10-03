using System;
using System.Collections;
using System.Collections.Generic;
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
    private UnityEvent OnLeavePlanet, OnExitWin, OnStart;
    [SerializeField]
    PlayerState currentState;
    [SerializeField]
    float targetSpeed = 1.0f,
        gravityScaleMax = 0.5f;
    float gravityScale;
    [SerializeField]
    Vector3 direction = Vector3.up;
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
        planets = GameObject.FindGameObjectsWithTag("planet");
        gravityScale = gravityScaleMax;
    }

    void Update()
    {
        MoveForward();

        switch (currentState)
        {
            case (PlayerState.None):
                if (smokeTrail.isEmitting) smokeTrail.Stop();
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    OnStart.Invoke();
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
                /*if (Input.GetKeyDown(KeyCode.Space))
                    RestartLevel();*/
                return;
            case (PlayerState.Win):
                if (Input.GetKeyDown(KeyCode.Space))
                    OnExitWin.Invoke();
                MoveInOrbit();
                break;
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
        else if (currentState == PlayerState.Orbiting || currentState == PlayerState.Win)
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
        Vector3 target;
        target = Quaternion.Euler(new Vector3(0, 0, -90f)) * (transform.position - currentPlanet.position).normalized * orientation;
        if (currentState == PlayerState.Orbiting)
            direction = target * targetSpeed + (currentPlanet.position - transform.position).normalized / targetSpeed;
        else if (currentState == PlayerState.Win)
            direction = target;
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
        if (currentState == PlayerState.Win) return;
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
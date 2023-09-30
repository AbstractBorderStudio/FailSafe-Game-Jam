using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Planet : MonoBehaviour
{
    [SerializeField]
    private UnityEvent OnExplore;

    [SerializeField]
    private Color exploredColor;
    [SerializeField]
    private ParticleSystem exploredParticle;
    public bool IsExplored {get; set;} = false;
    public SpriteRenderer sprite;
    private Player p;
    private Transform t;
    
    void Start()
    {
        t = GetComponent<Transform>();
        p = GameObject.FindAnyObjectByType<Player>();
    }

    public void Explored()
    {
        if (!IsExplored) exploredParticle.Play();
        sprite.color = exploredColor;
        IsExplored = true;
        OnExplore.Invoke();
    }
}

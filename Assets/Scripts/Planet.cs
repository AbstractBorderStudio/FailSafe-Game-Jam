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
    private Fade fade;
    //[SerializeField]
    //int index;
    
    void Start()
    {
        t = GetComponent<Transform>();
        p = GameObject.FindAnyObjectByType<Player>();
        //fade = FindObjectOfType<Fade>();
    }

    public void Explored()
    {
        if (!IsExplored) exploredParticle.Play();
        sprite.color = exploredColor;
        IsExplored = true;
        //fade.SetSpot(index, transform.position);
        OnExplore.Invoke();
    }
}

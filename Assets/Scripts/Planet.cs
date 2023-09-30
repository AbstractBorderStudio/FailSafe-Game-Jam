using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Planet : MonoBehaviour
{
    private Player p;
    private Transform t;
    // Start is called before the first frame update
    void Start()
    {
        t = GetComponent<Transform>();
        p = GameObject.FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 pPos = p.GetComponent<Transform>().position;

        // if (Vector3.Magnitude(t.position - pPos) < 10.0f)
        // {
        //     p.Attract(t.position - pPos);
        // }
    }
}

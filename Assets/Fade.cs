using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Fade : MonoBehaviour
{
    [SerializeField]
    Material mat;
    [SerializeField]
    Animator anim;

    void Start()
    {
    }

    public void SetSpot(int i, Vector3 pos)
    {
        mat.SetVector("_pos" + i, pos);
        anim.Play("_limit" + i);
    }

    public void ResetSpot(int i)
    {
        mat.SetFloat("_limit" + i, 0f);
        mat.SetVector("_pos" + i, Vector3.zero);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField]
    private Animator anim;

    public void Shake()
    {
        anim.SetTrigger("shake");
    }
}

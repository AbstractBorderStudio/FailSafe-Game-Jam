using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SituaWin : MonoBehaviour
{
    [SerializeField] private GameObject canvasWin;
    [SerializeField] private GameObject canvasSettings;
    [SerializeField] private Animator animator;

    public void Win()
    {
        canvasSettings.SetActive(false);
        canvasWin.SetActive(true);
        animator.SetTrigger("Win");
    }
}

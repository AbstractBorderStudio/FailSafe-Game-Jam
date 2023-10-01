using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void NextLiv()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

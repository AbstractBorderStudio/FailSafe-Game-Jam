using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SituaDeath : MonoBehaviour
{
    [SerializeField] private GameObject canvasDeath;
    [SerializeField] private GameObject canvasSettings;

    public void Death()
    {
        canvasSettings.SetActive(false);
        canvasDeath.SetActive(true);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

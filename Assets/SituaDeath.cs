using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SituaDeath : MonoBehaviour
{
    [SerializeField] private GameObject canvasDeath;
    [SerializeField] private GameObject canvasSettings;

    public void Death()
    {
        canvasSettings.SetActive(false);
        canvasDeath.SetActive(true);
    }
}

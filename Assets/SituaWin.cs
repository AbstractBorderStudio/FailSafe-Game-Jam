using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SituaWin : MonoBehaviour
{
    [SerializeField] private GameObject canvasWin;
    [SerializeField] private GameObject canvasSettings;
    

    public void Win()
    {
        canvasSettings.SetActive(false);
        canvasWin.SetActive(true);
    }
}

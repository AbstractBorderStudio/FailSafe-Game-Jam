using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] List<GameObject> Livelli = new List<GameObject>();
    [SerializeField] private GameObject currentLiv;
    [SerializeField] private GameObject nextLiv;
    [SerializeField] private GameObject prevLiv;
    [SerializeField] private int contatoreLiv = 0;

    [SerializeField] private Button frecciaDX;
    [SerializeField] private Button frecciaSX;

    [SerializeField] private AudioSource audio;
    void Start()
    {
        
        nextLiv = Livelli[1];
        for (int i = 0; i < Livelli.Count; i++)
        {
            if (i==0)
            {
                currentLiv = Livelli[i];
                currentLiv.GetComponent<Animator>().Play("Dentro");
            }

            if (i==1)
            {
                nextLiv = Livelli[i];
            }

            if (i>0)
            {
                Livelli[i].GetComponent<Animator>().Play("FuoriDX");
            }
        }
        
    }

    void Update()
    {
        if (contatoreLiv>0)
        {
            frecciaSX.interactable = true;
        }

        if (contatoreLiv == Livelli.Count-2)
        {
            frecciaDX.interactable = true;
        }

        if (contatoreLiv == 0)
        {
            contatoreLiv = 0;
            frecciaSX.interactable = false;
        }

        if (frecciaDX.interactable && Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetCliccatoSX();
            audio.Play();
        }

        if (frecciaSX.interactable && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetCliccatoDX();
            audio.Play();
        }
    }
    
    public void SetCliccatoSX()
    {
        currentLiv.GetComponent<Animator>().SetTrigger("UscitaSX");
        nextLiv.GetComponent<Animator>().SetTrigger("EntrataDX");
        prevLiv = currentLiv;
        currentLiv = nextLiv;
        contatoreLiv += 1;
        
        if (contatoreLiv == Livelli.Count-1)
        {
            nextLiv = Livelli[contatoreLiv];
            frecciaDX.interactable = false;
        }
        else 
        {
            nextLiv = Livelli[contatoreLiv+1];
        }
    }

    public void SetCliccatoDX()
    {
        currentLiv.GetComponent<Animator>().SetTrigger("UscitaDX");
        prevLiv.GetComponent<Animator>().SetTrigger("EntrataSX");
        nextLiv = currentLiv;
        currentLiv = prevLiv;
        contatoreLiv -= 1;

        if (contatoreLiv == 0)
        {
            prevLiv = Livelli[0];
        }
        else
        {
            prevLiv = Livelli[contatoreLiv - 1];
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LivManager : MonoBehaviour
{
    [SerializeField] private int contatore;
    [SerializeField] private GameObject LevelSelector;
    
    // Start is called before the first frame update
    void Start()
    {
        contatore = LevelSelector.GetComponent<LevelSelector>().GetContatore();
    }

    // Update is called once per frame
    void Update()
    {
        contatore = LevelSelector.GetComponent<LevelSelector>().GetContatore();
    }

    public void Play()
    {
        SceneManager.LoadScene(contatore + 2);
    }
}

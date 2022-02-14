using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    void Awake()
    {

    }

    void Update()
    {
        
    }

    public void StartButton()
    {
        if (PlayerPrefs.HasKey("Max Stage"))
        {
            SceneManager.LoadScene("Main Scene");
        }
        else
        {
            SceneManager.LoadScene("Tutorial Scene");
        }
    }
}

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
        SceneManager.LoadScene("Main Scene");
    }
}

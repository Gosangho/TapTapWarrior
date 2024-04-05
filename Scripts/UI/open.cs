using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class open : MonoBehaviour
{
    public Button Test1Button;
    public Button Test2Button;
    void Start()
    {
        Test1Button.onClick.AddListener(Test1Scene);
        Test2Button.onClick.AddListener(Test2Scene);
    }

    
    public void Test1Scene()
    {
        SceneManager.LoadScene(2);
    }
    public void Test2Scene()
    {
        SceneManager.LoadScene(1);
    }
}

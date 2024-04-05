using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setcullingmask : MonoBehaviour
{
    private Camera lobbyCamera;
    
    void Start()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        if (currentSceneName == "TabTabs/Scenes/lobby")
        {
            lobbyCamera.cullingMask |= LayerMask.GetMask("UI");
        }
        else
        {
            lobbyCamera.cullingMask &= ~LayerMask.GetMask("UI");
        }

    }

}

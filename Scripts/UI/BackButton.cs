using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BackButton : MonoBehaviour
{
    public GameObject targetUi;

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && targetUi.activeSelf == false)
        {
            Debug.Log("Escape");
            targetUi.SetActive(true);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }

    public void TargetUiQuit()
    {
        targetUi.SetActive(false);
    }

    public void OnApplicationQuit()
    {
        DataManager.Instance.SaveGameData();
    }
}

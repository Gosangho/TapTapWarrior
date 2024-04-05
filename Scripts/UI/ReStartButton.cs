using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ReStartButton : MonoBehaviour
{
    public void ReStartButton2()
    {
        StartCoroutine(ReStart());
    }
    IEnumerator ReStart()
    {
        Time.timeScale = 1.0f;
        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(3);
    }
    public void TutorialReStart()
    {
        SceneManager.LoadScene(6);
    }
}

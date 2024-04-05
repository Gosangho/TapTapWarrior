using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SkipButton : MonoBehaviour
{
    public Image Image;
    public Sprite GoImage;
    FadeScene FadeSceneInstance;
    private void Start()
    {
        Image = GetComponent<Image>();
        FadeSceneInstance = FindObjectOfType<FadeScene>();
    }
    public void LobbyButton()
    {
        DataManager.Instance.playerData.TutorialPlay = true;
        DataManager.Instance.selectCharacter(4,"true");
        FadeSceneInstance.LoadLobbyScene();
    }
}

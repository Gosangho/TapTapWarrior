using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


 public class LoadingScene : MonoBehaviour
 {
     public Slider loadingSliderBar;
     public TextMeshProUGUI loadingNumberText;
     public AsyncOperation asyncOperation;

     public GameObject loadingImage;

     public Camera cam;

     void Start()
     {
         ContinueButton.continueButtonClick = false;
         DataManager.Instance.SaveGameData();
   
         StartCoroutine(Loading());
     }

     IEnumerator Loading()
     {
         if (FadeScene.isLobby)
         {
             asyncOperation = SceneManager.LoadSceneAsync("TabTabs/Scenes/lobby");
             asyncOperation.allowSceneActivation = false;
             FadeScene.isLobby = false;
         }
         else if (FadeScene.isBattle)
         {
             asyncOperation = SceneManager.LoadSceneAsync("TabTabs/Scenes/Test3Battle 1");
             asyncOperation.allowSceneActivation = false;
             FadeScene.isBattle = false;
         }
         else if (FadeScene.isTutorial)
         {
             asyncOperation = SceneManager.LoadSceneAsync("TabTabs/Scenes/Tutorial");
             asyncOperation.allowSceneActivation = false;
             FadeScene.isTutorial = false;
         }


         float duration = 5f; 
         float targetValue = 1f;
         float startTime = Time.time;

         while (loadingSliderBar.value < targetValue)
         {
             float progress = (Time.time - startTime) / duration;
             loadingSliderBar.value = progress;

             float scaleValue = loadingSliderBar.value * 100f;
             int intValue = Mathf.RoundToInt(scaleValue);
             loadingNumberText.text = intValue.ToString();

             yield return null;
         }

         loadingSliderBar.value = targetValue;
         int finalValue = Mathf.RoundToInt(targetValue * 100f);
         loadingNumberText.text = finalValue.ToString();

         
         cam.rect = new Rect(0, 0, 1, 1);
         cam.clearFlags = CameraClearFlags.SolidColor;
         cam.backgroundColor = Color.black;

         loadingImage.SetActive(true);

         yield return new WaitForSeconds(0.1f);
         
         asyncOperation.allowSceneActivation = true;

     }
 }



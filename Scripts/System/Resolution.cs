using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Resolution : MonoBehaviour
{
    // 1. 기기의 해상도가 게임화면보다 "넓은" 경우
    //- 게임화면의 너비(W)가 감소해야 하고 X의 값이 변경되어야한다. 
    //- 1920x1080의 해상도 게임을 200x100해상도 기기에 대응한다면
    //- X = [1-{(1920/1080)}/(200/100)}] / 2
    //- Y = 0 // 움직이지 않음
    //- W = (1920/1080)/(200/100)
    //- H = 1 // 높이유지

    // 2. 기기의 해상도가 게임화면보다 더 "높은"경우
    //- 게임화면의 높이가 감소해야 하며 Y의 값이 변경되어야 한다.
    //- X = 0 // 움직이지 않음
    //- Y = [1-{(100/200)/(1920/1080)}] /2
    //- W = 1 // 게임화면의 너비는 유지
    //- H = (100/200)/(1920/1080)


    public GameObject loadingImage;


    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        int targetWidth = 540;
        int targetHeight = 860;

        float tolerance = 0.1f;
        if ("lobby".Equals(sceneName) || "Opening".Equals(sceneName))
        {
            if (IsScreenRatioClose(Screen.width, Screen.height, targetWidth, targetHeight, tolerance))
            {
                StartCoroutine(cameraFull());
            }
        }
        else
        {
            StartCoroutine(cameraFull());
        }
    }

    private bool IsScreenRatioClose(int width, int height, int targetWidth, int targetHeight, float tolerance)
    {
        bool isChange = false;
        float fixedWidth = 540f;
        float fixedHeight = 860f;

        // 기기의 실제 화면 크기
        float deviceWidth = Screen.width;
        float deviceHeight = Screen.height;

        // 고정 비율에 따라 확대된 height 계산
        float scaledHeight = (fixedHeight / fixedWidth) * deviceWidth;

        if (scaledHeight > deviceHeight)
        {
            isChange = true;
        }
        else
        {
            isChange = false;
        }

        Debug.Log("ratioDifference : " + scaledHeight + "/" + deviceHeight + "/" + isChange);
        return isChange;
    }

    IEnumerator cameraFull()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        // 씬의 이름을 얻기
        string sceneName = currentScene.name;

        Camera.main.rect = new Rect(0, 0, 1, 1);
        Camera.main.clearFlags = CameraClearFlags.SolidColor;

        if ("lobby".Equals(sceneName))
        {
            Camera.main.backgroundColor = new Color(85 / 255f, 74 / 255f, 166 / 255f);
        }
        else
        {
            Camera.main.backgroundColor = Color.black;
        }

        loadingImage.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        SetResolution(540, 860);
    }

    void SetCanvasScaler(int _width = 540, int _height = 860)
    {
        CanvasScaler canvasScaler = FindObjectOfType<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(_width, _height);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
    }
    void SetResolution(int width = 540, int height = 860)
    {

        SetCanvasScaler(width, height);

        int deviceWidth = Screen.width; // 기기의 해상도 너비
        int deviceHeight = Screen.height; // 기기의 해상도 높이

        Screen.SetResolution(width, (int)(((float)deviceHeight / deviceWidth) * width), true);
        // 해상도 변경

        if ((float)width / height < (float)deviceWidth / deviceHeight)
        {// 만약 기기의 해상도비가 더 크다면
            float newWidth = ((float)width / height) / ((float)deviceWidth / deviceHeight);
            Camera.main.rect = new Rect((1f - newWidth) / 2f, 0f, newWidth, 1f);
            // 메인카메라의 ViewPortRect값 조절
            // Rect : X, Y, W, H 값
        }
        else
        {// 게임화면의 해상도비가 더 크다면
            float newHeight = ((float)deviceWidth / deviceHeight) / ((float)width / height);
            Camera.main.rect = new Rect(0f, (1f - newHeight) / 2f, 1f, newHeight);

        }
        loadingImage.SetActive(false);
    }

}
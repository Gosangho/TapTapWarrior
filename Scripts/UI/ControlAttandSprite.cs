using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ControlAttandSprite : MonoBehaviour
{
    public Image currentImage;
    [SerializeField] Sprite toDayAttand;  // 오늘 출석 이미지
    [SerializeField] Sprite lateAttand; // 지각 날짜의 출석 이미지
    [SerializeField] Sprite notGetAttand; // 날짜가 표시되지 않은 출석 이미지
    public Button attandRewardCharacterButton;
    public Button attandRewardGoldButton;
    public Sprite attandRewardCharacterSprite;
    public Sprite attandRewardGoldSprite;

    void Start()
    {
        currentImage = GetComponent<Image>();

        int day = int.Parse(gameObject.name);

        if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == false)
            currentImage.sprite = lateAttand;
        else if (day < System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == true)
            currentImage.sprite = toDayAttand;
        else if (day == System.DateTime.Now.Day && AttandManager.AttandInstance.attandDay[day - 1] == true)
            currentImage.sprite = toDayAttand;
        else
            currentImage.sprite = notGetAttand;
    }

    public void UpdateSprite()
    {
        if (currentImage.sprite != toDayAttand)
        {
            currentImage.sprite = toDayAttand;
            DataManager.Instance.playerData.Gold += 10;
            AttandManager.AttandInstance.attandCount++;
            DataManager.Instance.SaveGameData();
            if (AttandManager.AttandInstance.attandCount >= 14)
            {
                attandRewardCharacterButton.image.sprite = attandRewardCharacterSprite;
                attandRewardGoldButton.image.sprite = attandRewardGoldSprite;
            }
            BackEndManager.Instance.DbSaveGameData();
        }
    }
}

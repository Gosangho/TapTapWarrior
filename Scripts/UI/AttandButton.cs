using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttandButton : MonoBehaviour
{
    [SerializeField]
    private ControlAttandSprite[] rewardSprite;
    [SerializeField]
    private GameObject rewards; // Contents
    [SerializeField]
    private Button[] rewardsButton;

    void Start()
    {
        rewardSprite = rewards.GetComponentsInChildren<ControlAttandSprite>();
        for (int i = 0; i < rewardsButton.Length; i++)
        {
            int day = i + 1; // 날짜는 1부터 시작
            rewardsButton[i].onClick.AddListener(() => OnDateButtonClick(day));
        }
    }

    public void OnDateButtonClick(int day)
    {
        //int day = int.Parse(gameObject.name);
        if (day == System.DateTime.Now.Day)
        {
            rewardSprite[System.DateTime.Now.Day - 1].UpdateSprite();
            AttandManager.AttandInstance.attandDay[System.DateTime.Now.Day - 1] = true;
            DataManager.Instance.playerData.PlayerAttandence[System.DateTime.Now.Day - 1] = true;
        }
    }
}

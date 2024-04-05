using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;

public class AttandManager : MonoBehaviour
{
    static protected AttandManager s_AttandInstance;
    static public AttandManager AttandInstance { get { return s_AttandInstance; } }

    
    public bool[] attandDay = new bool[31];
    public Button attandanceCharacterReward;
    public Button attandanceGoldReward;
    public Sprite onAttandanceCharacterRewardSprite;
    public Sprite onAttandanceGoldRewardSprite;
    public Sprite getAttandanceCharacterRewardSprite;
    public Sprite getAttandanceGoldRewardSprite;
    public int attandCount;
    public bool getattandanceCharacterReward;
    public bool getattandanceGoldReward;

    private void Awake()
    {
        s_AttandInstance = this;
        attandDay = DataManager.Instance.playerData.PlayerAttandence;

        if (DataManager.Instance.playerData.SwordGirl2Get == true)
        {
            getattandanceCharacterReward = true;
            getattandanceGoldReward = true;
        }
    }

    private void Start()
    {
        for (int i = 0; i < attandDay.Length; i++)
        {
            if (attandDay[i])
            {
                attandCount++;
            }
        }
        Debug.Log("True Count: " + attandCount);
        
        if (attandCount >= 14 && getattandanceCharacterReward == false && getattandanceGoldReward == false)
        {
            attandanceCharacterReward.image.sprite = onAttandanceCharacterRewardSprite;
            attandanceGoldReward.image.sprite = onAttandanceGoldRewardSprite;
        }
        else if (attandCount >= 14 && getattandanceCharacterReward == true && getattandanceGoldReward == true)
        {
            attandanceCharacterReward.image.sprite = getAttandanceCharacterRewardSprite;
            attandanceGoldReward.image.sprite = getAttandanceGoldRewardSprite;
        }
    }

    public void GetAttandCharacter()
    {
        if (attandCount >= 14 && getattandanceCharacterReward == false)
        {
            attandanceCharacterReward.image.sprite = getAttandanceCharacterRewardSprite;
            DataManager.Instance.playerData.SwordGirl2Get = true;
            DataManager.Instance.SaveGameData();
            Debug.Log("14일 출석 캐릭터 보상" + DataManager.Instance.playerData.SwordGirl2Get);
            getattandanceCharacterReward = true;
        }
    }

    public void GetAttandGold()
    {
        if (attandCount >= 14 && getattandanceGoldReward == false)
        {
            attandanceGoldReward.image.sprite = getAttandanceGoldRewardSprite;
            DataManager.Instance.playerData.Gold += 100;
            DataManager.Instance.SaveGameData();
            Debug.Log("14일 출석 골드 보상" + DataManager.Instance.playerData.Gold);
            getattandanceGoldReward = true;
        }
    }
}

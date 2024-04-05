using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnEnableObj : MonoBehaviour
{
    int goldTemp;
    TextMeshProUGUI goldText;
    public Button swordGirl2Button;
    public Sprite swordGirl2Sprite;

    private void Awake()
    {
        goldText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        goldTemp = DataManager.Instance.playerData.Gold;
    }

    private void OnEnable()
    {
        if (goldText != null && goldTemp != DataManager.Instance.playerData.Gold)
        {
            goldText.text = DataManager.Instance.playerData.Gold.ToString();
            goldTemp = DataManager.Instance.playerData.Gold;
        }

        if (DataManager.Instance.playerData.SwordGirl2Get)
        {
            swordGirl2Button.image.sprite = swordGirl2Sprite;
        }
    }
}

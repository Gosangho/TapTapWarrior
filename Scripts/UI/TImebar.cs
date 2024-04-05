using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


 public class TImebar : MonoBehaviour
 {
     public static Image timebarImage;
     public float TimebarImagefillAmount;
     public PlayerHeart PlayerHeart;
     public float StartTimeGauge = 0.5f; // 시작시 Timebar의 게이지는 50%시작
     [SerializeField] public float depletionRate = 0.1f; // 초당 Timebar의 게이지 10% 하락
     [SerializeField] public float depletionRateIncrease = 0.01f; // 몹 3마리 처치할때마다 타임 게이지 하락값 1%씩 상승
     public int KillCount = 0;
     void Start()
     {
         TimebarImagefillAmount = 0.5f;
         timebarImage = GetComponent<Image>();
         PlayerHeart = FindObjectOfType<PlayerHeart>();
         timebarImage.fillAmount = StartTimeGauge;
     }

     void Update()
     {
         TimebarImagefillAmount = timebarImage.fillAmount;
         
         timebarImage.fillAmount -= Time.deltaTime * depletionRate; // TimebarGauge 1초당 10%씩 하락

         if (KillCount % 3 == 0 && KillCount > 0)
         {// 몹을 3마리 처리할때마다
             depletionRate += depletionRateIncrease; // 타임바의 게이지 하락속도 1%씩 상승
             KillCount = 0;
         }
     }
 }






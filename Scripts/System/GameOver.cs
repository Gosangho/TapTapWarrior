using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


 public class GameOver : MonoBehaviour
 {
     public Button continue_Button;
     public GameObject newRecordObj;
     ScoreSystem scoreSystemInstance;
     Test3Battle test3BattleInstance;
     DataManager dataManagerInstance;
     public TextMeshProUGUI resultScore;
     public TextMeshProUGUI resultBestScore;

     private void OnEnable()
     {
         int bestScore = 0;
         string characterName = "";
         test3BattleInstance = FindObjectOfType<Test3Battle>();
         scoreSystemInstance = FindObjectOfType<ScoreSystem>();
         dataManagerInstance = FindObjectOfType<DataManager>();

         resultScore.gameObject.SetActive(true);
         resultBestScore.gameObject.SetActive(true);

         resultScore.text = "Score : " + scoreSystemInstance.score.ToString();

         if (SelectCharacter.swordGirl1)
         {
             
             if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl1PreviousBestScore < scoreSystemInstance.score)
             {
                 newRecordObj.gameObject.SetActive(true);
                 resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
             }
             else
             {
                 newRecordObj.gameObject.SetActive(false);
             }
             DataManager.Instance.swordGirl1.characterName = "Lana";
             characterName  = DataManager.Instance.swordGirl1.characterName;
             BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl1);
         }
         else if (SelectCharacter.swordGirl2)
         {
             if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl2PreviousBestScore < scoreSystemInstance.score)
             {
                 newRecordObj.gameObject.SetActive(true);
                 resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
             }
             else
             {
                 newRecordObj.gameObject.SetActive(false);
             }
             DataManager.Instance.swordGirl2.characterName = "Sia";
             characterName  = DataManager.Instance.swordGirl2.characterName;
             BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl2);
      

         }
         else if (SelectCharacter.swordGirl3)
         {
             if (test3BattleInstance.playerDie == true && scoreSystemInstance.swordGirl3PreviousBestScore < scoreSystemInstance.score)
             {
                 newRecordObj.gameObject.SetActive(true);
                 resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
             }
             else
             {
                 newRecordObj.gameObject.SetActive(false);
             }
             DataManager.Instance.swordGirl3.characterName = "Zena";
             characterName  = DataManager.Instance.swordGirl3.characterName;
             BackEndManager.Instance.SaveBestScore(DataManager.Instance.swordGirl3);
         }
         else
         {
             
             if (test3BattleInstance.playerDie == true && scoreSystemInstance.leonPreviousBestScore < scoreSystemInstance.score)
             {
                 newRecordObj.gameObject.SetActive(true);
                 resultBestScore.text = "Best Score : " + scoreSystemInstance.score;
             }
             else
             {
                 newRecordObj.gameObject.SetActive(false);
             }
             DataManager.Instance.leon.characterName = "leon";
             characterName  = DataManager.Instance.leon.characterName;
             BackEndManager.Instance.SaveBestScore(DataManager.Instance.leon);
         }

         // 모든 캐릭터의 최대 스코어를 갱신 했는지 확인 
         // user 테이블에 최고 점수를 저장하는 컬럼을 추가 예정
         if(bestScore < scoreSystemInstance.swordGirl1PreviousBestScore)
         {
             bestScore = scoreSystemInstance.score;
         }
         else if(bestScore < scoreSystemInstance.swordGirl2PreviousBestScore)
         {
             bestScore = scoreSystemInstance.score;
         }
         else if(bestScore < scoreSystemInstance.swordGirl3PreviousBestScore)
         {
             bestScore = scoreSystemInstance.score;
         }
         else if(bestScore < scoreSystemInstance.leonPreviousBestScore)
         {
             bestScore = scoreSystemInstance.score;
         }
         

         if(bestScore <= scoreSystemInstance.score ) {
             BackEndManager.Instance.RankInputdate(scoreSystemInstance.score, characterName);
         }
         
     }
 }



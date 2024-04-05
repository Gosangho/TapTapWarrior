using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


 public class Right_Orc2_material : MonoBehaviour
 {
     public Material objectMaterial;
     public float Right_brightnessValue; // 개별 객체의 명암값
     Test3Battle Test3BattleInstance;
     TutorialBattleSystem TutorialBattleSystem;
     int currentSceneIndex;
     private void Start()
     {
         // 새로운 Material 인스턴스를 생성하여 현재 객체의 Material을 설정합니다.
         Scene currentScene = SceneManager.GetActiveScene();
         currentSceneIndex = currentScene.buildIndex;
         Test3BattleInstance = FindObjectOfType<Test3Battle>();
         TutorialBattleSystem = FindObjectOfType<TutorialBattleSystem>();
         GetComponent<Renderer>().material = objectMaterial;
     }
     private void Update()
     {
         if (currentSceneIndex == 3)
         {
             if (Test3BattleInstance.selectEnemy == Test3BattleInstance.RightEnemy)
             {
                 Right_brightnessValue = 1.0f;
             }
             else
             {
                 Right_brightnessValue = 0.2f;
             }
         }
         else if (currentSceneIndex == 5)
         {
             if (TutorialBattleSystem.selectEnemy == TutorialBattleSystem.RightEnemy)
             {
                 Right_brightnessValue = 1.0f;
             }
             else
             {
                 Right_brightnessValue = 0.2f;
             }
         }
         
         objectMaterial.SetFloat("_Brightness", Right_brightnessValue);

     }
 }






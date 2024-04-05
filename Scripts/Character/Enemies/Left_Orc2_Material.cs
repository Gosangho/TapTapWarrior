using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


 public class Left_Orc2_Material : MonoBehaviour
 {
     public Material newMaterial;
     public float Left_brightnessValue1; // 개별 객체의 명암값
     Test3Battle Test3BattleInstance1;
     TutorialBattleSystem TutorialBattleSystem;
     int currentSceneIndex;
     private void Start()
     {
         Scene currentScene = SceneManager.GetActiveScene();
         currentSceneIndex = currentScene.buildIndex;
         Test3BattleInstance1 = FindObjectOfType<Test3Battle>();
         TutorialBattleSystem = FindObjectOfType<TutorialBattleSystem>();
         StartCoroutine(ChangeMaterialAfterOneFrame());
     }

     private IEnumerator ChangeMaterialAfterOneFrame()
     {
         yield return null;

         // Material 변경 코드
         GetComponent<Renderer>().material = newMaterial;
         newMaterial.SetFloat("_Brightness", Left_brightnessValue1);
     }

     private void Update()
     {
         if (currentSceneIndex == 3)
         {
             if (Test3BattleInstance1.selectEnemy == Test3BattleInstance1.LeftEnemy)
             {
                 Left_brightnessValue1 = 1.0f;
             }
             else
             {
                 Left_brightnessValue1 = 0.2f;
             }
         }
         else if (currentSceneIndex == 5)
         {
             if (TutorialBattleSystem.selectEnemy == TutorialBattleSystem.LeftEnemy)
             {
                 Left_brightnessValue1 = 1.0f;
             }
             else
             {
                 Left_brightnessValue1 = 0.2f;
             }
         }
         newMaterial.SetFloat("_Brightness", Left_brightnessValue1);
     }
     // Material이 게임 시작전에는 변경이 안돼는 문제 -> start와 awake등의 문제인듯함.
     // 게임이 runtime중에는 변경이 가능함
     // 해결해야함
     // 이를 이용해서 임시로 runtime중에 한 프레임 대기후에 mesh renderer의 material을 복제한 material로 변경
     // -> 이를통해 각 객체가 각각의 명암값을 가질 수 있다.
     // but 게임 시작전에 변경해서 하는 방법을 해결해보자.
 }







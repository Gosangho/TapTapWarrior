using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using TMPro;



 public class Test3Battle : GameSystem
 {
     public EnemyBase selectEnemy;
     public EnemyBase LeftEnemy;
     public EnemyBase RightEnemy;
     public CharacterBase CharacterBaseInstance;
     public List<EnemyBase> SceneEnemyList = new List<EnemyBase>();
     public ENodeType ClickNode;
     public Button AttackButton;
     public Button SelectEnemyButton;
     public PlayerBase PlayerBaseInstance;
     public GameObject Left_Ork;
     public GameObject Right_Ork;
     public bool Right_MonsterDie; 
     public bool Left_MonsterDie;
     public Node NodeInstance;
     public GameObject swordGirl1_CriticalEffect;
     public GameObject swordGirl2_CriticalEffect;
     public GameObject swordGirl3_CriticalEffect;
     public GameObject leon_CriticalEffect;
     public bool FirstAttack; 
     public bool Right_TrainAttack; 
     public bool Left_TrainAttack; 
     public ScoreSystem ScoreSystemInstance;
     public GameObject ScoreTextObj;
     public TImebar TimebarInstance;
     public bool FirstDashAttack;
     public GameObject swordGirl1_AfterImage; 
     public GameObject swordGirl2_AfterImage;
     public GameObject swordGirl3_AfterImage;
     public GameObject leon_AfterImage;
     public GameObject swordGirl1_FirstAttack;
     public GameObject swordGirl2_FirstAttack;
     public GameObject swordGirl3_FirstAttack;
     public GameObject leon_FirstAttack;
     public GameObject swordGirl1;
     public GameObject swordGirl2;
     public GameObject swordGirl3;
     public GameObject leon;
     public GameObject reStartObj;
     public SelectCharacter selectCharacterInstance;
     public bool playerDie = false;
     public GameObject resultObj;
     public Button continue_Button;
     public bool repetition;
     void Start()
     {
         selectCharacterInstance = FindObjectOfType<SelectCharacter>();
         initGetCharacter();

         initGetButton();

         ClickNode = ENodeType.Default;

         GameObject character2Object = GameObject.FindGameObjectWithTag("Player");
         if (character2Object != null)
         {
             PlayerBaseInstance = character2Object.GetComponent<PlayerBase>();
         }
         
         AttackButton.onClick.AddListener(Attack);
         SelectEnemyButton.onClick.AddListener(SelectEnemy);
         StartSpawn();
         Right_MonsterDie = false;
         Left_MonsterDie = false;
         Right_TrainAttack = false;
         Left_TrainAttack = false;
         FirstAttack = true;
         FirstDashAttack = true;
         repetition = false;
         NodeInstance = FindObjectOfType<Node>();
         ScoreSystemInstance = FindObjectOfType<ScoreSystem>();
         TimebarInstance = FindObjectOfType<TImebar>();
         EffectInit();
         ContinueButton.continueButtonClick = false;
     }

     private void initGetCharacter() {
         string getCharacterName = DataManager.Instance.getCharacter(0);

         if("Leon".Equals(getCharacterName))
         {
             SelectCharacter.leon = true;
         }
         else if("Sword2".Equals(getCharacterName))
         {
             SelectCharacter.swordGirl2 = true;
         }
         else if("Sword3".Equals(getCharacterName))
         {
             SelectCharacter.swordGirl3 = true;
         }
         else
         {
             SelectCharacter.swordGirl1 = true;
         }

         if (SelectCharacter.leon)
         {
             leon.SetActive(true);
         }
         else if (SelectCharacter.swordGirl2)
         {
             swordGirl2.SetActive(true);
         }
         else if (SelectCharacter.swordGirl3)
         {
             swordGirl3.SetActive(true);
         }
         else
         { 
             swordGirl1.SetActive(true);
             SelectCharacter.swordGirl1 = true;
         }
     }

     private void initGetButton() {
         string controlType = DataManager.Instance.getCharacter(1);

         Transform AttackButtonTrans;
         Transform DashButtonTrans;

         if( AttackButton != null || SelectEnemyButton != null)
         {
             AttackButtonTrans = AttackButton.transform;
             DashButtonTrans = SelectEnemyButton.transform;

             if("B".Equals(controlType))
             {
                 Vector3 tempPosition = AttackButtonTrans.position;
                 AttackButtonTrans.position = DashButtonTrans.position;
                 DashButtonTrans.position = tempPosition;
             } else {
                 Vector3 tempPosition = DashButtonTrans.position;
                 AttackButtonTrans.position = AttackButtonTrans.position;
                 DashButtonTrans.position = tempPosition;
             }
         }
     }

     public override void OnSystemInit()
     {
         GameManager.NotificationSystem.SceneMonsterSpawned.AddListener(HandleSceneMonsterSpawned);
     }

     private void HandleSceneMonsterSpawned(EnemyBase spawnedEnemy)
     {

         if (selectEnemy == null)
             selectEnemy = spawnedEnemy;

         SceneEnemyList.Add(spawnedEnemy);
     }

     void Update()
     {
         if (ClickNode != ENodeType.Default)
         {// ClickNode가 Default라면

             if (selectEnemy == null) { return; }

             if (ClickNode == selectEnemy.GetOwnNodes().Peek().nodeSheet.m_NodeType)
             {// (해당 NodeType의 버튼인 경우)

                 GameManager.NotificationSystem.NodeHitSuccess?.Invoke();
                 ScoreSystemInstance.score += 1; 

                 if (SelectCharacter.swordGirl1)
                 {
                     if (ScoreSystem.swordGirl1BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl1BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else if (SelectCharacter.swordGirl2)
                 {
                     if (ScoreSystem.swordGirl2BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl2BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else if (SelectCharacter.swordGirl3)
                 {
                     if (ScoreSystem.swordGirl3BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl3BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else
                 {
                     if (ScoreSystem.leonBestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.leonBestScore = ScoreSystemInstance.score;
                     }
                 }

                 TImebar.timebarImage.fillAmount += 0.1f;

                 PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                 , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f);

                 Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; // 위치 설정
                 GameObject gameObject = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); // 위치에 텍스트 생성

                 if (Right_TrainAttack == true || Left_TrainAttack == true)
                 {
                     // 오른쪽 몬스터가 사망한 상태에서 또 오른쪽 몬스터를 타격할 경우
                     playerDie = true;
                     PlayerBase.PlayerAnim.SetTrigger("Die");
                     resultObj.gameObject.SetActive(true);
                     continue_Button.gameObject.SetActive(true);
                     reStartObj.gameObject.SetActive(true);

                     if (ContinueButton.continueButtonClick == true)
                     {
                         continue_Button.gameObject.SetActive(false);
                     }
                     else
                     {
                         continue_Button.gameObject.SetActive(true);
                     }
                 }

                 RandAnim();

                 if (FirstAttack)
                 {// 첫 공격시에만 활용되는 함수
                     if (SelectCharacter.swordGirl1)
                     {
                         GameObject swordGirl1FirstAttack =
                         Instantiate(swordGirl1_FirstAttack, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                     }
                     else if (SelectCharacter.swordGirl2)
                     {
                         GameObject swordGirl2FirstAttack =
                         Instantiate(swordGirl2_FirstAttack, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                     }
                     else if (SelectCharacter.swordGirl3)
                     {
                         GameObject swordGirl3FirstAttack =
                         Instantiate(swordGirl3_FirstAttack, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                     }
                     else
                     {
                         GameObject leonFirstAttack =
                         Instantiate(leon_FirstAttack, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                     }


                     FirstAttack = false;
                     float RandAttackSound = Random.value; // 0~1사이의 값
                     if (RandAttackSound < 0.4f)
                     {
                         audioManager.Instance.SfxAudioPlay("Char_Attack1");
                     }
                     else if (RandAttackSound < 0.8f)
                     {
                         audioManager.Instance.SfxAudioPlay("Char_Attack2");
                     }
                     else
                     {
                         audioManager.Instance.SfxAudioPlay("Char_Spirit");
                     }
                 }
                 RandAttackAudio();
                 RandEnemyHitAudio();
                 if (selectEnemy == RightEnemy)
                 {
                     Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage");
                 }
                 else
                 {
                     Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage");
                 }

                 Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().gameObject.transform.position;
                 Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                 selectEnemy.GetOwnNodes().Dequeue();
                 selectEnemy.Hit();

                 if (selectEnemy.GetOwnNodes().Count <= 0)
                 {

                     float rand = Random.Range(0f, 1f);
                     if (rand <= 0.05f)
                     {// 5% 확률 (1골드 획득)
                         DataManager.Instance.playerData.Gold += 1;
                     }

                     else if (rand <= 0.0005f)
                     {// 0.05% 확률 (10골드 획득)
                         DataManager.Instance.playerData.Gold += 10;
                     }

                     BackEndManager.Instance.DbSaveGameData();

                     selectEnemy.Die();
                     audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                     TimebarInstance.KillCount += 1;

                     if (SelectCharacter.swordGirl1)
                     {
                         DataManager.Instance.swordGirl1.totalKillScore++;
                     }
                     else if (SelectCharacter.swordGirl2)
                     {
                         DataManager.Instance.swordGirl2.totalKillScore++;
                     }
                     else if (SelectCharacter.swordGirl3)
                     {
                         DataManager.Instance.swordGirl3.totalKillScore++;
                     }
                     else
                     {
                         DataManager.Instance.leon.totalKillScore++;
                     }

                     if (Right_MonsterDie)
                     {
                         RightMonsterSpawn();

                         Right_MonsterDie = false;
                     }
                     else
                     {
                         LeftMonsterSpawn();

                         Left_MonsterDie = false;
                     }
                 }
             }
             else
             {
                 GameManager.NotificationSystem.NodeHitFail?.Invoke();

                 if (selectEnemy != null)
                 {
                     selectEnemy.Attack();
                 }

                 if (selectEnemy == RightEnemy)
                 {
                     Test3Spawn.Instance.Spawn_RightNode(selectEnemy);
                 }
                 else if (selectEnemy == LeftEnemy)
                 {
                     Test3Spawn.Instance.SpawnLeft_Node(selectEnemy);
                 }
             }

             ClickNode = ENodeType.Default; // reset
             Debug.Log(ClickNode);
         }

         if (TImebar.timebarImage.fillAmount <=0 && repetition == false)
         {// timeOver
             repetition = true;
             Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Attack");
             Right_Orc2_Anim.RightAnim.SetTrigger("Right_Attack");
             playerDie = true;
             PlayerBase.PlayerAnim.SetTrigger("Die");
             resultObj.gameObject.SetActive(true);
             reStartObj.gameObject.SetActive(true);

             if (ContinueButton.continueButtonClick == true)
             {
                 continue_Button.gameObject.SetActive(false);
             }
             else
             {
                 continue_Button.gameObject.SetActive(true);
             }
         }
     }

     void Attack()
     {
         ClickNode = ENodeType.Attack;
         if (playerDie == true)
         {
             ClickNode = ENodeType.Default;
         }
     }

     void SelectEnemy()
     {
         if (selectEnemy == RightEnemy && playerDie == false)
         {// 오른쪽 몬스터가 사망했고 selectEnemy가 RightEnemy인경우
             if (LeftEnemy.GetOwnNodes().Count == Test3Spawn.Instance.LeftAttackNum)
             {

                 if (FirstDashAttack || FirstAttack)
                 {
                     playerDie = true;
                     PlayerBase.PlayerAnim.SetTrigger("Die");
                     resultObj.gameObject.SetActive(true);
                     continue_Button.gameObject.SetActive(true);
                     reStartObj.gameObject.SetActive(true);

                     if (ContinueButton.continueButtonClick == true)
                     {
                         continue_Button.gameObject.SetActive(false);
                     }
                     else
                     {
                         continue_Button.gameObject.SetActive(true);
                     }
                 }

                 ScoreSystemInstance.score += 1;

                 if (SelectCharacter.swordGirl1)
                 {
                     if (ScoreSystem.swordGirl1BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl1BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else if (SelectCharacter.swordGirl2)
                 {
                     if (ScoreSystem.swordGirl2BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl2BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else if (SelectCharacter.swordGirl3)
                 {
                     if (ScoreSystem.swordGirl3BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl3BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else
                 {
                     if (ScoreSystem.leonBestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.leonBestScore = ScoreSystemInstance.score;
                     }
                 }

                 RandDashAttackAudio();
                 RandEnemyHitAudio();
                 Right_TrainAttack = false;

                 selectEnemy = LeftEnemy;

                 TImebar.timebarImage.fillAmount += 0.1f;

                 PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                 , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); 
                 
                 Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position; 
                 GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity);


                 if (SelectCharacter.swordGirl1)
                 {
                     swordGirl1_AfterImage.transform.localScale = new Vector3(-1.0f, swordGirl1_AfterImage.transform.localScale.y, swordGirl1_AfterImage.transform.localScale.z);
                     Instantiate(swordGirl1_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }
                 else if (SelectCharacter.swordGirl2)
                 {
                     swordGirl2_AfterImage.transform.localScale = new Vector3(-1.0f, swordGirl2_AfterImage.transform.localScale.y, swordGirl2_AfterImage.transform.localScale.z);
                     Instantiate(swordGirl2_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }
                 else if (SelectCharacter.swordGirl3)
                 {
                     swordGirl3_AfterImage.transform.localScale = new Vector3(-1.0f, swordGirl3_AfterImage.transform.localScale.y, swordGirl3_AfterImage.transform.localScale.z);
                     Instantiate(swordGirl3_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }
                 else
                 {
                     leon_AfterImage.transform.localScale = new Vector3(-1.0f, leon_AfterImage.transform.localScale.y, leon_AfterImage.transform.localScale.z);
                     Instantiate(leon_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }

                 Left_Orc2_Anim.LeftAnim.SetTrigger("Left_Damage");


                 Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                 selectEnemy.GetOwnNodes().Dequeue();

                 selectEnemy.Hit();

                 PlayerBase.PlayerTransform.localScale =
                 new Vector3(-1f, PlayerBase.PlayerTransform.localScale.y, PlayerBase.PlayerTransform.localScale.z);

                 FirstDashAttack = true;
                 
                 if (selectEnemy.GetOwnNodes().Count <= 0)
                 {

                     float rand = Random.Range(0f, 1f);
                     if (rand <= 0.05f)
                     {
                         DataManager.Instance.playerData.Gold += 1;
                     }

                     else if (rand <= 0.0005f)
                     {
                         DataManager.Instance.playerData.Gold += 10;
                     }

                     BackEndManager.Instance.DbSaveGameData();

                     FirstDashAttack = false;
                     selectEnemy.Die();
                     audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");

                     if (SelectCharacter.swordGirl1)
                     {
                         DataManager.Instance.swordGirl1.totalKillScore++;
                     }
                     else if (SelectCharacter.swordGirl2)
                     {
                         DataManager.Instance.swordGirl2.totalKillScore++;
                     }
                     else if (SelectCharacter.swordGirl3)
                     {
                         DataManager.Instance.swordGirl3.totalKillScore++;
                     }
                     else
                     {
                         DataManager.Instance.leon.totalKillScore++;
                     }

                     TimebarInstance.KillCount += 1;

                     if (Right_MonsterDie)
                     {
                         RightMonsterSpawn();

                         Right_MonsterDie = false;
                     }
                     else
                     {
                         LeftMonsterSpawn();
                     }
                 }
             }
         }
         else if (selectEnemy == LeftEnemy && playerDie == false)
         {
             if (RightEnemy.GetOwnNodes().Count == Test3Spawn.Instance.RightAttackNum)
             {

                 if (FirstDashAttack || FirstAttack)
                 {
                     playerDie = true;
                     PlayerBase.PlayerAnim.SetTrigger("Die");
                     resultObj.gameObject.SetActive(true);
                     continue_Button.gameObject.SetActive(true);
                     reStartObj.gameObject.SetActive(true);

                     if (ContinueButton.continueButtonClick == true)
                     {
                         continue_Button.gameObject.SetActive(false);
                     }
                     else
                     {
                         continue_Button.gameObject.SetActive(true);
                     }
                 }

                 ScoreSystemInstance.score += 1;

                 if (SelectCharacter.swordGirl1)
                 {
                     if (ScoreSystem.swordGirl1BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl1BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else if (SelectCharacter.swordGirl2)
                 {
                     if (ScoreSystem.swordGirl2BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl2BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else if (SelectCharacter.swordGirl3)
                 {
                     if (ScoreSystem.swordGirl3BestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.swordGirl3BestScore = ScoreSystemInstance.score;
                     }
                 }
                 else
                 {
                     if (ScoreSystem.leonBestScore <= ScoreSystemInstance.score)
                     {
                         ScoreSystem.leonBestScore = ScoreSystemInstance.score;
                     }
                 }

                 RandDashAttackAudio();
                 RandEnemyHitAudio();
                 Left_TrainAttack = false;

                 selectEnemy = RightEnemy;

                 TImebar.timebarImage.fillAmount += 0.1f; 

                 PlayerBaseInstance.gameObject.transform.position = new Vector3(selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.x
                 , selectEnemy.GetOwnNodes().Peek().gameObject.transform.position.y, 0.0f); 

                 Vector3 scorePosition = selectEnemy.GetOwnNodes().Peek().transform.position;
                 GameObject ScoreTextobj = Instantiate(ScoreTextObj, scorePosition, Quaternion.identity); 


                 if (SelectCharacter.swordGirl1)
                 {
                     swordGirl1_AfterImage.transform.localScale = new Vector3(1.0f, swordGirl1_AfterImage.transform.localScale.y, swordGirl1_AfterImage.transform.localScale.z);
                     Instantiate(swordGirl1_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }
                 else if (SelectCharacter.swordGirl2)
                 {
                     swordGirl2_AfterImage.transform.localScale = new Vector3(1.0f, swordGirl2_AfterImage.transform.localScale.y, swordGirl2_AfterImage.transform.localScale.z);
                     Instantiate(swordGirl2_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }
                 else if (SelectCharacter.swordGirl3)
                 {
                     swordGirl3_AfterImage.transform.localScale = new Vector3(1.0f, swordGirl3_AfterImage.transform.localScale.y, swordGirl3_AfterImage.transform.localScale.z);
                     Instantiate(swordGirl3_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }
                 else
                 {
                     leon_AfterImage.transform.localScale = new Vector3(1.0f, leon_AfterImage.transform.localScale.y, leon_AfterImage.transform.localScale.z);
                     Instantiate(leon_AfterImage, PlayerBaseInstance.gameObject.transform.position, Quaternion.identity);
                 }

                 Right_Orc2_Anim.RightAnim.SetTrigger("Right_Damage"); 


                 Destroy(selectEnemy.GetOwnNodes().Peek().gameObject);

                 selectEnemy.GetOwnNodes().Dequeue();

                 selectEnemy.Hit();

                 PlayerBase.PlayerTransform.localScale =
                 new Vector3(1f, PlayerBase.PlayerTransform.localScale.y, PlayerBase.PlayerTransform.localScale.z);

                 FirstDashAttack = true;
                 
                 if (selectEnemy.GetOwnNodes().Count <= 0)
                 {
                     float rand = Random.Range(0f, 1f);
                     if (rand <= 0.05f)
                     {
                         DataManager.Instance.playerData.Gold += 1;
                     }

                     else if (rand <= 0.0005f)
                     {
                         DataManager.Instance.playerData.Gold += 10;
                     }

                     BackEndManager.Instance.DbSaveGameData();

                     FirstDashAttack = false;
                     selectEnemy.Die();
                     audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
                     TimebarInstance.KillCount += 1;

                     if (SelectCharacter.swordGirl1)
                     {
                         DataManager.Instance.swordGirl1.totalKillScore++;
                     }
                     else if (SelectCharacter.swordGirl2)
                     {
                         DataManager.Instance.swordGirl2.totalKillScore++;
                     }
                     else if (SelectCharacter.swordGirl3)
                     {
                         DataManager.Instance.swordGirl3.totalKillScore++;
                     }
                     else
                     {
                         DataManager.Instance.leon.totalKillScore++;
                     }

                     if (Right_MonsterDie)
                     {
                         RightMonsterSpawn();
                         Right_MonsterDie = false;
                     }
                     else
                     {
                         LeftMonsterSpawn();
                     }
                 }
             }
         }
     }

     public void RandAttackAudio()
     {
         float RandAttackSound = Random.value;
         if (RandAttackSound < 0.4f)
         {
             audioManager.Instance.SfxAudioPlay("Char_Attack1");
         }
         else if (RandAttackSound < 0.8f)
         {
             audioManager.Instance.SfxAudioPlay("Char_Attack2");
         }
         else
         {
             RandCriticalEffect();
             audioManager.Instance.SfxAudioPlay("Char_Spirit");
         }
     }
     public void RandDashAttackAudio()
     {
         int Ran = Random.Range(0, 2);
         if (Ran == 0)
         {
             audioManager.Instance.SfxAudioPlay("Char_Dash1");
         }
         else
         {
             audioManager.Instance.SfxAudioPlay("Char_Dash2");
         }
     }
     public void RandEnemyHitAudio()
     {
         int Ran = Random.Range(0, 2);
         if (Ran == 0)
         {
             audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Hit");
         }
         else
         {
             audioManager.Instance.SfxAudioPlay_Enemy("Enemy_Dead");
         }
     }
     public void RandAnim()
     {
         int randAnim = Random.Range(0, 6);
         if (FirstAttack==false)
         {
             if (randAnim == 0)
             {
                 PlayerBase.PlayerAnim.SetTrigger("Atk_1"); 
             }
             else if (randAnim == 1)
             {
                 PlayerBase.PlayerAnim.SetTrigger("Atk_2"); 
             }
             else if (randAnim == 2)
             {
                 PlayerBase.PlayerAnim.SetTrigger("Atk_3"); 
             }
             else if (randAnim == 3)
             {
                 PlayerBase.PlayerAnim.SetTrigger("Atk_4");
             }
             else if (randAnim == 4)
             {
                 PlayerBase.PlayerAnim.SetTrigger("Atk_5");
             }
             else
             {// 5
                 PlayerBase.PlayerAnim.SetTrigger("Atk_7"); 
             }
 
         }

     }

     void RandCriticalEffect()
     {

         Vector3 targetPosition = selectEnemy.GetOwnNodes().Peek().transform.position;

         if (selectEnemy == LeftEnemy)
         {
             swordGirl1_CriticalEffect.transform.localScale =
             new Vector3(-1.0f, swordGirl1_CriticalEffect.transform.localScale.y, swordGirl1_CriticalEffect.transform.localScale.z);

             swordGirl2_CriticalEffect.transform.localScale =
             new Vector3(-0.6f, 0.6f, swordGirl2_CriticalEffect.transform.localScale.z);

             swordGirl3_CriticalEffect.transform.localScale =
             new Vector3(-0.8f, 0.7f, swordGirl3_CriticalEffect.transform.localScale.z);

             leon_CriticalEffect.transform.localScale =
             new Vector3(-1.0f, leon_CriticalEffect.transform.localScale.y, leon_CriticalEffect.transform.localScale.z);
         }
         else
         {
             swordGirl1_CriticalEffect.transform.localScale =
             new Vector3(1.0f, swordGirl1_CriticalEffect.transform.localScale.y, swordGirl1_CriticalEffect.transform.localScale.z);

             swordGirl2_CriticalEffect.transform.localScale =
             new Vector3(0.6f, 0.6f, swordGirl2_CriticalEffect.transform.localScale.z);

             swordGirl3_CriticalEffect.transform.localScale =
             new Vector3(0.8f, 0.7f, swordGirl3_CriticalEffect.transform.localScale.z);

             leon_CriticalEffect.transform.localScale =
             new Vector3(1.0f, leon_CriticalEffect.transform.localScale.y, leon_CriticalEffect.transform.localScale.z);
         }
         
         
         if (SelectCharacter.swordGirl1)
         {
             Instantiate(swordGirl1_CriticalEffect, targetPosition, Quaternion.identity);
         }
         else if (SelectCharacter.swordGirl2)
         {
             Instantiate(swordGirl2_CriticalEffect, targetPosition, Quaternion.identity);
         }
         else if (SelectCharacter.swordGirl3)
         {
             Instantiate(swordGirl3_CriticalEffect, targetPosition, Quaternion.identity);
         }
         else if (SelectCharacter.leon)
         {
             Instantiate(leon_CriticalEffect, targetPosition, Quaternion.identity);
         }
     }
         
     public void StartSpawn()
     {
         GameObject RightMonster = Right_Ork;
         GameObject RightSpawnMonster = Instantiate(RightMonster, new Vector3(4.0f, 0.72f, 0), Quaternion.identity);
         EnemyBase spawnEnemy = RightSpawnMonster.GetComponent<EnemyBase>();
         if (spawnEnemy != null)
         {
             Test3Spawn.Instance.Spawn_RightNode(spawnEnemy);
         }
         RightEnemy = spawnEnemy; 
         selectEnemy = spawnEnemy;

         GameObject LefttMonster = Left_Ork;
         GameObject LeftSpawnMonster = Instantiate(LefttMonster, new Vector3(-4.0f, 0.72f, 0), Quaternion.identity);
         EnemyBase spawnEnemy2 = LeftSpawnMonster.GetComponent<EnemyBase>();
         if (spawnEnemy2 != null)
         {
             Test3Spawn.Instance.SpawnLeft_Node(spawnEnemy2);
         }
         LeftEnemy = spawnEnemy2; 
     }
     public void RightMonsterSpawn()
     {
         GameObject RightMonster = Right_Ork;
         GameObject RightSpawnMonster = Instantiate(RightMonster, new Vector3(4.0f, 0.72f, 0), Quaternion.identity);
         EnemyBase spawnEnemy = RightSpawnMonster.GetComponent<EnemyBase>();
         if (spawnEnemy != null)
         {
             Test3Spawn.Instance.Spawn_RightNode(spawnEnemy);
         }
         selectEnemy = spawnEnemy;
         RightEnemy = spawnEnemy;
     }
     public void LeftMonsterSpawn()
     {
         GameObject LefttMonster = Left_Ork;
         GameObject LeftSpawnMonster = Instantiate(LefttMonster, new Vector3(-4.0f, 0.72f, 0), Quaternion.identity);
         EnemyBase spawnEnemy2 = LeftSpawnMonster.GetComponent<EnemyBase>();
         if (spawnEnemy2 != null)
         {
             Test3Spawn.Instance.SpawnLeft_Node(spawnEnemy2);
         }
         selectEnemy = spawnEnemy2;
         LeftEnemy = spawnEnemy2;
     }

     void EffectInit()
     {
         swordGirl1_CriticalEffect.transform.localScale =
         new Vector3(1.0f, swordGirl1_CriticalEffect.transform.localScale.y, swordGirl1_CriticalEffect.transform.localScale.z);

         swordGirl2_CriticalEffect.transform.localScale =
         new Vector3(0.6f, 0.6f, swordGirl2_CriticalEffect.transform.localScale.z);

         swordGirl3_CriticalEffect.transform.localScale =
         new Vector3(0.8f, 0.7f, swordGirl3_CriticalEffect.transform.localScale.z);

         leon_CriticalEffect.transform.localScale =
         new Vector3(1.0f, leon_CriticalEffect.transform.localScale.y, leon_CriticalEffect.transform.localScale.z);

         swordGirl1_AfterImage.transform.localScale =
         new Vector3(1.0f, swordGirl1_AfterImage.transform.localScale.y, swordGirl1_AfterImage.transform.localScale.z);

         swordGirl2_AfterImage.transform.localScale =
         new Vector3(1.0f, swordGirl2_AfterImage.transform.localScale.y, swordGirl2_AfterImage.transform.localScale.z);

         swordGirl3_AfterImage.transform.localScale =
         new Vector3(1.0f, swordGirl3_AfterImage.transform.localScale.y, swordGirl3_AfterImage.transform.localScale.z);

         leon_AfterImage.transform.localScale =
         new Vector3(1.0f, leon_AfterImage.transform.localScale.y, leon_AfterImage.transform.localScale.z);
     }
 }




using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


 public class Test3Spawn : MonoBehaviour
 {
     // 인스턴스에 대한 정적 참조
     public static Test3Spawn Instance { get; private set; }

     [Header("Spawn Setting")]
     [SerializeField] bool IsSpawnAlignmentRandom = false;
     [SerializeField] public GameObject m_SpawnLocation_Right;
     [SerializeField] public GameObject m_SpawnLocation_Left;
     [SerializeField] public List<GameObject> m_NodeList = new List<GameObject>(); // 몬스터에 부착될 노드리스트
     [SerializeField] private List<GameObject> m_monsterPrefabList; // 생성될 몬스터 리스트
     public Test3Battle BattleInstance;
     public GameObject AttackNode;
     public Vector2 SpawnNodePosition;
     public int RightAttackNum;
     public int LeftAttackNum;
     int RightRow;
     int LeftRow;

     private void Awake()
     {
         // NodeSpawnManager 인스턴스가 하나만 있는지 확인하십시오.
         if (Instance == null)
         {
             Instance = this;
             //DontDestroyOnLoad(gameObject); // 선택 사항: 이 관리자가 장면 로드 간에 지속되도록 하려면
         }
         else
         {
             Destroy(gameObject);
         }
     }

     private void Start()
     {
         GameManager.NotificationSystem.SceneMonsterDeath.AddListener(HandleSceneMonsterDeath);
         BattleInstance = FindObjectOfType<Test3Battle>();
         // SpawnLocation 자식 개체 찾기
         m_SpawnLocation_Right = transform.Find("SpawnLocation_Right").gameObject;
         m_SpawnLocation_Left = transform.Find("SpawnLocation_Left").gameObject;
        
     }

     private void HandleSceneMonsterDeath(EnemyBase arg0)
     {
         float delayTime = 1.0f;
         StartCoroutine(DelaySpawn(delayTime));
     }

     private IEnumerator DelaySpawn(float delayTime)
     {
         yield return new WaitForSeconds(delayTime);

     }

     public void Spawn_RightNode(EnemyBase enemyBase)
     {
         //노드가 0개가 아니라면 생성 된 노드를 삭제하고 Queue를 비웁니다.
         if (enemyBase.GetOwnNodes().Count != 0)
         {
             foreach (Node ownNode in enemyBase.GetOwnNodes())
             {
                 Destroy(ownNode.gameObject);
             }
             enemyBase.GetOwnNodes().Clear();
         }

         NodeArea nodeArea = enemyBase.nodeArea;

         // NodeArea가 발견되었는지 확인하십시오.
         if (nodeArea == null)
         {
             Debug.LogError("적 또는 적의 자식에서 NodeArea를 찾을 수 없습니다.");
             return;
         }

         // 스폰할 노드 수는 행 수와 같습니다.
         int spawnNodeNum = nodeArea.Rows;
         // 7행 3열의 구조 -> 각 행에서 3열의 공간에 공격포인트를 생성할지 안할지 확률로 생성
         for (RightRow = 0; RightRow < spawnNodeNum; RightRow++)
         {
             int ranAttackNode = Random.Range(0, 2); // 50퍼센트 확률
             if (ranAttackNode == 0)
             {
                 // 이 행에서 임의로 열을 선택합니다.
                 int randColumn = UnityEngine.Random.Range(0, nodeArea.Columns);

                 // 목록에서 열에 해당하는 노드 프리팹을 가져옵니다.
                 GameObject nodePrefab = GetNodePrefab(randColumn);

                 // NodeArea에서 이 노드의 위치를 가져옵니다.
                 Vector2 spawnPosition = nodeArea.GetGridPosition(RightRow, randColumn);

                 // 계산된 위치에 노드를 인스턴스화하고 nodeArea의 자식으로 설정합니다.
                 GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                 // 노드의 이름을 설정합니다.
                 spawnedNode.name = $"Node_{RightRow}_{randColumn}";

                 // NodeSheet로 노드를 초기화합니다.
                 Node nodeComponent = spawnedNode.GetComponent<Node>();

                 nodeComponent.Init_Right();

                 //에너미가 소유하는 노드에 추가합니다.
                 enemyBase.AddNodes(nodeComponent);
                 RightAttackNum++; // 오른쪽 몬스터에 몇개의 공격포인트가 생성되었는지 알려주는 변수
             }
             else
             {
                 Debug.Log("오른쪽 몬스터의 공격포인트를 생성하지 않습니다.");
             }

             if (RightRow == 6 && RightAttackNum == 0)
             {// for문이 7번 돌았을때까지 생성된 공격포인트가 0개 일경우(예외설정)
                 int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                 int randRow = UnityEngine.Random.Range(0, 7);
                 GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                 Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                 GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                 Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                 nodeComponent2.Init_Right();
                 //에너미가 소유하는 노드에 추가합니다.
                 enemyBase.AddNodes(nodeComponent2);
                 RightAttackNum++;
             }
         }
         RightRow = 0;
     }
     public void SpawnLeft_Node(EnemyBase enemyBase)
     {
         //노드가 0개가 아니라면 생성 된 노드를 삭제하고 Queue를 비웁니다.
         if (enemyBase.GetOwnNodes().Count != 0)
         {
             foreach (Node ownNode in enemyBase.GetOwnNodes())
             {
                 Destroy(ownNode.gameObject);
             }
             enemyBase.GetOwnNodes().Clear();
         }

         NodeArea nodeArea = enemyBase.nodeArea;

         // NodeArea가 발견되었는지 확인하십시오.
         if (nodeArea == null)
         {
             Debug.LogError("적 또는 적의 자식에서 NodeArea를 찾을 수 없습니다.");
             return;
         }

         // 스폰할 노드 수는 행 수와 같습니다.
         int spawnNodeNum = nodeArea.Rows;
         // spawnNodeNum = 7; 0,1,2,3,4,5,6
         for (LeftRow = 0; LeftRow < spawnNodeNum; LeftRow++)
         {
             int ranAttackNodeNum = Random.Range(0, 2); // 우선은 50퍼센트 확률

             if (ranAttackNodeNum == 0)
             {// 생성되는 확률이 뽑혔다면
              // 이 행에서 임의로 열을 선택합니다.
                 int randColumn = UnityEngine.Random.Range(0, nodeArea.Columns);

                 // 목록에서 열에 해당하는 노드 프리팹을 가져옵니다.
                 GameObject nodePrefab = GetNodePrefab(randColumn);

                 // NodeArea에서 이 노드의 위치를 가져옵니다.
                 Vector2 spawnPosition = nodeArea.GetGridPosition(LeftRow, randColumn);

                 // 계산된 위치에 노드를 인스턴스화하고 nodeArea의 자식으로 설정합니다.
                 GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                 // 노드의 이름을 설정합니다.
                 spawnedNode.name = $"Node_{LeftRow}_{randColumn}";

                 // NodeSheet로 노드를 초기화합니다.
                 Node nodeComponent = spawnedNode.GetComponent<Node>();

                 nodeComponent.Init_Left();

                 //에너미가 소유하는 노드에 추가합니다.
                 enemyBase.AddNodes(nodeComponent);
                 LeftAttackNum++; // 왼쪽몬스터에 몇개의 공격포인트가 생성되었는지 알려주는 변수
             }
             else
             {// 뽑히지 않았다면 공격포인트를 생성하지 않음
                 Debug.Log("왼쪽 몬스터의 공격포인트를 생성하지 않습니다.");
             }

             if (LeftRow == 6 && LeftAttackNum == 0)
             {// for문이 7번 돌았을때까지 생성된 공격포인트가 0개 일경우(예외설정)
                 int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                 int randRow = UnityEngine.Random.Range(0, 7);
                 GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                 Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                 GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                 Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                 nodeComponent2.Init_Right();
                 //에너미가 소유하는 노드에 추가합니다.
                 enemyBase.AddNodes(nodeComponent2);
                 LeftAttackNum++;
             }
         }
         LeftRow = 0;
     }
     private GameObject GetNodePrefab(int Column)
     {
         //랜덤 스폰이 아니라면
         if (!IsSpawnAlignmentRandom)
         {
             if (Column == 0)
                 return m_NodeList.Find(x => x.GetComponent<Node>().nodeSheet.m_NodeType == ENodeType.Attack);
             if (Column == 1)
                 return m_NodeList.Find(x => x.GetComponent<Node>().nodeSheet.m_NodeType == ENodeType.Attack);
             if (Column == 2)
                 return m_NodeList.Find(x => x.GetComponent<Node>().nodeSheet.m_NodeType == ENodeType.Attack);

             return null;
         }

         //else 랜덤 스폰이라면
         return m_NodeList[UnityEngine.Random.Range(0, m_NodeList.Count)];
     }
 }





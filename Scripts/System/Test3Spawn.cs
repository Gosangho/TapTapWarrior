using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


 public class Test3Spawn : MonoBehaviour
 {
     // �ν��Ͻ��� ���� ���� ����
     public static Test3Spawn Instance { get; private set; }

     [Header("Spawn Setting")]
     [SerializeField] bool IsSpawnAlignmentRandom = false;
     [SerializeField] public GameObject m_SpawnLocation_Right;
     [SerializeField] public GameObject m_SpawnLocation_Left;
     [SerializeField] public List<GameObject> m_NodeList = new List<GameObject>(); // ���Ϳ� ������ ��帮��Ʈ
     [SerializeField] private List<GameObject> m_monsterPrefabList; // ������ ���� ����Ʈ
     public Test3Battle BattleInstance;
     public GameObject AttackNode;
     public Vector2 SpawnNodePosition;
     public int RightAttackNum;
     public int LeftAttackNum;
     int RightRow;
     int LeftRow;

     private void Awake()
     {
         // NodeSpawnManager �ν��Ͻ��� �ϳ��� �ִ��� Ȯ���Ͻʽÿ�.
         if (Instance == null)
         {
             Instance = this;
             //DontDestroyOnLoad(gameObject); // ���� ����: �� �����ڰ� ��� �ε� ���� ���ӵǵ��� �Ϸ���
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
         // SpawnLocation �ڽ� ��ü ã��
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
         //��尡 0���� �ƴ϶�� ���� �� ��带 �����ϰ� Queue�� ���ϴ�.
         if (enemyBase.GetOwnNodes().Count != 0)
         {
             foreach (Node ownNode in enemyBase.GetOwnNodes())
             {
                 Destroy(ownNode.gameObject);
             }
             enemyBase.GetOwnNodes().Clear();
         }

         NodeArea nodeArea = enemyBase.nodeArea;

         // NodeArea�� �߰ߵǾ����� Ȯ���Ͻʽÿ�.
         if (nodeArea == null)
         {
             Debug.LogError("�� �Ǵ� ���� �ڽĿ��� NodeArea�� ã�� �� �����ϴ�.");
             return;
         }

         // ������ ��� ���� �� ���� �����ϴ�.
         int spawnNodeNum = nodeArea.Rows;
         // 7�� 3���� ���� -> �� �࿡�� 3���� ������ ��������Ʈ�� �������� ������ Ȯ���� ����
         for (RightRow = 0; RightRow < spawnNodeNum; RightRow++)
         {
             int ranAttackNode = Random.Range(0, 2); // 50�ۼ�Ʈ Ȯ��
             if (ranAttackNode == 0)
             {
                 // �� �࿡�� ���Ƿ� ���� �����մϴ�.
                 int randColumn = UnityEngine.Random.Range(0, nodeArea.Columns);

                 // ��Ͽ��� ���� �ش��ϴ� ��� �������� �����ɴϴ�.
                 GameObject nodePrefab = GetNodePrefab(randColumn);

                 // NodeArea���� �� ����� ��ġ�� �����ɴϴ�.
                 Vector2 spawnPosition = nodeArea.GetGridPosition(RightRow, randColumn);

                 // ���� ��ġ�� ��带 �ν��Ͻ�ȭ�ϰ� nodeArea�� �ڽ����� �����մϴ�.
                 GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                 // ����� �̸��� �����մϴ�.
                 spawnedNode.name = $"Node_{RightRow}_{randColumn}";

                 // NodeSheet�� ��带 �ʱ�ȭ�մϴ�.
                 Node nodeComponent = spawnedNode.GetComponent<Node>();

                 nodeComponent.Init_Right();

                 //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                 enemyBase.AddNodes(nodeComponent);
                 RightAttackNum++; // ������ ���Ϳ� ��� ��������Ʈ�� �����Ǿ����� �˷��ִ� ����
             }
             else
             {
                 Debug.Log("������ ������ ��������Ʈ�� �������� �ʽ��ϴ�.");
             }

             if (RightRow == 6 && RightAttackNum == 0)
             {// for���� 7�� ������������ ������ ��������Ʈ�� 0�� �ϰ��(���ܼ���)
                 int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                 int randRow = UnityEngine.Random.Range(0, 7);
                 GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                 Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                 GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                 Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                 nodeComponent2.Init_Right();
                 //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                 enemyBase.AddNodes(nodeComponent2);
                 RightAttackNum++;
             }
         }
         RightRow = 0;
     }
     public void SpawnLeft_Node(EnemyBase enemyBase)
     {
         //��尡 0���� �ƴ϶�� ���� �� ��带 �����ϰ� Queue�� ���ϴ�.
         if (enemyBase.GetOwnNodes().Count != 0)
         {
             foreach (Node ownNode in enemyBase.GetOwnNodes())
             {
                 Destroy(ownNode.gameObject);
             }
             enemyBase.GetOwnNodes().Clear();
         }

         NodeArea nodeArea = enemyBase.nodeArea;

         // NodeArea�� �߰ߵǾ����� Ȯ���Ͻʽÿ�.
         if (nodeArea == null)
         {
             Debug.LogError("�� �Ǵ� ���� �ڽĿ��� NodeArea�� ã�� �� �����ϴ�.");
             return;
         }

         // ������ ��� ���� �� ���� �����ϴ�.
         int spawnNodeNum = nodeArea.Rows;
         // spawnNodeNum = 7; 0,1,2,3,4,5,6
         for (LeftRow = 0; LeftRow < spawnNodeNum; LeftRow++)
         {
             int ranAttackNodeNum = Random.Range(0, 2); // �켱�� 50�ۼ�Ʈ Ȯ��

             if (ranAttackNodeNum == 0)
             {// �����Ǵ� Ȯ���� �����ٸ�
              // �� �࿡�� ���Ƿ� ���� �����մϴ�.
                 int randColumn = UnityEngine.Random.Range(0, nodeArea.Columns);

                 // ��Ͽ��� ���� �ش��ϴ� ��� �������� �����ɴϴ�.
                 GameObject nodePrefab = GetNodePrefab(randColumn);

                 // NodeArea���� �� ����� ��ġ�� �����ɴϴ�.
                 Vector2 spawnPosition = nodeArea.GetGridPosition(LeftRow, randColumn);

                 // ���� ��ġ�� ��带 �ν��Ͻ�ȭ�ϰ� nodeArea�� �ڽ����� �����մϴ�.
                 GameObject spawnedNode = Instantiate(nodePrefab, spawnPosition, Quaternion.identity, nodeArea.transform);

                 // ����� �̸��� �����մϴ�.
                 spawnedNode.name = $"Node_{LeftRow}_{randColumn}";

                 // NodeSheet�� ��带 �ʱ�ȭ�մϴ�.
                 Node nodeComponent = spawnedNode.GetComponent<Node>();

                 nodeComponent.Init_Left();

                 //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                 enemyBase.AddNodes(nodeComponent);
                 LeftAttackNum++; // ���ʸ��Ϳ� ��� ��������Ʈ�� �����Ǿ����� �˷��ִ� ����
             }
             else
             {// ������ �ʾҴٸ� ��������Ʈ�� �������� ����
                 Debug.Log("���� ������ ��������Ʈ�� �������� �ʽ��ϴ�.");
             }

             if (LeftRow == 6 && LeftAttackNum == 0)
             {// for���� 7�� ������������ ������ ��������Ʈ�� 0�� �ϰ��(���ܼ���)
                 int randColumn2 = UnityEngine.Random.Range(0, nodeArea.Columns);
                 int randRow = UnityEngine.Random.Range(0, 7);
                 GameObject nodePrefab2 = GetNodePrefab(randColumn2);
                 Vector2 spawnPosition2 = nodeArea.GetGridPosition(randRow, randColumn2);
                 GameObject spawnedNode2 = Instantiate(nodePrefab2, spawnPosition2, Quaternion.identity, nodeArea.transform);
                 Node nodeComponent2 = spawnedNode2.GetComponent<Node>();
                 nodeComponent2.Init_Right();
                 //���ʹ̰� �����ϴ� ��忡 �߰��մϴ�.
                 enemyBase.AddNodes(nodeComponent2);
                 LeftAttackNum++;
             }
         }
         LeftRow = 0;
     }
     private GameObject GetNodePrefab(int Column)
     {
         //���� ������ �ƴ϶��
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

         //else ���� �����̶��
         return m_NodeList[UnityEngine.Random.Range(0, m_NodeList.Count)];
     }
 }





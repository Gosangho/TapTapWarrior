using System;
using System.Collections.Generic;

using Spine;
using Spine.Unity;
using UnityEngine;

using Sirenix.OdinInspector;
using UnityEngine.Serialization;


 public enum EAreaType
 {
     Collider2D,
     SpineAnimation
 }

 public class NodeArea : MonoBehaviour
 {
    
     [Title("Boundary Setting")]
     [GUIColor(0.9f, 0.4f, 0.8f, 1f)]
     [Required("반드시 BoxCollider2D를 넣어주세요.")]
     public BoxCollider2D boxCollider2D;
     
     [GUIColor(0.9f, 0.4f, 0.8f, 1f)]
     [Required("반드시 skeletonAnimation를 설정해주세요.")]
     public SkeletonAnimation skeletonAnimation;
     
     [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
     [InfoBox("Collider2D는 BoxCollider2D의 크기를 기준으로 합니다.\n" + "SpineAnimation은 SkeletonAnimation의 크기를 기준으로 합니다.")]
     [EnumToggleButtons, HideLabel]
     [SerializeField] private EAreaType m_areaType = EAreaType.Collider2D;
     
     
     [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
     /*[MinValue(1),MaxValue(10)]*/
     [ProgressBar(1, 10)]
     [SerializeField] private int m_rows = 7;
     
     [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
     /*[MinValue(1),MaxValue(10)]*/
     [ProgressBar(1, 10)]
     [SerializeField] private int m_columns = 3;
     
     public int Rows => m_rows;
     public int Columns => m_columns;
     
     [GUIColor(0.3f, 0.8f, 0.8f, 1f)]
     [DisableIf("m_areaType", EAreaType.Collider2D)]
     [SerializeField] private Vector2 m_spineBoundOffset;
     
     [Title("Debug Setting")]
     [GUIColor(0.9f, 0.24f, 0.38f, 1f)]
     [SerializeField] private bool m_debugVisible = false;
     
     private Vector2 m_boxSize;
     private Bounds m_spineBounds;
     private List<Vector2> m_gridCenters = new List<Vector2>();
     
     
     // 마지막으로 알려진 위치, 크기 및 회전을 저장합니다.
     private Vector2 m_lastPosition;
     private Vector2 m_lastSize;
     private Quaternion m_lastRotation;
     
     private void Awake()
     {
         if (boxCollider2D==null)
         {
             boxCollider2D = GetComponentInParent<BoxCollider2D>();    
         }
         
         BoxSizeSetup();
         DivideBoxColliderArea(m_boxSize);
         
         m_lastPosition = transform.position;
         m_lastSize = m_boxSize;
         m_lastRotation = transform.rotation;
     }

     private void Start()
     {
         /*if (boxCollider2D==null)
         {
             boxCollider2D = GetComponentInParent<BoxCollider2D>();    
         }
         
         BoxSizeSetup();
         DivideBoxColliderArea(m_boxSize);
         
         m_lastPosition = transform.position;
         m_lastSize = m_boxSize;
         m_lastRotation = transform.rotation;*/
     }

     private void Update()
     {
         if (HasStateChanged())
         {
             UpdateBounds();
             DivideBoxColliderArea(m_boxSize);
             UpdateLastKnownState();
         }
     }
     
     private bool HasStateChanged()
     {
         return m_lastPosition != (Vector2)transform.position ||
                m_lastSize != m_boxSize ||
                m_lastRotation != transform.rotation;
     }
     
     private void UpdateBounds()
     {
         switch (m_areaType)
         {
             case EAreaType.Collider2D:
                 m_boxSize = boxCollider2D.bounds.size;
                 break;
             case EAreaType.SpineAnimation:
                 m_spineBounds = GetBoundingBox(skeletonAnimation.Skeleton);
                 m_boxSize = m_spineBounds.size;
                 break;
         }
     }
     
     private void UpdateLastKnownState()
     {
         m_lastPosition = transform.position;
         m_lastSize = m_boxSize;
         m_lastRotation = transform.rotation;
     }

     private void BoxSizeSetup()
     {
         switch (m_areaType)
         {
             case EAreaType.Collider2D:
                 m_boxSize = boxCollider2D.bounds.size;
                 break;
             case EAreaType.SpineAnimation:
                 m_spineBounds = GetBoundingBox(skeletonAnimation.Skeleton);
                 m_boxSize = m_spineBounds.size;
                 break;
         }
     }
     
     void DivideBoxColliderArea(Vector2 size)
     {
         float rectWidth = size.x / m_columns;
         float rectHeight = size.y / m_rows;

         m_gridCenters.Clear();

         for (int i = 0; i < m_rows; i++)
         {
             // 각 행의 하단에 시작점을 정의합니다.
             Vector2 startPoint;
             
             switch(m_areaType)
             {
                 case EAreaType.Collider2D:
                     startPoint = (Vector2)transform.position - size / 2f + new Vector2(0, rectHeight * i+ boxCollider2D.offset.y);
                     break;
                 case EAreaType.SpineAnimation:
                     // 스켈레톤 경계의 최소 y값을 시작점으로 사용
                     startPoint = new Vector2(transform.position.x + m_spineBounds.center.x -size.x/2f,
                                      transform.position.y +m_spineBounds.center.y - m_spineBounds.extents.y) +
                                  new Vector2(0, rectHeight * i);
                     break;
                 
                 default: startPoint = Vector2.zero; break;
             }
             
             for (int j = 0; j < m_columns; j++)
             {
                 Vector2 center = startPoint + new Vector2(rectWidth * (j + 0.5f), rectHeight * 0.5f);
                 m_gridCenters.Add(center);
             }
         }
     }
     
     Bounds GetBoundingBox(Skeleton skeleton)
     {
         float minX = float.PositiveInfinity, minY = float.PositiveInfinity,
             maxX = float.NegativeInfinity, maxY = float.NegativeInfinity;

         foreach (Bone bone in skeleton.Bones)
         {
             minX = Mathf.Min(minX, bone.WorldX);
             minY = Mathf.Min(minY, bone.WorldY);
             maxX = Mathf.Max(maxX, bone.WorldX);
             maxY = Mathf.Max(maxY, bone.WorldY);
         }
         
         Vector3 size = new Vector3(maxX - minX, maxY - minY, 0) + (Vector3)m_spineBoundOffset;
         Vector3 center = new Vector3(minX + size.x / 2, minY + size.y / 2, 0);
         return new Bounds(center, size);
     }
     
     public float GetRectangleWidth()
     {
         return m_boxSize.x / m_columns;
     }

     public float GetRectangleHeight()
     {
         return m_boxSize.y / m_rows;
     }
     
     public Vector2 GetGridPosition(int row, int column)
     {
         if (row < 0 || row >= m_rows || column < 0 || column >= m_columns)
         {
             Debug.LogError("Row or column out of bounds");
             return Vector2.zero;
         }

         int index = row * m_columns + column;

         return m_gridCenters[index];
     }

     void OnDrawGizmos()
     {
         if (m_areaType == EAreaType.SpineAnimation)
         {
             Gizmos.color = Color.red;
             Gizmos.DrawWireCube(gameObject.transform.position + m_spineBounds.center, m_spineBounds.size);
         }
         
         if (m_debugVisible)
         {
             BoxSizeSetup();
             DivideBoxColliderArea(m_boxSize);
             
             Gizmos.color = Color.blue;
             foreach (Vector2 center in m_gridCenters)
             {
                 float rectWidth = m_boxSize.x / m_columns;
                 float rectHeight = m_boxSize.y / m_rows;
                 Vector2 rectPosition = center - new Vector2(rectWidth / 2f, rectHeight / 2f);
                 Gizmos.DrawWireCube(rectPosition + new Vector2(rectWidth / 2f, rectHeight / 2f),
                     new Vector3(rectWidth, rectHeight, 0f));

                 Gizmos.DrawSphere(center, 0.025f);
             }
         }
     }
     
 }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


 public abstract class NodeBase<TNodeSheet> : MonoBehaviour where TNodeSheet : NodeSheet
 {
     
     [Header("Node Settings")]
     [SerializeField] protected TNodeSheet m_sheet = null;

     public  NodeSheet nodeSheet => m_sheet;
        
     //이 노트가 현재 히트 할 수 있는 가?
     private bool m_isHitable = false;
     public virtual void Init_Right(){}
     public virtual void Init_Left() {}

 }

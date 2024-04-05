using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;



 public enum ENodeType
 {
     Up,
     Left,
     Right,
     Attack,
     Default
 }
 
 public class NodeSheet : ScriptableObject
 {
     [Header("Attribute")]
     [SerializeField] public ENodeType m_NodeType;
     
     [SerializeField] public Sprite m_NodeImage;

 }

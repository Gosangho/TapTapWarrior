using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




  public class Node : NodeBase<NodeSheet>
  {
      
      [SerializeField] private GameObject m_button;
      public GameObject nodeButton => m_button;
      

      public override void Init_Right()
      {
          // SpriteRenderer 컴포넌트를 얻습니다.
          SpriteRenderer nodeSprite = m_button.GetComponent<SpriteRenderer>();
          // Node의 스프라이트를 설정합니다.
          nodeSprite.sprite = m_sheet.m_NodeImage;
          
          
          // 부모 오브젝트인 NodeArea를 얻습니다.
          NodeArea nodeArea = GetComponentInParent<NodeArea>();
          
          // NodeArea로부터 사각형의 너비와 높이를 얻습니다.
          float width = nodeArea.GetRectangleWidth();
          float height = nodeArea.GetRectangleHeight();

          // 스프라이트의 실제 너비와 높이를 얻습니다.
          float spriteWidth = nodeSprite.sprite.bounds.size.x;
          float spriteHeight = nodeSprite.sprite.bounds.size.y;

          // 사각형의 너비와 높이를 스프라이트의 너비와 높이로 나누어서, 스케일 팩터를 계산합니다.
          float scaleX = width / spriteWidth;
          float scaleY = height / spriteHeight;

          // 계산한 스케일 팩터를 이용해 게임 오브젝트의 스케일을 설정합니다.
          gameObject.transform.localScale = new Vector3(scaleX, scaleY, 1f);
      }

      public override void Init_Left()
      {
          // SpriteRenderer 컴포넌트를 얻습니다.
          SpriteRenderer nodeSprite = m_button.GetComponent<SpriteRenderer>();
          // Node의 스프라이트를 설정합니다.
          nodeSprite.sprite = m_sheet.m_NodeImage;


          // 부모 오브젝트인 NodeArea를 얻습니다.
          NodeArea nodeArea = GetComponentInParent<NodeArea>();

          // NodeArea로부터 사각형의 너비와 높이를 얻습니다.
          float width = nodeArea.GetRectangleWidth();
          float height = nodeArea.GetRectangleHeight();

          // 스프라이트의 실제 너비와 높이를 얻습니다.
          float spriteWidth = nodeSprite.sprite.bounds.size.x;
          float spriteHeight = nodeSprite.sprite.bounds.size.y;

          // 사각형의 너비와 높이를 스프라이트의 너비와 높이로 나누어서, 스케일 팩터를 계산합니다.
          float scaleX = width / spriteWidth;
          float scaleY = height / spriteHeight;

          // 계산한 스케일 팩터를 이용해 게임 오브젝트의 스케일을 설정합니다.
          gameObject.transform.localScale = new Vector3(-scaleX, scaleY, 1f);
      }

      public void SetLocalScale(Vector3 scale)
      {
          transform.localScale = scale;
      }
  }


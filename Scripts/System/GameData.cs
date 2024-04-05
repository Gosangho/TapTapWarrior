using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


  [System.Serializable]
  public class CharacterData
  {
      public string characterName;
      public int bestScore;
      public int totalKillScore;
      public string PlayerName;
  }

  [System.Serializable]
  public class PlayerData
  {
      public string PlayerId;
      public bool TutorialPlay;// 튜토리얼을 플레이 했는지의 여부
      public bool MakeNickName;// ID를 생성했는지의 여부
      public string PlayerName;
      public int Gold;
      public int AdsYn;// 광고 상품 구입여부
      public string AdsDate; // 광고 상품 구입 기간
      public bool SwordGirl2Get;
      public bool SwordGirl3Get;
      public bool LeonGet;
      public bool[] PlayerAttandence = new bool[31];// 플레이어의 출석상태
  }
   



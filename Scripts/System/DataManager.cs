
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    #region Singleton
    private static DataManager instance = null;
    public static DataManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    private string swordGirl1FileName = "sword1.json";
    private string swordGirl2FileName = "sword2.json";
    private string swordGirl3FileName = "sword3.json";
    private string leonFileName = "leon.json";
    private string playerDataFileName = "PlayerData.json";

    public CharacterData swordGirl1 = new CharacterData();
    public CharacterData swordGirl2 = new CharacterData();
    public CharacterData swordGirl3 = new CharacterData();
    public CharacterData leon = new CharacterData();
    public PlayerData playerData = new PlayerData();

    string[] prefsList = {"selectCharacter","controlType","soundSfx","soundBgm","tutorialPlay"};

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        LoadGameData();
    }

    private void LoadCharacterData(string fileName, ref CharacterData character)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            character = JsonUtility.FromJson<CharacterData>(jsonData);
        }
    }

    public void LoadGameData()
    {
        LoadCharacterData(swordGirl1FileName, ref swordGirl1);
        LoadCharacterData(swordGirl2FileName, ref swordGirl2);
        LoadCharacterData(swordGirl3FileName, ref swordGirl3);
        LoadCharacterData(leonFileName, ref leon);

        string playerfilePath = Path.Combine(Application.persistentDataPath, playerDataFileName);

        if (File.Exists(playerfilePath))
        {
            string playerjsonData = File.ReadAllText(playerfilePath);
            playerData = JsonUtility.FromJson<PlayerData>(playerjsonData);
        }

        Debug.Log("불러오기 완료");
    }

    public void SaveCharacterData(string fileName, CharacterData character)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string jsonData = JsonUtility.ToJson(character, true);

        File.WriteAllText(filePath, jsonData);
    }

    public void SaveGameData()
    {
        
        SaveCharacterData(swordGirl1FileName, swordGirl1);
        SaveCharacterData(swordGirl2FileName, swordGirl2);
        SaveCharacterData(swordGirl3FileName, swordGirl3);
        SaveCharacterData(leonFileName, leon);

        string playerfilePath = Path.Combine(Application.persistentDataPath, playerDataFileName);
        string playerjsonData = JsonUtility.ToJson(playerData, true);

        File.WriteAllText(playerfilePath, playerjsonData);

        Debug.Log("저장완료");
    }

    public void DbSaveGameData() {
        string playerfilePath = Path.Combine(Application.persistentDataPath, playerDataFileName);
        string playerjsonData = JsonUtility.ToJson(playerData, true);

        File.WriteAllText(playerfilePath, playerjsonData);
        BackEndManager.Instance.DbSaveGameData();
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
        Debug.Log("게임종료 및 저장완료");
    }

    public void selectCharacter(int prefsNum, string character)
    {
        PlayerPrefs.SetString(prefsList[prefsNum], character);
        PlayerPrefs.Save(); 
    }

    public string getCharacter(int prefsNum)
    {
        return PlayerPrefs.GetString(prefsList[prefsNum], "default");
    }
}


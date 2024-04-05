using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class audioManager : MonoBehaviour
{
    #region Singleton
    private static audioManager instance = null;
    public static audioManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [SerializeField]public AudioSource BgmAudio;
    [SerializeField]public AudioSource SfxAudio_Char_AttackAudio;
    [SerializeField]public AudioSource SfxAudio_Enemy_hitAudio;
    [SerializeField]public AudioSource SfxTutorial;
    [SerializeField]public AudioClip[] BgmClip;
    [SerializeField]public AudioClip[] SfxClip;
    [SerializeField]public AudioClip[] SfxClip_Enemyhit;
    public Image SfxImage;
    public Image BgmImage;
    public Sprite SfxFirstImage;
    public Sprite SfxSecondImage;
    public Sprite BgmFirstImage;
    public Sprite BgmSecondImage;

    public Slider sfxSlider;

    public Slider BgmSlider;

    private void Start()
    {
        BgmAudio = GameObject.Find("BGM_audio").GetComponent<AudioSource>();
        SfxAudio_Char_AttackAudio = GameObject.Find("SFX_audio_CharAttack").GetComponent<AudioSource>();
        SfxAudio_Enemy_hitAudio = GameObject.Find("SFX_audio_Enemyhit").GetComponent<AudioSource>();
        SfxTutorial = GameObject.Find("SFX_audio_Tutorialsfx").GetComponent<AudioSource>();

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            int ranBGM = Random.Range(2, 5);
            BgmAudio.clip = BgmClip[ranBGM];
            BgmAudio.Play();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            BgmAudio.clip = BgmClip[0];
            BgmAudio.Play();
        }
        else if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            BgmAudio.clip = BgmClip[1];
            BgmAudio.Play();
        }

        string sfxGet = DataManager.Instance.getCharacter(2);
        string bgmGet = DataManager.Instance.getCharacter(3);
        float sfxVolume = 0.0f;
        float bgmVolume = 0.0f;

        if("default".Equals(sfxGet))
        {
            sfxVolume = 0.5f;
        } else {
            sfxVolume = float.Parse(sfxGet);
        }
        
        if("default".Equals(bgmGet))
        {
            bgmVolume = 0.5f;
        } else {
            bgmVolume = float.Parse(bgmGet);
        }
        Debug.Log("sfxVolume::"+sfxVolume+" bgmVolume::"+bgmVolume);
        GetSfxAudioVolume(sfxVolume);
        GetBgmAudioVolume(bgmVolume);
    }

    
    private void Update()
    {
        if (!BgmAudio.isPlaying && SceneManager.GetActiveScene().buildIndex==3)
        {
            int ranBGM = Random.Range(2, 5);
            BgmAudio.clip = BgmClip[ranBGM];
            BgmAudio.Play();
        }

        if (!BgmAudio.isPlaying)
        {
            BgmAudio.Play();
        }
    }
    
    public void BgmAudioPlay(string type)
    {
        int BgmIndex = 0;

        switch (type)
        {
            case "LobyyBgm": BgmIndex = 0; break;
            case "TotorialBgm": BgmIndex = 1; break;
            case "BattleBgm1": BgmIndex = 2; break;
            case "BattleBgm2": BgmIndex = 3; break;
            case "BattleBgm3": BgmIndex = 4; break;
            case "QuitBgm": BgmIndex = 5; break;
        }
        BgmAudio.clip = BgmClip[BgmIndex];
        BgmAudio.Play();
    }

    public void SfxAudioPlay(string type)
    {
        int SfxIndex = 0;

        switch (type)
        {
            case "Ui_Click": SfxIndex = 0; break;
            case "Char_Attack1": SfxIndex = 1; break;
            case "Char_Attack2": SfxIndex = 2; break;
            case "Char_Dash1": SfxIndex = 3; break;
            case "Char_Dash2": SfxIndex = 4; break;
            case "Char_Dead": SfxIndex = 5; break;
            case "Char_Spirit": SfxIndex = 6; break;
            case "ItemGet": SfxIndex = 7; break;
            case "Tutorial_Text": SfxIndex = 8; break;
        }
        
        SfxAudio_Char_AttackAudio.clip = SfxClip[SfxIndex];
        SfxAudio_Char_AttackAudio.Play();
    }
    public void SfxAudioPlay_Enemy(string type)
    {
        int SfxIndex = 0;

        switch (type)
        {
            case "Enemy_Dead": SfxIndex = 0; break;
            case "Enemy_Hit": SfxIndex = 1; break;
            case "Enemy_Attack": SfxIndex = 2; break;
            case "Tutorial_Warning": SfxIndex = 3; break;
        }

        SfxAudio_Enemy_hitAudio.clip = SfxClip_Enemyhit[SfxIndex];
        SfxAudio_Enemy_hitAudio.Play();
    }

    public void SetSfxAudioVolume(float volume)
    {
        SfxAudio_Char_AttackAudio.volume = volume;
        SfxAudio_Enemy_hitAudio.volume = volume;
        SfxTutorial.volume = volume;

        if (volume <= 0)
        {
            SfxImage.sprite = SfxSecondImage;
        }
        else
        {
            SfxImage.sprite = SfxFirstImage;
        }

         DataManager.Instance.selectCharacter(2, volume.ToString());
    }
    public void SetBgmAudioVolume(float volume)
    {
        BgmAudio.volume = volume;
       
        if (volume <= 0)
        {
            BgmImage.sprite = BgmSecondImage;
        }
        else
        {
            BgmImage.sprite = BgmFirstImage;
        }
         DataManager.Instance.selectCharacter(3, volume.ToString());
    }

    void GetSfxAudioVolume(float volume)
    {
        SfxAudio_Char_AttackAudio.volume = volume;
        SfxAudio_Enemy_hitAudio.volume = volume;
        SfxTutorial.volume = volume;

        if(sfxSlider != null) {
            sfxSlider.value = volume;
        }

        if (volume <= 0)
        {
            if(SfxImage != null) {
                SfxImage.sprite = SfxSecondImage;
            }
        }
        else
        {
            if(SfxImage != null) {
                SfxImage.sprite = SfxFirstImage;
            }
        }

    }
    void GetBgmAudioVolume(float volume)
    {
        BgmAudio.volume = volume;

        if(BgmSlider != null) {
            BgmSlider.value = volume;
        }
       
        if (volume <= 0)
        {
            if(BgmImage != null) {
                BgmImage.sprite = BgmSecondImage;
            }
        }
        else
        {
            if(BgmImage != null) {
                BgmImage.sprite = BgmFirstImage;
            }
        }
    }


    public void PlayButtonClick()
    {
        SfxAudioPlay("Ui_Click");
    }
}

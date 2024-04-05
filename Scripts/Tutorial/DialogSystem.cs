using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour
{
    [SerializeField]
    private Speaker[] Speakers;
    [SerializeField]
    private DialogData[] Dialogs; // 대사의 목록 배열
    [SerializeField]
    private bool isAutoStart = true;
    private bool isFirst = true; // 최초 1회만 호출 하는지의 여부
    private int CurrentDialogIndex = -1; // 현재 대사 순번
    private int CurrentSpeakerIndex = 0;
    private float TypingSpeed = 0.1f; // 택스트 타이핑 효과의 재생속도
    private bool IsTypingEffect = false; // 택스트 타이핑 효과를 재생중인지의 여부


    private void Awake()
    {
        TutorialSetup();
    }

    private void TutorialSetup()
    {
        for (int i = 0; i < Speakers.Length; i++)
        {
            SetActiveObject(Speakers[i], false);
            Speakers[i].Leon.SetActive(true);
        }
    }

    public bool UpdateDialog()
    {
        if (isFirst == true)
        {
            TutorialSetup();

            if (isAutoStart)
            {
                SetNextDialog();
            }
            isFirst = false;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (IsTypingEffect == true)
            {
                IsTypingEffect = false;

                // 타이핑 효과를 중지하고, 현재 대사를 전체 출력
                StopCoroutine("OnTypingText");
                Speakers[CurrentSpeakerIndex].TextDialog.text = Dialogs[CurrentDialogIndex].Dialogues;
                // 대사가 완료되었을 때 출력되는 커서 활성화
                Speakers[CurrentSpeakerIndex].ObjectArrow.SetActive(true);
                return false;
            }


            if (Dialogs.Length > CurrentDialogIndex +1)
            {// 대사가 더이상 없을경우
                SetNextDialog();
            }
            else
            {// 더이상 대화가 남아있지 않다면
                for (int i = 0; i < Speakers.Length; i++)
                {
                    SetActiveObject(Speakers[i], false);
                    Speakers[i].Leon.SetActive(false);
                }
                // Skip버튼을 Go버튼으로 변경
                // 씬 전환
                return true;
            }

        }
        return false;
    }

    public void SetNextDialog()
    {
        SetActiveObject(Speakers[CurrentSpeakerIndex], false);
        CurrentDialogIndex++;
        CurrentSpeakerIndex = Dialogs[CurrentDialogIndex].SpeakerIndex;
        SetActiveObject(Speakers[CurrentSpeakerIndex], true);
        Speakers[CurrentSpeakerIndex].TextName.text = Dialogs[CurrentDialogIndex].Name;
        Speakers[CurrentSpeakerIndex].TextDialog.text = Dialogs[CurrentDialogIndex].Dialogues;

        StartCoroutine("OnTypingText"); // 타이핑을 한글자씩 재생하도록
    }

    public void SetActiveObject(Speaker speaker, bool visible)
    {
        speaker.ImageDialog.gameObject.SetActive(visible);
        speaker.TextName.gameObject.SetActive(visible);
        speaker.TextDialog.gameObject.SetActive(visible);
        //speaker.ImageArrow.gameObject.SetActive(visible);
        speaker.Leon.SetActive(visible);

        speaker.ObjectArrow.SetActive(false);
        // 대화가 종료되었을떄만 활성화(대화창 오른쪽하단의 에로우)
    }

    IEnumerator OnTypingText()
    {
        int index = 0;

        IsTypingEffect = true;
        audioManager.Instance.SfxAudioPlay("Tutorial_Text"); // 대화창의 텍스트 출력시 재생되는 오디오
        // 택스를 한글자씩 재생
        while (index < Dialogs[CurrentDialogIndex].Dialogues.Length+1)
        {
            Speakers[CurrentDialogIndex].TextDialog.text = Dialogs[CurrentDialogIndex]
                .Dialogues.Substring(0, index);
            index++;
            yield return new WaitForSeconds(TypingSpeed);
        }

        IsTypingEffect = false;

        // 대사가 완료되었을때 출력되는 커서 활성화
        Speakers[CurrentSpeakerIndex].ObjectArrow.SetActive(true);
    }
}
[System.Serializable]
public struct Speaker
{
    public GameObject Leon;
    public Image ImageDialog; // 대화창 Image UI
    public TextMeshProUGUI TextName;
    public TextMeshProUGUI TextDialog;
    public GameObject ObjectArrow; // 대사완료시 출력되는 에로우
    //public Image ImageArrow;
}
[System.Serializable]
public struct DialogData
{
    public int SpeakerIndex; // 대화의 순서를 정함
    public string Name; // 캐릭터 이름
    public string Dialogues; // 대사
}

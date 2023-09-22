using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialUI;
    public Button switchModeBtn; 
    public Button[] tutorial1UnableButtons; //�T�ΦP�����W�����s
    public PlayableDirector tutorialDirector;

    private bool switchModeBtnClicked;

    private void OnEnable()
    {
        for (int i = 0; i < tutorial1UnableButtons.Length; i++)
        {
            tutorial1UnableButtons[i].interactable = false;
        }
    }
    private void Start()
    {
        switchModeBtnClicked = false;
        switchModeBtn.onClick.AddListener(StartSwitchModeBtnClickedCor);
    }
    public void StartTutorialTimeline()
    {
        tutorialDirector.playableGraph.GetRootPlayable(0).Play();
    }

    public void StartSwitchModeTutorial1()
    {
        TutorialUI.SetActive(true);
        GameManager.Instance.SetState(GameState.Paused);
        tutorialDirector.playableGraph.GetRootPlayable(0).Pause();
    }
    public void StartSwitchModeBtnClickedCor()
    {
        StartCoroutine(SwitchModeBtnClicked());
    }
    private IEnumerator SwitchModeBtnClicked()
    {
        switchModeBtnClicked = true;
        //�Ѱ��Ȱ�

        switchModeBtn.interactable = false;
        yield return Yielders.GetWaitForSeconds(1f);

        //�i��U�@�ӫ���
        for (int i = 0; i < tutorial1UnableButtons.Length; i++)
        {
            tutorial1UnableButtons[i].interactable = true;
        }

        switchModeBtnClicked = false;
        Debug.Log("��U�@�Ӵ���");
        tutorialDirector.playableGraph.GetRootPlayable(0).Play();
    }
}

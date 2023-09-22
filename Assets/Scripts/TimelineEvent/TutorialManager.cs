using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialUI;
    public Button switchModeBtn; 
    public Button[] tutorial1UnableButtons; //禁用同場景上的按鈕
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
        //解除暫停

        switchModeBtn.interactable = false;
        yield return Yielders.GetWaitForSeconds(1f);

        //進到下一個指示
        for (int i = 0; i < tutorial1UnableButtons.Length; i++)
        {
            tutorial1UnableButtons[i].interactable = true;
        }

        switchModeBtnClicked = false;
        Debug.Log("到下一個提示");
        tutorialDirector.playableGraph.GetRootPlayable(0).Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialUI;
    public GameObject TutorialUI2;
    public Button switchModeBtn; 
    public Button[] tutorial1UnableButtons; //禁用同場景上的按鈕
    public PlayableDirector tutorialDirector;

    Mouse mouse = Mouse.current;

    private bool switchModeBtnClicked;
    private bool isTutorial2;

    private void Start()
    {
        switchModeBtnClicked = false;
        isTutorial2 = false;

        if (switchModeBtnClicked ==false)
        {
            switchModeBtn.onClick.AddListener(StartSwitchModeBtnClickedCor);
        }
    }
    private void OnDisable()
    {
        switchModeBtn.onClick.RemoveListener(StartSwitchModeBtnClickedCor);
    }
    private void Update()
    {
        if (mouse.leftButton.wasPressedThisFrame && isTutorial2)
        {
            TutorialUI2.SetActive(false);

            for (int i = 0; i < tutorial1UnableButtons.Length; i++)
            {
                tutorial1UnableButtons[i].interactable = true;
            }

            tutorialDirector.Stop();
            isTutorial2 = false;
            Time.timeScale = 1;
        }
    }
    public void StartTutorialTimeline()
    {
        tutorialDirector.Play();
    }

    public void StartSwitchModeTutorial1()
    {
        TutorialUI.SetActive(true);

        for (int i = 0; i < tutorial1UnableButtons.Length; i++)
        {
            tutorial1UnableButtons[i].interactable = false;
        }

        tutorialDirector.Pause();
        Time.timeScale = 0;
    }
    public void StartSwitchModeBtnClickedCor()
    {
        StartCoroutine(SwitchModeBtnClicked());
    }

    private IEnumerator SwitchModeBtnClicked()
    {
        switchModeBtnClicked = true;
        tutorialDirector.playableGraph.GetRootPlayable(0).Play();
        Time.timeScale = 1;
        TutorialUI.SetActive(false);
        //switchModeBtn.interactable = false;
        yield return Yielders.GetWaitForSeconds(0.5f);

        switchModeBtnClicked = false;
        Debug.Log("到下一個提示");
        StartSwitchModeTutorial2();
    }
    public void StartSwitchModeTutorial2()
    {
        isTutorial2 = true;
        TutorialUI2.SetActive(true);
        Time.timeScale = 0;
    }
}

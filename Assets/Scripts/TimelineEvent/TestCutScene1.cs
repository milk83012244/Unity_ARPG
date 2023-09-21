using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Playables;

/// <summary>
/// 測試過場用
/// </summary>
public class TestCutScene1 : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    public PlayableDirector playableDirector;

    public string dialogueNode;

    public void StartDialogue(string dialogueNode)
    {
        if (dialogueRunner == null)
        {
            Debug.Log("沒有DialogueRunner組件");
            return;
        }
        dialogueRunner.onDialogueComplete.AddListener(DialogueEnd); //對話完成事件

        dialogueRunner.StartDialogue(dialogueNode);
        //playableDirector.Pause();
        playableDirector.playableGraph.GetRootPlayable(0).Pause();

    }
    private void DialogueEnd()
    {
        playableDirector.playableGraph.GetRootPlayable(0).Play();
    }
}

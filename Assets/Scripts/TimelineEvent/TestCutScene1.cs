using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Playables;

/// <summary>
/// ���չL����
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
            Debug.Log("�S��DialogueRunner�ե�");
            return;
        }
        dialogueRunner.onDialogueComplete.AddListener(DialogueEnd); //��ܧ����ƥ�

        dialogueRunner.StartDialogue(dialogueNode);
        //playableDirector.Pause();
        playableDirector.playableGraph.GetRootPlayable(0).Pause();

    }
    private void DialogueEnd()
    {
        playableDirector.playableGraph.GetRootPlayable(0).Play();
    }
}

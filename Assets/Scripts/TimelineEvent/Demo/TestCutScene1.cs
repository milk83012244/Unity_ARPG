using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Playables;

/// <summary>
/// ���չL����ܥ�
/// </summary>
public class TestCutScene1 : MonoBehaviour
{
    [SerializeField] private DialogueRunner dialogueRunner;

    public PlayableDirector playableDirector;

    public string dialogueNode;

    private void Awake()
    {
        if (GameManager.Instance.isNewGame)
        {
            playableDirector.Play();
        }
    }

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
        dialogueRunner.onDialogueComplete.RemoveListener(DialogueEnd);
    }
}

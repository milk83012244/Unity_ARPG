using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class DialogueBubbleCreate : MonoBehaviour
{
   static Transform bubbleTransform;
   static DialogueRunner dialogueRunner;
    private void Awake()
    {
        Init();
    }
    /// <summary>
    /// �ͦ���ܮ�
    /// </summary>
    public static void Create(DialogueRunner dialogueRunner, Transform parent, Vector3 localPosition, string dialogueStartNode)
    {
        if (dialogueRunner.IsDialogueRunning)
        {
            return;
        }
        dialogueRunner.gameObject.SetActive(true);

        if (dialogueRunner.gameObject.activeSelf)
        {
            if (bubbleTransform == null) //�S����ܮشN�ͦ�
            {
                bubbleTransform = Instantiate(GameAssets.i.dialogueBubbleCanvas, parent);
                bubbleTransform.localPosition = localPosition;
            }
            else
            {
                bubbleTransform.SetParent(parent);
                bubbleTransform.localPosition = localPosition;
            }

            DialogueViewBase dialogueView = bubbleTransform.GetComponent<DialogueViewBase>(); //�]�w�ͦ�����w�ب�Runner
            DialogueViewBase[] currentDialogueView = new DialogueViewBase[] { dialogueView };
            dialogueRunner.SetDialogueViews(currentDialogueView); //�]�w��ܮت����e(�~��DialogueViewBase)���}��

            dialogueRunner.StartDialogue("TestYarnScript");//�qYarn�}�������wtitle�}�l
        }
    }
    public void Init()
    {
        bubbleTransform = GameObject.Find("MainDialogueCanvas").transform;
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
        //if (dialogueRunner.gameObject.activeSelf)
        //{
        //    DialogueViewBase dialogueView = bubbleTransform.GetComponent<DialogueViewBase>(); //�]�w�ͦ�����w�ب�Runner
        //    DialogueViewBase[] currentDialogueView = new DialogueViewBase[] { dialogueView };
        //    dialogueRunner.SetDialogueViews(currentDialogueView);

        //    //bubbleTransform.gameObject.SetActive(false);
        //}
    }
}

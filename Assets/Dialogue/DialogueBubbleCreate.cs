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
    /// 生成對話框
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
            if (bubbleTransform == null) //沒有對話框就生成
            {
                bubbleTransform = Instantiate(GameAssets.i.dialogueBubbleCanvas, parent);
                bubbleTransform.localPosition = localPosition;
            }
            else
            {
                bubbleTransform.SetParent(parent);
                bubbleTransform.localPosition = localPosition;
            }

            DialogueViewBase dialogueView = bubbleTransform.GetComponent<DialogueViewBase>(); //設定生成的氣泡框到Runner
            DialogueViewBase[] currentDialogueView = new DialogueViewBase[] { dialogueView };
            dialogueRunner.SetDialogueViews(currentDialogueView); //設定對話框的內容(繼承DialogueViewBase)的腳本

            dialogueRunner.StartDialogue("TestYarnScript");//從Yarn腳本的指定title開始
        }
    }
    public void Init()
    {
        bubbleTransform = GameObject.Find("MainDialogueCanvas").transform;
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
        //if (dialogueRunner.gameObject.activeSelf)
        //{
        //    DialogueViewBase dialogueView = bubbleTransform.GetComponent<DialogueViewBase>(); //設定生成的氣泡框到Runner
        //    DialogueViewBase[] currentDialogueView = new DialogueViewBase[] { dialogueView };
        //    dialogueRunner.SetDialogueViews(currentDialogueView);

        //    //bubbleTransform.gameObject.SetActive(false);
        //}
    }
}

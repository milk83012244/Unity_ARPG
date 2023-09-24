using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 存檔點
/// </summary>
public class SavePoint : MonoBehaviour
{
    public GameObject saveSlotsPanelPrefab;
    [HideInInspector] public GameObject saveSlotsPanelInstance;

    [SerializeField] private GameObject saveLoadMenuPrefab;
    public SaveLoadMenu saveLoadMenu;

    [HideInInspector] public SaveSlotsPanelMenu saveSlotsPanelMenu;

    public Transform canvasParent;


    public void OpenSaveSlot(bool isSave)
    {
        InstantiateSaveSlotPanel(isSave);
    }

    /// <summary>
    /// 生成存檔槽菜單
    /// </summary>
    public void InstantiateSaveSlotPanel(bool isSave)
    {
        if (saveSlotsPanelInstance == null)
            saveSlotsPanelInstance = Instantiate(saveSlotsPanelPrefab, canvasParent);

        if (saveSlotsPanelInstance != null)
        {
            saveSlotsPanelMenu = saveSlotsPanelInstance.GetComponent<SaveSlotsPanelMenu>();
            if (isSave)
                saveSlotsPanelMenu.ActivateMenu(false, true);
            else
                saveSlotsPanelMenu.ActivateMenu(true, false);
        }
    }
    /// <summary>
    /// 開啟存檔選單
    /// </summary>
    public void OpenSaveLoadMenu()
    {
        GameObject saveLoadMenuObj = Instantiate(saveLoadMenuPrefab, canvasParent);
        saveLoadMenu = saveLoadMenuObj.GetComponent<SaveLoadMenu>();

        saveLoadMenu.ActiveteMenu(
            //按下存檔
            () =>
            {
                OpenSaveSlot(true);
            },
            //按下讀取
            () =>
            {
                OpenSaveSlot(false);
            }
            );
    }
}

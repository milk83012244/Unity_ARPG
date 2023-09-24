using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// �s���I
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
    /// �ͦ��s�ɼѵ��
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
    /// �}�Ҧs�ɿ��
    /// </summary>
    public void OpenSaveLoadMenu()
    {
        GameObject saveLoadMenuObj = Instantiate(saveLoadMenuPrefab, canvasParent);
        saveLoadMenu = saveLoadMenuObj.GetComponent<SaveLoadMenu>();

        saveLoadMenu.ActiveteMenu(
            //���U�s��
            () =>
            {
                OpenSaveSlot(true);
            },
            //���UŪ��
            () =>
            {
                OpenSaveSlot(false);
            }
            );
    }
}

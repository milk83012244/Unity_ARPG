using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �D���歶��
/// </summary>
public class InventoryTabGroup : MonoBehaviour
{
    public List<InventoryTabButton> tabButtons;
    public List<GameObject> objectToSwap;

    #region ���ҹϤ����A
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    #endregion

    public InventoryTabButton selectedTab;

    /// <summary>
    /// �q�\���Ҩƥ�
    /// </summary>
    public void Subscrible(InventoryTabButton tabButton)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<InventoryTabButton>();
        }

        tabButtons.Add(tabButton);
    }
    /// <summary>
    /// ���жi�J����
    /// </summary>
    public void OnTabEnter(InventoryTabButton tabButton)
    {
        ResetTabs();
        if (selectedTab == null || tabButton != selectedTab)
        {
            tabButton.background.sprite = tabHover;
        }
    }
    /// <summary>
    /// �������}����
    /// </summary>
    public void OnTabExit(InventoryTabButton tabButton)
    {
        ResetTabs();
    }
    /// <summary>
    /// ��ܭ���
    /// </summary>
    public void OnTabSelected(InventoryTabButton tabButton)
    {
        if (selectedTab != null)
        {
            selectedTab.Deselect();
        }

        selectedTab = tabButton;
        selectedTab.Select();

        ResetTabs();
        tabButton.background.sprite = tabActive;
        int index = tabButton.transform.GetSiblingIndex();
        for (int i = 0; i < objectToSwap.Count; i++)
        {
            if (i == index)
            {
                objectToSwap[i].SetActive(true);
            }
            else
            {
                objectToSwap[i].SetActive(false);
            }
        }
    }
    /// <summary>
    /// ���m����
    /// </summary>
    public void ResetTabs()
    {
        foreach (InventoryTabButton tabButton in tabButtons)
        {
            if (selectedTab != null && tabButton == selectedTab) { continue; }
            tabButton.background.sprite = tabIdle;
        }
    }
}

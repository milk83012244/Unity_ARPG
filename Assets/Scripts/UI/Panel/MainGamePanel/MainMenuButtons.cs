using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    [SerializeField]private GameObject partyPanelPrefab;
    private GameObject partyPanelInstance;

    public Transform partyPanelParent;

    public Button partyButton;

    private void Awake()
    {
        partyButton.onClick.AddListener(PartyButtonClick);
    }
    public void PartyButtonClick()
    {
        if (partyPanelInstance == null)
        {
            partyPanelInstance = Instantiate(partyPanelPrefab, partyPanelParent);
        }
        else
        {
            partyPanelInstance.SetActive(true);
        }
    }
}

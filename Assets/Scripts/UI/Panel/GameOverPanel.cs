using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public Image panel;
    public CanvasGroup UICanvasGroup;
    public Button loadBtn;
    public Button backStartScene;

    public GameObject saveSlotsPanelPrefab;
    [HideInInspector] public GameObject saveSlotsPanelInstance;

    [SerializeField] private GameObject saveLoadMenuPrefab;

    [HideInInspector] public SaveSlotsPanelMenu saveSlotsPanelMenu;

    public Transform canvasParent;
    private void OnEnable()
    {
        StartPanelFadeInCor();
    }
    private void Start()
    {
        loadBtn.onClick.AddListener(InstantiateLoadSlotPanel);
        backStartScene.onClick.AddListener(BackToStartMenu);
    }
    /// <summary>
    /// 生成存檔槽菜單
    /// </summary>
    public void InstantiateLoadSlotPanel()
    {
        if (saveSlotsPanelInstance == null)
            saveSlotsPanelInstance = Instantiate(saveSlotsPanelPrefab, canvasParent);

        if (saveSlotsPanelInstance != null)
        {
            saveSlotsPanelMenu = saveSlotsPanelInstance.GetComponent<SaveSlotsPanelMenu>();
            saveSlotsPanelMenu.ActivateMenu(true, false);
        }
    }
    public void BackToStartMenu()
    {
        SceneManager.LoadScene("StartScene");
    }
    public void StartPanelFadeInCor()
    {
        StartCoroutine(PanelFadeIn());
    }
    private IEnumerator PanelFadeIn()
    {
        panel.DOFade(0.8f, 2f);
        yield return Yielders.GetWaitForSeconds(2f);
        UICanvasGroup.DOFade(1, 1f);
        yield return Yielders.GetWaitForSeconds(1f);
    }
}

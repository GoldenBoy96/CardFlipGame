using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject menuUI;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject replayUI;
    [SerializeField] GameObject levelEditorUI;
    List<GameObject> ScreenUIGroup = new();

    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject loseUI;
    List<GameObject> PopUpUIGroup = new();


    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GroupUI();
    }

    void GroupUI()
    {
        ScreenUIGroup = new()
        {
            menuUI,
            gameUI,
            replayUI, 
            levelEditorUI
        };

        PopUpUIGroup = new()
        {
            pauseUI,
            winUI,
            loseUI,
        };
    }
    
    private void OpenUI(GameObject ui)
    {
        //if (ui.activeSelf == true) { return; }
        if (ScreenUIGroup.Contains(ui))
        {
            foreach (GameObject go in ScreenUIGroup)
            {
                go.SetActive(false);
            }
            ui.SetActive(true);
        }
        if (PopUpUIGroup.Contains(ui))
        {
            Debug.Log("PopUpUIGroup.Contains(ui) " + ui);
            foreach (GameObject go in PopUpUIGroup)
            {
                go.SetActive(false);
            }
            ui.SetActive(true);
        }
    }

    public void OpenMenuUI()
    {
        OpenUI(menuUI);
    }
    public void OpenGameUI()
    {
        OpenUI(gameUI);
    }
    public void OpenReplayUI()
    {
        OpenUI(replayUI);
    }
    public void OpenLevelEditorUI()
    {
        OpenUI(levelEditorUI);
    }
    public void OpenPauseUI()
    {
        OpenUI(pauseUI);
    }
    public void OpenWinUI()
    {
        OpenUI(winUI);
    }
    public void OpenLoseUI()
    {
        OpenUI(loseUI);
    }

    public void CloseSelf(GameObject UIToClose)
    {
        UIToClose.SetActive(false);
    }

    public void ToggleUI(GameObject UIToToggle)
    {
        UIToToggle.SetActive(!UIToToggle.activeSelf);
    }
}

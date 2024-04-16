using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public int unlockLevel;
    public int selectLevel;

    public Button[] stageSelectButton;

    public GameObject levelSelectPanel;

    private void Awake()
    {
        Singleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*LevelCheck();*/
    }



    // Update is called once per frame
    void Update()
    {
        test();
    }

    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void SelectLevel(int sceneIndex)
    {
        levelSelectPanel.SetActive(false);

        selectLevel = sceneIndex;

        SceneManager.LoadScene(sceneIndex);
    }

    public void LevelCheck()
    {

        unlockLevel = PlayerPrefs.GetInt("stageLevel", 1);



        for (int i = 0; i < stageSelectButton.Length; i++)
        {
            if (unlockLevel <= i)
            {
                stageSelectButton[i].interactable = false;
            }
            else
            {
                stageSelectButton[i].interactable = true;
            }
        }
    }

    public void OpenPanel()
    {
        if (PauseBtn.isPaused != true)
        {
            levelSelectPanel.SetActive(true);
        }
    }

    public void ClosePanel()
    {
        if (PauseBtn.isPaused != true)
        {
            levelSelectPanel.SetActive(false);
        }
    }

    public void test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelCheck();
        }
    }
}



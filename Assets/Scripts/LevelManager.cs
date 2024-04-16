using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public AudioClip buttonClip; //버튼 클릭시 효과음.

    public int unlockLevel;
    public int selectLevel;

    public Button[] stageSelectButton;

    public GameObject levelSelectCanvas;

    AudioSource audioSource;

    private void Awake()
    {
        Singleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();  
        InitData();
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
        levelSelectCanvas.SetActive(false);

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

    public void OpenCanvas()
    {
        if (PauseBtn.isPaused != true)
        {
            levelSelectCanvas.SetActive(true);
        }
    }

    public void CloseCanvas()
    {
        if (PauseBtn.isPaused != true)
        {
            audioSource.PlayOneShot(buttonClip);
            levelSelectCanvas.SetActive(false);
        }
    }

    public void test()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LevelCheck();
        }
    }

    void InitData()
    {
        // 처음 게임을 시작했는지 확인하는 UserData 라는 키가 저장 되어있는지 확인합니다.
        // 해당 키가 없다면 각 스테이지에 최단기록을 저장하는 키에 기본값을 넣어주고 UserData키를 저장합니다.
        if (PlayerPrefs.HasKey("UserData") == false)
        {
            Debug.Log("첫 시작 유저");            

            PlayerPrefs.SetFloat("Stage1", 60f);
            PlayerPrefs.SetFloat("Stage2", 60f);
            PlayerPrefs.SetFloat("Stage3", 60f);
            PlayerPrefs.SetFloat("Stage4", 60f);

            PlayerPrefs.SetString("UserData", "record");
        }
    }
}



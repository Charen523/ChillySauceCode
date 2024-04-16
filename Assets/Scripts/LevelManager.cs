using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public AudioClip buttonClip; //��ư Ŭ���� ȿ����.

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
        // ó�� ������ �����ߴ��� Ȯ���ϴ� UserData ��� Ű�� ���� �Ǿ��ִ��� Ȯ���մϴ�.
        // �ش� Ű�� ���ٸ� �� ���������� �ִܱ���� �����ϴ� Ű�� �⺻���� �־��ְ� UserDataŰ�� �����մϴ�.
        if (PlayerPrefs.HasKey("UserData") == false)
        {
            Debug.Log("ù ���� ����");            

            PlayerPrefs.SetFloat("Stage1", 60f);
            PlayerPrefs.SetFloat("Stage2", 60f);
            PlayerPrefs.SetFloat("Stage3", 60f);
            PlayerPrefs.SetFloat("Stage4", 60f);

            PlayerPrefs.SetString("UserData", "record");
        }
    }
}



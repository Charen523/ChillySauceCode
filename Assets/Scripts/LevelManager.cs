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

    public GameObject levelSelectCanvas;

    

    private void Awake()
    {
        Singleton();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);
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

            PlayerPrefs.SetFloat("Stage1_Time", 0);
            PlayerPrefs.SetFloat("Stage2_Time", 0);
            PlayerPrefs.SetFloat("Stage3_Time", 0);
            PlayerPrefs.SetFloat("Stage4_Time", 0);

            PlayerPrefs.SetInt("Stage1_Score", 0);
            PlayerPrefs.SetInt("Stage2_Score", 0);
            PlayerPrefs.SetInt("Stage3_Score", 0);
            PlayerPrefs.SetInt("Stage4_Score", 0);

            PlayerPrefs.SetString("UserData", "record");
                    }
    }
}



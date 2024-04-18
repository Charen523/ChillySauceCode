using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject levelSelectCanvas; //���� ����â

    public int unlockLevel; //������� �رݵ� ����
    public int selectLevel; //���÷���

    public Button[] stageSelectButton; //�������� ���� ��ư �迭

    private void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        // ó�� ������ �����ߴ��� Ȯ���ϴ� UserData ��� Ű�� ���� �Ǿ��ִ��� Ȯ���մϴ�.
        // �ش� Ű�� ���ٸ� �� ���������� �ִܱ���� �����ϴ� Ű�� �⺻���� �־��ְ� UserDataŰ�� �����մϴ�.
        if (PlayerPrefs.HasKey("UserData") == false)
        {
            /*�ű����� �ð� �ʱ�ȭ*/
            PlayerPrefs.SetFloat("Stage1_Time", 0);
            PlayerPrefs.SetFloat("Stage2_Time", 0);
            PlayerPrefs.SetFloat("Stage3_Time", 0);
            PlayerPrefs.SetFloat("Stage4_Time", 0);

            /*�ű����� ���� �ʱ�ȭ*/
            PlayerPrefs.SetInt("Stage1_Score", 0);
            PlayerPrefs.SetInt("Stage2_Score", 0);
            PlayerPrefs.SetInt("Stage3_Score", 0);
            PlayerPrefs.SetInt("Stage4_Score", 0);

            PlayerPrefs.SetString("UserData", "record"); //�ű� ���� ����
        }
    }

    /*�������� ��ư Ŭ�� ��*/
    public void SelectLevel(int sceneIndex)
    {
        levelSelectCanvas.SetActive(false); //�������� ���� �ǳ� ��Ȱ��ȭ
        selectLevel = sceneIndex; //���� ���� �ε��� �Ҵ�
        SceneManager.LoadScene(sceneIndex); //�������� ������ �̵�
    }

    /*�ر� ������ ���� ���� ��ư Ȱ��ȭ/��Ȱ��ȭ*/
    public void LevelCheck()
    {
        unlockLevel = PlayerPrefs.GetInt("stageLevel", 1); //�رݷ��� Ű�� ������ Ȯ���ϰ� ������ Ű����, ������ 1�� ��ȯ

        for (int i = 0; i < stageSelectButton.Length; i++)
        {
            if (unlockLevel <= i)
                stageSelectButton[i].interactable = false; //���� ��ư ��Ȱ��ȭ
            else
                stageSelectButton[i].interactable = true; //���� ��ư Ȱ��ȭ
        }
    }

    /*��ŸƮ ��ư ��ȣ�ۿ� ��*/
    public void OpenCanvas()
    {
        if (PauseBtn.isPaused != true)
            levelSelectCanvas.SetActive(true); //����ĵ���� Ȱ��ȭ
    }

    /*X ��ư ��ȣ�ۿ� ��*/
    public void CloseCanvas()
    {
        if (PauseBtn.isPaused != true)
        {
            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //��ư ȿ����
            levelSelectCanvas.SetActive(false); //����ĵ���� Ȱ��ȭ

            //��ŸƮ ������ ���� ���� ȭ���� ������ �ٽ� ��ŸƮ ��ư�� �����̵��� ��
            StartBtn.isStartBtnPushed = false;
        }
    }
}
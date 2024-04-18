using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject levelSelectCanvas; //레벨 선택창

    public int unlockLevel; //현재까지 해금된 레벨
    public int selectLevel; //선택레벨

    public Button[] stageSelectButton; //스테이지 선택 버튼 배열

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
        // 처음 게임을 시작했는지 확인하는 UserData 라는 키가 저장 되어있는지 확인합니다.
        // 해당 키가 없다면 각 스테이지에 최단기록을 저장하는 키에 기본값을 넣어주고 UserData키를 저장합니다.
        if (PlayerPrefs.HasKey("UserData") == false)
        {
            /*신규유저 시간 초기화*/
            PlayerPrefs.SetFloat("Stage1_Time", 0);
            PlayerPrefs.SetFloat("Stage2_Time", 0);
            PlayerPrefs.SetFloat("Stage3_Time", 0);
            PlayerPrefs.SetFloat("Stage4_Time", 0);

            /*신규유저 점수 초기화*/
            PlayerPrefs.SetInt("Stage1_Score", 0);
            PlayerPrefs.SetInt("Stage2_Score", 0);
            PlayerPrefs.SetInt("Stage3_Score", 0);
            PlayerPrefs.SetInt("Stage4_Score", 0);

            PlayerPrefs.SetString("UserData", "record"); //신규 유저 감별
        }
    }

    /*스테이지 버튼 클릭 시*/
    public void SelectLevel(int sceneIndex)
    {
        levelSelectCanvas.SetActive(false); //스테이지 선택 판넬 비활성화
        selectLevel = sceneIndex; //선택 레벨 인덱스 할당
        SceneManager.LoadScene(sceneIndex); //스테이지 씬으로 이동
    }

    /*해금 레벨에 따른 레벨 버튼 활성화/비활성화*/
    public void LevelCheck()
    {
        unlockLevel = PlayerPrefs.GetInt("stageLevel", 1); //해금레벨 키값 유무를 확인하고 있으면 키값을, 없으면 1을 반환

        for (int i = 0; i < stageSelectButton.Length; i++)
        {
            if (unlockLevel <= i)
                stageSelectButton[i].interactable = false; //레벨 버튼 비활성화
            else
                stageSelectButton[i].interactable = true; //레벨 버튼 활성화
        }
    }

    /*스타트 버튼 상호작용 시*/
    public void OpenCanvas()
    {
        if (PauseBtn.isPaused != true)
            levelSelectCanvas.SetActive(true); //레벨캔버스 활성화
    }

    /*X 버튼 상호작용 시*/
    public void CloseCanvas()
    {
        if (PauseBtn.isPaused != true)
        {
            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //버튼 효과음
            levelSelectCanvas.SetActive(false); //레벨캔버스 활성화

            //스타트 씬에서 레벨 선택 화면이 꺼지면 다시 스타트 버튼이 깜빡이도록 함
            StartBtn.isStartBtnPushed = false;
        }
    }
}
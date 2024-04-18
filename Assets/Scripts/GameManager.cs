using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("카드")]
    public Card firstCard; //GameManger가 들고있을 첫번째 카드 변수
    public Card secondCard; //GameManager가 들고있을 두번째 카드 변수

    [Header("방해오브젝트")]
    public GameObject Explosion; //2레벨~ 방해물: 실패시 화면 살짝 가리는 효과.
    public GameObject Fireball; //4레벨 방해물: 하늘에서 내리는 메테오.

    [Header("애니메이터")]
    public Animator tryBoxAnim; //TryBox를 움직이는 데에 쓰일 Animator.
    public Animator bonusTimeAnim; //Bonus Time Text에 쓰일 Animator.

    [Header("이미지 UI")]
    public Image matchPanel; //짝 맞추기 결과 Panel.
    public Image timeFull; //시간이 줄어드는 것을 보여줄 막대 UI.

    [Header("텍스트 UI")]
    public Text matchText; //짝을 맞췄을 때 나올 Text.
    public Text tryText; //뒤집기 시도한 횟수를 보여줄 Text.
    public Text timeText; //남은 시간을 표시하는 Text.
    public Text endText; //게임이 끝났음을 표시하는 Text. 버튼의 역할도 함.(ReTry 함수)
    public Text scoreText;  // 점수를 표시할 Text.
    public Text bonusTimeText;  // 보너스 & 페널티 시간 Text.
    public Text bestScoreText; //최고점수 Text.
    public Text bestTimeText; //최단시간 Text.

    public bool isMatching; // 현재 카드가 매칭 중인지 나타내는 변수

    /*시간 관련 변수*/
    float startTime = 60f; //스테이지 당 총 시간을 저장하는 변수.
    float bgmChangeTime = 10f; //bgm이 변하는 시간을 저장하는 변수.

    /*MatchPanel 배경색*/
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //실패시 이미지 배경색.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//성공시 이미지 배경색.
    Color WaitingColor = new Color(1, 1, 1); //평상시 이미지 배경색.


    /*인게임 변수*/
    int cardCount; //카드 갯수를 저장하는 변수
    int tryNum = 0; //매칭 시도 횟수를 저장하는 변수.
    float time; //현재 시간을 저장하는 변수.
    int score; // 현재 점수를 저장하는 변수

    bool changeMusic = false; // BGM이 변경 여부 나타내는 변수

    /*레코드 변수*/
    int bestScore; // 최고 점수를 저장하는 변수
    float bestTime; // 최단 시간을 저장하는 변수

    /*LevelManager 참조용*/
    int selectLevel = LevelManager.Instance.selectLevel; //LevelManager에서 받아오는 현재 레벨 변수.
    int unlockLevel = LevelManager.Instance.unlockLevel; //LevelManager에서 받아오는 플레이어의 해금 레벨 변수.

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        Time.timeScale = 1f;
    }

    void Start()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[4]); //게임시작 효과음.

        /*기존 데이터 불러오기*/
        bestScore = PlayerPrefs.GetInt("Stage" + selectLevel + "_Score"); // 레지스트리에 저장되있는 현재 스테이지의 최고 점수 할당
        bestScoreText.text = "최고점수: " + bestScore.ToString(); // 최고 점수 UI에 텍스트 할당
        bestTime = PlayerPrefs.GetFloat("Stage" + selectLevel + "_Time"); // 레지스트리에 저장되있는 현재 스테이지의 최단 기록 할당
        bestTimeText.text = "최단기록: " + bestTime.ToString("N2"); // 최단 기록 UI에 텍스트 할당
        
        /*게임시작 시 초기화할 데이터*/
        isMatching = false; // 매칭 상태 bool 초기화
        cardCount = Board.cardArrayLength; //현재 레벨에 따른 카드 개수 불러오기.
        time = startTime; // 타이머 초기화
        timeFull.fillAmount = 1; // 시간 막대 UI 초기화
        MatchInvoke(); //Match사인 초기화.
        tryBoxAnim.SetBool("IsOver", false); //시도횟수 애니메이션 막기.

        /*난이도 변경*/
        startTime = startTime - 5 * (LevelManager.Instance.selectLevel - 1); //난이도에 따라 게임 시간 변경.
        bgmChangeTime = startTime / 5; // BGM 교체 시간을 총 게임 시간에 따라 변경.

        if (selectLevel == 4) //Fireball 생성
        {
            InvokeRepeating("FireballAppear", 0f, 0.2f);
        }
    }
   
    void Update()
    {
        /*시간 UI 변경*/
        timeText.text = time.ToString("N2"); // 타이머 UI에 텍스트 할당
        TextColorUpdate(); //시간이 줄어듦에 따라 시간텍스트 붉어짐.

        /*제한 시간 임박 시*/
        if (time < bgmChangeTime) 
        {
            BGMChange();
        }

        /*게임 진행 중*/
        if (cardCount > 0)
        {
            if (time > 0)
            {
                time -= Time.deltaTime; // 타이머 감소
                timeFull.fillAmount = time / startTime; //timeFull 이미지가 시간에 비례해 줄어듦
            }
            else time = 0f;
        }
        else StartCoroutine("EndGame"); //게임 클리어 시

        /*시간 초과 시*/
        if (time <= 0) 
        {
            endText.gameObject.SetActive(true); //EndPanel UI 활성화
            tryBoxAnim.SetBool("IsOver", true); //시도 UI 애니메이션 움직임 재생
            Invoke("EndTimeInoke", 2.0f); //Animation 재생 기다리기
        }
    }

    // 카드를 매칭하는 메서드
    public void Matched()
    {
        /*시도 횟수*/
        tryNum++; //시도횟수 추가
        tryText.text = tryNum.ToString(); //시도횟수 출력

        if (firstCard.idx == secondCard.idx) //매칭성공
        {
            matchPanel.color = SuccessColor; //매치판넬을 초록색으로 변경

            /*매치 텍스트를 idx에 따라 적절한 사람 이름으로 변경.*/
            switch (firstCard.idx % 4)
            {
                case 0:
                    matchText.text = "김영선!";
                    break;
                case 1:
                    matchText.text = "박재균!";
                    break;
                case 2:
                    matchText.text = "이승영!";
                    break;
                case 3:
                    matchText.text = "정승연!";
                    break;
                default:
                    Debug.Log("인덱스 오류.");
                    break;
            }

            /*매칭 성공한 카드 파괴*/
            firstCard.DestroyCard(); 
            secondCard.DestroyCard();
            cardCount -= 2; // 카드 갯수 감소

            BonusTime(); //보너스 시간
            Invoke("matchSoundInvoke", 1f); //성공 효과음 재생
        }
        else //매칭실패
        {
            matchPanel.color = FailColor; //매치판넬을 붉은색으로 변경
            matchText.text = "실패..."; //매치 텍스트를 실패로 변경
            Invoke("failSoundInvoke", 1f); //실패 효과음 재생

            /*레벨에 따른 방해요소 추가(폭발)*/
            if (selectLevel >= 2)
            {
                for (int i = 0; i < selectLevel - 1; i++)
                {
                    Invoke("ExplosionAppear", 1.0f);
                    Invoke("ExplosionAppear", 1.2f);
                    Invoke("ExplosionAppear", 1.4f);
                }
            }

            /*카드 원상복구*/
            firstCard.CloseCard();
            secondCard.CloseCard();
            TimePenalty();
        }

        Invoke("MatchInvoke", 1f); //1초 후 매치판넬 대기 상태로 복귀
        
        /*카드 데이터 초기화*/
        firstCard = null;
        secondCard = null;
    }

    /*EndButton 클릭시*/
    public void ReTry()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //끝 버튼 효과음
        Time.timeScale = 1f; //시간흐름 복구

        /*기존 배경음악으로 복구*/
        AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[0]; 
        AudioManager.Instance.audioSource[0].Play();

        StartBtn.isStartBtnPushed = false; //스타트버튼 깜빡임 다시 시작되도록 값 변경

        /*스타트씬으로 회귀*/
        SceneManager.LoadScene("StartScene");
    }

    /*카드를 맞췄을 때 나올 효과음 지연함수.*/
    void matchSoundInvoke()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[3]);
    }

    /*카드를 틀렸을 때 나올 효과음 지연함수.*/
    void failSoundInvoke()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[1]);
    }

    /*게임 클리어시 게임을 끝내는 메서드*/
    IEnumerator EndGame()
    {
        /*새 스테이지 해금.*/
        if (unlockLevel <= selectLevel)
        {
            PlayerPrefs.SetInt("stageLevel", selectLevel + 1);
        }

        /*점수*/
        score = (int)(time * 100f) - 10 * tryNum; //점수 계산.
        scoreText.text = score.ToString(); //점수 UI에 점수 계산식의 결과값의 텍스트를 할당
        
        /*기록갱신 확인*/
        if (score > bestScore) // 현재 점수가 최고 점수보다 높은가?
        {
            PlayerPrefs.SetInt("Stage" + selectLevel + "_Score", score); // 최고 점수 레지스트리에 현재 점수 할당
        }
        if (time > bestTime) // 현재 남은 시간이 최단 기록보다 높은가? (더 빨리 클리어 했는가?)
        {
            PlayerPrefs.SetFloat("Stage" + selectLevel + "_Time", time); // 최단 기록 레지스트리에 현재 남은 시간 할당
        }

        yield return new WaitForSeconds(1f); //카드 뒤집는 시간 대기
        
        endText.gameObject.SetActive(true); //엔드텍스트 활성화.
        tryBoxAnim.SetBool("IsOver", true); //시도 UI 애니메이션 움직임 재생.
        
        yield return new WaitForSeconds(2f); //애니메이션 재생 시간 대기
        
        Time.timeScale = 0f; // 시간 정지
    }

    /*시간이 지남에 따라 타이머 UI의 색깔을 업데이트 시켜주는 메서드*/
    void TextColorUpdate()
    {
        float textColor = time / startTime; // 시간이 지남에 따라 서서히 감소함
        timeText.color = new Color(1f, textColor, textColor); // 시간이 지남에 따라 흰색 -> 빨강으로 변화함
    }

    /*BGM을 변경하는 메서드*/
    void BGMChange()
    {
        if (changeMusic == false) // BGM이 변경된 적이 있는가?
        {
            changeMusic = true; // BGM이 변경되었다고 설정

            AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[1]; // BGM clip의 두번째 BGM 할당.
            AudioManager.Instance.audioSource[0].Play(); // 해당 클립 재생
        }
    }

    /*Match 사인을 초기상태로 돌리는 함수.*/
    void MatchInvoke()
    {
        matchPanel.color = WaitingColor; //배경색을 회색으로 변경.
        matchText.text = "화이팅!"; //문구를 화이팅으로 변경.
    }

    /*게임종료 시 TimeScale 지연시키는 함수.*/
    void EndTimeInoke()
    {
        Time.timeScale = 0;
    }

    /*보너스 타임을 부여하는 메서드*/
    void BonusTime()
    {
        time += 1.5f; // 현재 시간에 보너스 부여
        BonusPenaltyTime(1.5f);
    }

    /*페널티 타임을 부여하는 메서드*/
    public void TimePenalty()
    {
        time -= 1f; //현재 시간에 페널티 부여
        BonusPenaltyTime(-1f);
    }

    /*보너스 / 페널티 타임에 해당하는 UI의 애니메이션 재생 & 텍스트, 색상을 바꿔주는 메서드*/
    public void BonusPenaltyTime(float time)
    {
        bonusTimeAnim.SetTrigger("isBonusPenaltyTime"); // UI의 애니메이션 트리거 발동
        if (time == 1.5f) // 보너스 타임에 지정된 값인가?
        {
            bonusTimeText.text = "+" + time.ToString() + "sec"; // 텍스트 할당
            bonusTimeText.color = new Color(115 / 255f, 205 / 255f, 255 / 255f); // 색상 변경
        }   
        else if(time == -1f) // 페널티 타임에 지정된 값인가?
        {
            bonusTimeText.text = time.ToString() + "sec"; // 텍스트 할당
            bonusTimeText.color = new Color(255 / 255f, 76 / 255f, 84 / 255f); //색상 변경
        }
    }

    void ExplosionAppear()
    {
        Instantiate(Explosion); //폭발 오브젝트 생성
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[5]); //폭발 효과음.
    }

    void FireballAppear()
    {
        Instantiate(Fireball); //파이어볼 오브젝트 생성
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[6]); //폭발 효과음.
    }
}
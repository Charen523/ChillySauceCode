using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    /*Animator 선언*/
    public Animator tryBoxAnim; //TryBox를 움직이는 데에 쓰일 예정.
    public Animator bonusTimeAnim; //Bonus Time Text에 쓰일 Animator

    /*UI 선언*/
    public Image matchPanel; //짝을 맞췄을 때 나올 판넬.
    public Image timeFull; //시간이 줄어드는 것을 보여줄 UI.
    public Text matchText; //짝을 맞췄을 때 나올 text
    public Text tryText; //뒤집기 시도한 횟수를 보여줄 text
    public Text timeText;
    public Text endText;
    public Text scoreText;  // 점수를 표시할 text
    public Text bonusTimeText;  // 보너스 & 페널티 시간 text
    public Text bestScoreText;
    public Text bestTimeText;

    // 게임 진행중 카드 선택 시 카드 오브젝트가 저장될 변수.
    public Card firstCard;
    public Card secondCard;

    float time;
    public float startTime = 60f;
    public float bgmChangeTime = 10f;

    public int cardCount;

    public bool isMatching;

    public int tryNum = 0; //카드 뒤집기를 시도한 횟수를 저장하는 변수.

    

    //Match사인 배경색
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //실패시 이미지 배경색.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//성공시 이미지 배경색.
    Color WaitingColor = new Color(1, 1, 1); //평상시 이비지 배경색.

    bool changeMusic = false;

    int score;

    public int bestScore;

    public float bestTime;

    int selectLevel;
    int unlockLevel;

    public void Awake()
    {
        Singleton();
        Time.timeScale = 1f;
        cardCount = 16;
    }
    // Start is called before the first frame update
    void Start()
    {
        

        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[4]);

        tryBoxAnim.SetBool("IsOver", false);
        isMatching = false;
        startTime = startTime - 5 * (LevelManager.Instance.selectLevel - 1); //난이도에 따라 게임시간 변경.
        time = startTime; //

        timeFull.fillAmount = 1;

        Invoke("MatchInvoke", 0f); //Match사인 초기화.

        unlockLevel = LevelManager.Instance.unlockLevel;
        selectLevel = LevelManager.Instance.selectLevel;

        bestScore = PlayerPrefs.GetInt("Stage" + selectLevel + "_Score");
        bestTime = PlayerPrefs.GetFloat("Stage" + selectLevel + "_Time");

        bestScoreText.text = "최고점수: " + bestScore.ToString();
        bestTimeText.text = "최단기록: " + bestTime.ToString("N2");
    }

    // Update is called once per frame
    void Update()
    {

        if (time > 0)
        {
            TextColorUpdate();
        }

        if (time < bgmChangeTime)
        {
            BGMChange();
        }

        if (cardCount > 0)
        {
            if (time <= 0) 
                time = 0f;
            else
            {
                time -= Time.deltaTime;
                timeFull.fillAmount = time / startTime; //timeFull 이미지가 시간에 비례해 줄어듦.
            }

            timeText.text = time.ToString("N2");
        }
        else
        {
            StartCoroutine("EndGame");
        }
        if (time <= 0)
        {
            endText.gameObject.SetActive(true);
            tryBoxAnim.SetBool("IsOver", true); //시도 UI 애니메이션 움직임 재생.
            Debug.Log("지연중");
            Invoke("EndTimeInoke", 2.5f);

        }
    }

    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Matched()
    {
        tryNum++; //시도횟수 카운트.
        tryText.text = tryNum.ToString(); //시도횟수 출력.

        if (firstCard.idx == secondCard.idx) //카드가 일치하는 경우.
        {
            matchPanel.color = SuccessColor; //초록색으로 변경.

            /*idx에 따라 적절한 사람 이름으로 변경.*/
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

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            BonusTime();
            Invoke("matchSoundInvoke", 1f); //성공시 효과음 재생.

            cardCount -= 2;
        }
        else
        {
            matchPanel.color = FailColor; //매치판넬을 붉은색으로 변경.
            matchText.text = "실패..."; //매치텍스트를 실패로 변경.
            Invoke("failSoundInvoke", 1f); //실패시 효과음 재생.

            /*실패시 카드 원래대로 뒤집기*/
            firstCard.CloseCard();
            secondCard.CloseCard();
            TimePenalty();
        }

        Invoke("MatchInvoke", 1f); //1초 후 대기 상태로 복귀.
        firstCard = null;
        secondCard = null;
    }

    public void ReTry()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);


        Time.timeScale = 1f;

        AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[0];
        AudioManager.Instance.audioSource[0].Play();
        Board.isCardGenerated = false;
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

    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(1f); //카드 뒤집는 시간동안 지연.

        endText.gameObject.SetActive(true); //엔드텍스트 활성화.
        tryBoxAnim.SetBool("IsOver", true); //시도 UI 애니메이션 움직임 재생.

        yield return new WaitForSecondsRealtime(2f); //애니메이션 시간동안 지연.

        scoreText.text = ((int)(time * 100f) - 10 * tryNum).ToString();
        Time.timeScale = 0f;

        if (unlockLevel <= selectLevel)
        {
            PlayerPrefs.SetInt("stageLevel", selectLevel + 1);
        }

        score = (int)(time * 100f) - 10 * tryNum;

        if (score > bestScore)
        {
            PlayerPrefs.SetInt("Stage" + selectLevel + "_Score", score);
        }

        if (time > bestTime)
        {
            PlayerPrefs.SetFloat("Stage" + selectLevel + "_Time", time);
        }
    }

    void TextColorUpdate()
    {
        float textColor = time / startTime;

        timeText.color = new Color(1f, textColor, textColor);
    }


    void BGMChange()
    {
        if (changeMusic == false)
        {
            changeMusic = true;

            AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[1];
            AudioManager.Instance.audioSource[0].Play();
        }
    }

    //Match 사인을 초기상태로 돌리는 함수.
    void MatchInvoke()
    {
        matchPanel.color = WaitingColor; //배경색을 회식으로 변경.
        matchText.text = "화이팅!"; //문구를 화이팅으로 변경.
    }

    //게임종료 시 TimeScale 지연시키는 함수.
    void EndTimeInoke()
    {
        Time.timeScale = 0;
    }

    void BonusTime()
    {
        time += 1.5f;
        BonusPenaltyTime(1.5f);
    }

    public void TimePenalty()
    {
        time -= 1f;
        BonusPenaltyTime(-1f);
    }

    public void BonusPenaltyTime(float time)
    {
        bonusTimeAnim.SetTrigger("isBonusPenaltyTime");
        if (time == 1.5f)
        {
            bonusTimeText.text = "+" + time.ToString() + "sec";
            bonusTimeText.color = new Color(115 / 255f, 205 / 255f, 255 / 255f);
        }   
        else if(time == -1f)
        {
            bonusTimeText.text = time.ToString() + "sec";
            bonusTimeText.color = new Color(255 / 255f, 76 / 255f, 84 / 255f);
        }
    }
}

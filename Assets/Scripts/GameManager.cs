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
    public AudioClip clip;
    public Animator anim; //TryBox를 움직이는 데에 쓰일 예정.

    /*UI 선언*/
    public Image matchPanel; //짝을 맞췄을 때 나올 배경
    public Text matchTxt; //짝을 맞췄을 때 나올 text
    public Text tryTxt; //뒤집기 시도한 횟수를 보여줄 text
    public Text timetext;
    public Text endText;
    public Text scoreText;  // 점수를 표시할 text

    AudioSource audioSource;

    float time;
    public float startTime = 60f;
    public float bgmChangeTime = 10f;

    public int cardCount;

    public bool isMatching;

    public int tryNum = 0; //카드 뒤집기를 시도한 횟수를 저장하는 변수.

    // 게임 진행중 카드 선택 시 카드 오브젝트가 저장될 변수.
    public Card firstCard;
    public Card secondCard;

    //Match사인 배경색
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //실패시 이미지 배경색.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//성공시 이미지 배경색.
    Color WaitingColor = new Color(190 / 255f, 190 / 255f, 190 / 255f); //평상시 이비지 배경색.

    bool changeMusic = false;

    public void Awake()
    {
        Singleton();
        Time.timeScale = 1f;
        cardCount = 16;
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        anim.SetBool("IsOver", false);
        isMatching = false;
        time = startTime;

        Invoke("MatchInvoke", 0f); //Match사인 초기화.
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
            time -= Time.deltaTime;

            timetext.text = time.ToString("N2");
        }
        else
        {
            anim.SetBool("IsOver", true);
            StartCoroutine("EndGame");
        }
        if (time <= 0)
        {
            endText.gameObject.SetActive(true);
            anim.SetBool("IsOver", true);
            Debug.Log("지연중");
            Invoke("EndTimeInoke", 2f);
            
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
        tryTxt.text = tryNum.ToString(); //시도횟수 출력.

        if (firstCard.idx == secondCard.idx) //카드가 일치하는 경우.
        {
            matchPanel.color = SuccessColor; //초록색으로 변경.

            /*idx에 따라 적절한 사람 이름으로 변경.*/
            switch (firstCard.idx % 4)
            {
                case 0:
                    matchTxt.text = "김영선!";
                    break;
                case 1:
                    matchTxt.text = "박재균!";
                    break;
                case 2:
                    matchTxt.text = "이승영!";
                    break;
                case 3:
                    matchTxt.text = "정승연!";
                    break;
                default:
                    Debug.Log("인덱스 오류.");
                    break;
            }

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            Invoke("SoundInvoke", 1f);

            cardCount -= 2;
        }
        else
        {
            matchPanel.color = FailColor; //매치판넬을 붉은색으로 변경.
            matchTxt.text = "실패..."; //매치텍스트를 실패로 변경.

            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        Invoke("MatchInvoke", 1f); //1초 후 대기 상태로 복귀.
        firstCard = null;
        secondCard = null;
    }

    public void ReTry()
    {
        AudioManager.Instance.audioSource.clip = AudioManager.Instance.clips[0];
        AudioManager.Instance.audioSource.Play();

        SceneManager.LoadScene("StartScene");
    }

    void SoundInvoke()
    {
        audioSource.PlayOneShot(clip);
    }


    IEnumerator EndGame()
    {
        int unlockLevel = LevelManager.Instance.unlockLevel;
        int selectLevel = LevelManager.Instance.selectLevel;

        if ( unlockLevel <= selectLevel)
        {
            PlayerPrefs.SetInt("stageLevel", selectLevel + 1);
        }

        yield return new WaitForSecondsRealtime(1f);
        endText.gameObject.SetActive(true);
        scoreText.text = ((int)(time * 100f) - 10 * tryNum).ToString();
        Time.timeScale = 0f;
    }

    void TextColorUpdate()
    {
        float textColor = time / startTime;

        timetext.color = new Color(1f, textColor, textColor);
    }


    void BGMChange()
    {
        if (changeMusic == false)
        {
            changeMusic = true;

            AudioManager.Instance.audioSource.clip = AudioManager.Instance.clips[1];
            AudioManager.Instance.audioSource.Play();
        }
    }

    //Match 사인을 초기상태로 돌리는 함수.
    void MatchInvoke()
    {
        matchPanel.color = WaitingColor; //배경색을 회식으로 변경.
        matchTxt.text = "화이팅!"; //문구를 화이팅으로 변경.
    }

    //게임종료 시 TimeScale 지연시키는 함수.
    void EndTimeInoke()
    {
        Time.timeScale = 0;
    }
}

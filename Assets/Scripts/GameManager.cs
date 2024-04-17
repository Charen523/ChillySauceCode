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
    
    /*Animator ����*/
    public Animator tryBoxAnim; //TryBox�� �����̴� ���� ���� ����.
    public Animator bonusTimeAnim; //Bonus Time Text�� ���� Animator

    /*UI ����*/
    public Image matchPanel; //¦�� ������ �� ���� �ǳ�.
    public Image timeFull; //�ð��� �پ��� ���� ������ UI.
    public Text matchText; //¦�� ������ �� ���� text
    public Text tryText; //������ �õ��� Ƚ���� ������ text
    public Text timeText;
    public Text endText;
    public Text scoreText;  // ������ ǥ���� text
    public Text bonusTimeText;  // ���ʽ� & ���Ƽ �ð� text
    public Text bestScoreText;
    public Text bestTimeText;

    // ���� ������ ī�� ���� �� ī�� ������Ʈ�� ����� ����.
    public Card firstCard;
    public Card secondCard;

    float time;
    public float startTime = 60f;
    public float bgmChangeTime = 10f;

    public int cardCount;

    public bool isMatching;

    public int tryNum = 0; //ī�� �����⸦ �õ��� Ƚ���� �����ϴ� ����.

    

    //Match���� ����
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //���н� �̹��� ����.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//������ �̹��� ����.
    Color WaitingColor = new Color(1, 1, 1); //���� �̺��� ����.

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
        startTime = startTime - 5 * (LevelManager.Instance.selectLevel - 1); //���̵��� ���� ���ӽð� ����.
        time = startTime; //

        timeFull.fillAmount = 1;

        Invoke("MatchInvoke", 0f); //Match���� �ʱ�ȭ.

        unlockLevel = LevelManager.Instance.unlockLevel;
        selectLevel = LevelManager.Instance.selectLevel;

        bestScore = PlayerPrefs.GetInt("Stage" + selectLevel + "_Score");
        bestTime = PlayerPrefs.GetFloat("Stage" + selectLevel + "_Time");

        bestScoreText.text = "�ְ�����: " + bestScore.ToString();
        bestTimeText.text = "�ִܱ��: " + bestTime.ToString("N2");
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
                timeFull.fillAmount = time / startTime; //timeFull �̹����� �ð��� ����� �پ��.
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
            tryBoxAnim.SetBool("IsOver", true); //�õ� UI �ִϸ��̼� ������ ���.
            Debug.Log("������");
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
        tryNum++; //�õ�Ƚ�� ī��Ʈ.
        tryText.text = tryNum.ToString(); //�õ�Ƚ�� ���.

        if (firstCard.idx == secondCard.idx) //ī�尡 ��ġ�ϴ� ���.
        {
            matchPanel.color = SuccessColor; //�ʷϻ����� ����.

            /*idx�� ���� ������ ��� �̸����� ����.*/
            switch (firstCard.idx % 4)
            {
                case 0:
                    matchText.text = "�迵��!";
                    break;
                case 1:
                    matchText.text = "�����!";
                    break;
                case 2:
                    matchText.text = "�̽¿�!";
                    break;
                case 3:
                    matchText.text = "���¿�!";
                    break;
                default:
                    Debug.Log("�ε��� ����.");
                    break;
            }

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            BonusTime();
            Invoke("matchSoundInvoke", 1f); //������ ȿ���� ���.

            cardCount -= 2;
        }
        else
        {
            matchPanel.color = FailColor; //��ġ�ǳ��� ���������� ����.
            matchText.text = "����..."; //��ġ�ؽ�Ʈ�� ���з� ����.
            Invoke("failSoundInvoke", 1f); //���н� ȿ���� ���.

            /*���н� ī�� ������� ������*/
            firstCard.CloseCard();
            secondCard.CloseCard();
            TimePenalty();
        }

        Invoke("MatchInvoke", 1f); //1�� �� ��� ���·� ����.
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

    /*ī�带 ������ �� ���� ȿ���� �����Լ�.*/
    void matchSoundInvoke()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[3]);
    }

    /*ī�带 Ʋ���� �� ���� ȿ���� �����Լ�.*/
    void failSoundInvoke()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[1]);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(1f); //ī�� ������ �ð����� ����.

        endText.gameObject.SetActive(true); //�����ؽ�Ʈ Ȱ��ȭ.
        tryBoxAnim.SetBool("IsOver", true); //�õ� UI �ִϸ��̼� ������ ���.

        yield return new WaitForSecondsRealtime(2f); //�ִϸ��̼� �ð����� ����.

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

    //Match ������ �ʱ���·� ������ �Լ�.
    void MatchInvoke()
    {
        matchPanel.color = WaitingColor; //������ ȸ������ ����.
        matchText.text = "ȭ����!"; //������ ȭ�������� ����.
    }

    //�������� �� TimeScale ������Ű�� �Լ�.
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

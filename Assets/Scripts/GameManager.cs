using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    /*Animator ����*/
    public Animator tryBoxAnim; //TryBox�� �����̴� ���� ���� Animator.
    public Animator bonusTimeAnim; //Bonus Time Text�� ���� Animator.


    /*UI ����*/
    public Image matchPanel; //¦ ���߱� ��� Panel.
    public Image timeFull; //�ð��� �پ��� ���� ������ ���� UI.
    public Text matchText; //¦�� ������ �� ���� Text.
    public Text tryText; //������ �õ��� Ƚ���� ������ Text.
    public Text timeText; //���� �ð��� ǥ���ϴ� Text.
    public Text endText; //������ �������� ǥ���ϴ� Text. ��ư�� ���ҵ� ��.(ReTry �Լ�)
    public Text scoreText;  // ������ ǥ���� Text.
    public Text bonusTimeText;  // ���ʽ� & ���Ƽ �ð� Text.
    public Text bestScoreText; //�ְ����� Text.
    public Text bestTimeText; //�ִܽð� Text.

    // ���� ������ ī�� ���� �� ī�� ������Ʈ�� ����� ����.
    public Card firstCard; //GameManger�� ������� ù��° ī�� ����
    public Card secondCard; //GameManager�� ������� �ι�° ī�� ����

    /*�ð� ���� ����*/
    float time; //���� �ð��� �����ϴ� ����.
    float startTime = 60f; //�������� �� �� �ð��� �����ϴ� ����.
    float bgmChangeTime = 10f; //bgm�� ���ϴ� �ð��� �����ϴ� ����.

    int cardCount; //ī�� ������ �����ϴ� ����

    public bool isMatching; // ���� ī�尡 ��Ī ������ ��Ÿ���� ����

    public int tryNum = 0; //ī�� �����⸦ �õ��� Ƚ���� �����ϴ� ����.

    

    //Match���� ����
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //���н� �̹��� ����.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//������ �̹��� ����.
    Color WaitingColor = new Color(1, 1, 1); //���� �̹��� ����.

    bool changeMusic = false; // BGM�� ����Ǿ����� ��Ÿ���� ����

    int score; // ���� ������ �����ϴ� ����

    public int bestScore; // �ְ� ������ �����ϴ� ����

    public float bestTime; // �ִ� �ð��� �����ϴ� ����

    int selectLevel;
    int unlockLevel;

    public void Awake()
    {
        Singleton();
        Time.timeScale = 1f;
    }
    // GameManager �ʱ�ȭ & ����
    void Start()
    {
        cardCount = Board.cardArrayLenght;
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[4]);

        tryBoxAnim.SetBool("IsOver", false);

        isMatching = false; // ��Ī ���� bool �ʱ�ȭ
        startTime = startTime - 5 * (LevelManager.Instance.selectLevel - 1); //���̵��� ���� ���� �ð� ����.
        bgmChangeTime = startTime / 5; // BGM ��ü �ð��� ���� �ð��� ���� ����.
        
        time = startTime; // Ÿ�̸� �ʱ�ȭ

        timeFull.fillAmount = 1; // �ð� ���� UI �ʱ�ȭ

        Invoke("MatchInvoke", 0f); //Match���� �ʱ�ȭ.

        unlockLevel = LevelManager.Instance.unlockLevel;
        selectLevel = LevelManager.Instance.selectLevel;

        bestScore = PlayerPrefs.GetInt("Stage" + selectLevel + "_Score"); // ������Ʈ���� ������ִ� ���� ���������� �ְ� ���� �Ҵ�
        bestTime = PlayerPrefs.GetFloat("Stage" + selectLevel + "_Time"); // ������Ʈ���� ������ִ� ���� ���������� �ִ� ��� �Ҵ�

        bestScoreText.text = "�ְ�����: " + bestScore.ToString(); // �ְ� ���� UI�� �ؽ�Ʈ �Ҵ�
        bestTimeText.text = "�ִܱ��: " + bestTime.ToString("N2"); // �ִ� ��� UI�� �ؽ�Ʈ �Ҵ�
    }

    // Update is called once per frame
    void Update()
    {

        if (time > 0) // ���� �ð��� 0 ���� ���� �� (���� ���� ��)
        {
            TextColorUpdate();
        }

        if (time < bgmChangeTime) // ���� �ð��� BGM ���� �ð��� �������� �� (���� �ð� �ӹ�)
        {
            BGMChange();
        }

        if (cardCount > 0) // ���� ī�� ������ 0���� ���� ��
        {
            if (time <= 0)  // ���� �ð��� 0 ���ϰ� �Ǿ��� ��
                time = 0f; // Ÿ�̸Ӹ� 0���� ����
            else
            {
                time -= Time.deltaTime; // Ÿ�̸� ����
                timeFull.fillAmount = time / startTime; //timeFull �̹����� �ð��� ����� �پ��.
            }

            timeText.text = time.ToString("N2"); // Ÿ�̸� UI�� �ؽ�Ʈ �Ҵ�
        }
        else
        {
            StartCoroutine("EndGame");
        }
        if (time <= 0) // ���� �ð��� 0 ���ϰ� �Ǿ��� ��
        {
            endText.gameObject.SetActive(true); //�� ��ư UI Ȱ��ȭ
            tryBoxAnim.SetBool("IsOver", true); //�õ� UI �ִϸ��̼� ������ ���.
            Debug.Log("������");
            Invoke("EndTimeInoke", 2.0f);

        }
    }
    // �̱��� ȭ
    void Singleton()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // ī�带 ��Ī�ϴ� �޼���
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

            cardCount -= 2; // ī�� ���� ����
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
        Debug.Log("ȣ��");
        firstCard = null; //������ �ʱ�ȭ
        secondCard = null; //������ �ʱ�ȭ
    }

    public void ReTry()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);
        Time.timeScale = 1f;
        AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[0];
        AudioManager.Instance.audioSource[0].Play();
        Board.isCardGenerated = false;
        SceneManager.LoadScene("StartScene");
        //��ŸƮ��ư ������ �ٽ� ���۵ǵ��� �� ����
        StartBtn.isStartBtnPushed = false;
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

    /*���������� ���������� Ŭ������ ��� ������ ������ �޼��� (ī�� ���� < 0 && ���� �ð� > 0)*/
    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(1f); //ī�� ������ �ð����� ����.
        
        endText.gameObject.SetActive(true); //�����ؽ�Ʈ Ȱ��ȭ.
        tryBoxAnim.SetBool("IsOver", true); //�õ� UI �ִϸ��̼� ������ ���.
        scoreText.text = ((int)(time * 100f) - 10 * tryNum).ToString(); //���� UI�� ���� ������ ������� �ؽ�Ʈ�� �Ҵ�
       
        yield return new WaitForSecondsRealtime(2f); //�ִϸ��̼� �ð����� ����.
        
        Time.timeScale = 0f; // �ð� ����
        if (unlockLevel <= selectLevel)
        {
            PlayerPrefs.SetInt("stageLevel", selectLevel + 1);
        }
       
        score = (int)(time * 100f) - 10 * tryNum;
        
        if (score > bestScore) // ���� ������ �ְ� �������� ������?
        {
            PlayerPrefs.SetInt("Stage" + selectLevel + "_Score", score); // �ְ� ���� ������Ʈ���� ���� ���� �Ҵ�
        }
        
        if (time > bestTime) // ���� ���� �ð��� �ִ� ��Ϻ��� ������? (�� ���� Ŭ���� �ߴ°�?)
        {
            PlayerPrefs.SetFloat("Stage" + selectLevel + "_Time", time); // �ִ� ��� ������Ʈ���� ���� ���� �ð� �Ҵ�
        }
    }

    /*�ð��� ������ ���� Ÿ�̸� UI�� ������ ������Ʈ �����ִ� �޼���*/
    void TextColorUpdate()
    {
        float textColor = time / startTime; // �ð��� ������ ���� ������ ������

        timeText.color = new Color(1f, textColor, textColor); // �ð��� ������ ���� ��� -> �������� ��ȭ��
    }

    /*BGM�� �����ϴ� �޼���*/
    void BGMChange()
    {
        if (changeMusic == false) // BGM�� ����� ���� �ִ°�?
        {
            changeMusic = true; // BGM�� ����Ǿ��ٰ� ����

            AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[1]; // BGM clip�� �ι�° BGM �Ҵ�.
            AudioManager.Instance.audioSource[0].Play(); // �ش� Ŭ�� ���
        }
    }

    /*Match ������ �ʱ���·� ������ �Լ�.*/
    void MatchInvoke()
    {
        matchPanel.color = WaitingColor; //������ ȸ������ ����.
        matchText.text = "ȭ����!"; //������ ȭ�������� ����.
    }

    /*�������� �� TimeScale ������Ű�� �Լ�.*/
    void EndTimeInoke()
    {
        Time.timeScale = 0;
    }

    /*���ʽ� Ÿ���� �ο��ϴ� �޼���*/
    void BonusTime()
    {
        time += 1.5f; // ���� �ð��� ���ʽ� �ο�
        BonusPenaltyTime(1.5f);
    }

    /*���Ƽ Ÿ���� �ο��ϴ� �޼���*/
    public void TimePenalty()
    {
        time -= 1f; //���� �ð��� ���Ƽ �ο�
        BonusPenaltyTime(-1f);
    }

    /*���ʽ� / ���Ƽ Ÿ�ӿ� �ش��ϴ� UI�� �ִϸ��̼� ��� & �ؽ�Ʈ, ������ �ٲ��ִ� �޼���*/
    public void BonusPenaltyTime(float time)
    {
        bonusTimeAnim.SetTrigger("isBonusPenaltyTime"); // UI�� �ִϸ��̼� Ʈ���� �ߵ�
        if (time == 1.5f) // ���ʽ� Ÿ�ӿ� ������ ���ΰ�?
        {
            bonusTimeText.text = "+" + time.ToString() + "sec"; // �ؽ�Ʈ �Ҵ�
            bonusTimeText.color = new Color(115 / 255f, 205 / 255f, 255 / 255f); // ���� ����
        }   
        else if(time == -1f) // ���Ƽ Ÿ�ӿ� ������ ���ΰ�?
        {
            bonusTimeText.text = time.ToString() + "sec"; // �ؽ�Ʈ �Ҵ�
            bonusTimeText.color = new Color(255 / 255f, 76 / 255f, 84 / 255f); //���� ����
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("ī��")]
    public Card firstCard; //GameManger�� ������� ù��° ī�� ����
    public Card secondCard; //GameManager�� ������� �ι�° ī�� ����

    [Header("���ؿ�����Ʈ")]
    public GameObject Explosion; //2����~ ���ع�: ���н� ȭ�� ��¦ ������ ȿ��.
    public GameObject Fireball; //4���� ���ع�: �ϴÿ��� ������ ���׿�.

    [Header("�ִϸ�����")]
    public Animator tryBoxAnim; //TryBox�� �����̴� ���� ���� Animator.
    public Animator bonusTimeAnim; //Bonus Time Text�� ���� Animator.

    [Header("�̹��� UI")]
    public Image matchPanel; //¦ ���߱� ��� Panel.
    public Image timeFull; //�ð��� �پ��� ���� ������ ���� UI.

    [Header("�ؽ�Ʈ UI")]
    public Text matchText; //¦�� ������ �� ���� Text.
    public Text tryText; //������ �õ��� Ƚ���� ������ Text.
    public Text timeText; //���� �ð��� ǥ���ϴ� Text.
    public Text endText; //������ �������� ǥ���ϴ� Text. ��ư�� ���ҵ� ��.(ReTry �Լ�)
    public Text scoreText;  // ������ ǥ���� Text.
    public Text bonusTimeText;  // ���ʽ� & ���Ƽ �ð� Text.
    public Text bestScoreText; //�ְ����� Text.
    public Text bestTimeText; //�ִܽð� Text.

    public bool isMatching; // ���� ī�尡 ��Ī ������ ��Ÿ���� ����

    /*�ð� ���� ����*/
    float startTime = 60f; //�������� �� �� �ð��� �����ϴ� ����.
    float bgmChangeTime = 10f; //bgm�� ���ϴ� �ð��� �����ϴ� ����.

    /*MatchPanel ����*/
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //���н� �̹��� ����.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//������ �̹��� ����.
    Color WaitingColor = new Color(1, 1, 1); //���� �̹��� ����.


    /*�ΰ��� ����*/
    int cardCount; //ī�� ������ �����ϴ� ����
    int tryNum = 0; //��Ī �õ� Ƚ���� �����ϴ� ����.
    float time; //���� �ð��� �����ϴ� ����.
    int score; // ���� ������ �����ϴ� ����

    bool changeMusic = false; // BGM�� ���� ���� ��Ÿ���� ����

    /*���ڵ� ����*/
    int bestScore; // �ְ� ������ �����ϴ� ����
    float bestTime; // �ִ� �ð��� �����ϴ� ����

    /*LevelManager ������*/
    int selectLevel = LevelManager.Instance.selectLevel; //LevelManager���� �޾ƿ��� ���� ���� ����.
    int unlockLevel = LevelManager.Instance.unlockLevel; //LevelManager���� �޾ƿ��� �÷��̾��� �ر� ���� ����.

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
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[4]); //���ӽ��� ȿ����.

        /*���� ������ �ҷ�����*/
        bestScore = PlayerPrefs.GetInt("Stage" + selectLevel + "_Score"); // ������Ʈ���� ������ִ� ���� ���������� �ְ� ���� �Ҵ�
        bestScoreText.text = "�ְ�����: " + bestScore.ToString(); // �ְ� ���� UI�� �ؽ�Ʈ �Ҵ�
        bestTime = PlayerPrefs.GetFloat("Stage" + selectLevel + "_Time"); // ������Ʈ���� ������ִ� ���� ���������� �ִ� ��� �Ҵ�
        bestTimeText.text = "�ִܱ��: " + bestTime.ToString("N2"); // �ִ� ��� UI�� �ؽ�Ʈ �Ҵ�
        
        /*���ӽ��� �� �ʱ�ȭ�� ������*/
        isMatching = false; // ��Ī ���� bool �ʱ�ȭ
        cardCount = Board.cardArrayLength; //���� ������ ���� ī�� ���� �ҷ�����.
        time = startTime; // Ÿ�̸� �ʱ�ȭ
        timeFull.fillAmount = 1; // �ð� ���� UI �ʱ�ȭ
        MatchInvoke(); //Match���� �ʱ�ȭ.
        tryBoxAnim.SetBool("IsOver", false); //�õ�Ƚ�� �ִϸ��̼� ����.

        /*���̵� ����*/
        startTime = startTime - 5 * (LevelManager.Instance.selectLevel - 1); //���̵��� ���� ���� �ð� ����.
        bgmChangeTime = startTime / 5; // BGM ��ü �ð��� �� ���� �ð��� ���� ����.

        if (selectLevel == 4) //Fireball ����
        {
            InvokeRepeating("FireballAppear", 0f, 0.2f);
        }
    }
   
    void Update()
    {
        /*�ð� UI ����*/
        timeText.text = time.ToString("N2"); // Ÿ�̸� UI�� �ؽ�Ʈ �Ҵ�
        TextColorUpdate(); //�ð��� �پ�꿡 ���� �ð��ؽ�Ʈ �Ӿ���.

        /*���� �ð� �ӹ� ��*/
        if (time < bgmChangeTime) 
        {
            BGMChange();
        }

        /*���� ���� ��*/
        if (cardCount > 0)
        {
            if (time > 0)
            {
                time -= Time.deltaTime; // Ÿ�̸� ����
                timeFull.fillAmount = time / startTime; //timeFull �̹����� �ð��� ����� �پ��
            }
            else time = 0f;
        }
        else StartCoroutine("EndGame"); //���� Ŭ���� ��

        /*�ð� �ʰ� ��*/
        if (time <= 0) 
        {
            endText.gameObject.SetActive(true); //EndPanel UI Ȱ��ȭ
            tryBoxAnim.SetBool("IsOver", true); //�õ� UI �ִϸ��̼� ������ ���
            Invoke("EndTimeInoke", 2.0f); //Animation ��� ��ٸ���
        }
    }

    // ī�带 ��Ī�ϴ� �޼���
    public void Matched()
    {
        /*�õ� Ƚ��*/
        tryNum++; //�õ�Ƚ�� �߰�
        tryText.text = tryNum.ToString(); //�õ�Ƚ�� ���

        if (firstCard.idx == secondCard.idx) //��Ī����
        {
            matchPanel.color = SuccessColor; //��ġ�ǳ��� �ʷϻ����� ����

            /*��ġ �ؽ�Ʈ�� idx�� ���� ������ ��� �̸����� ����.*/
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

            /*��Ī ������ ī�� �ı�*/
            firstCard.DestroyCard(); 
            secondCard.DestroyCard();
            cardCount -= 2; // ī�� ���� ����

            BonusTime(); //���ʽ� �ð�
            Invoke("matchSoundInvoke", 1f); //���� ȿ���� ���
        }
        else //��Ī����
        {
            matchPanel.color = FailColor; //��ġ�ǳ��� ���������� ����
            matchText.text = "����..."; //��ġ �ؽ�Ʈ�� ���з� ����
            Invoke("failSoundInvoke", 1f); //���� ȿ���� ���

            /*������ ���� ���ؿ�� �߰�(����)*/
            if (selectLevel >= 2)
            {
                for (int i = 0; i < selectLevel - 1; i++)
                {
                    Invoke("ExplosionAppear", 1.0f);
                    Invoke("ExplosionAppear", 1.2f);
                    Invoke("ExplosionAppear", 1.4f);
                }
            }

            /*ī�� ���󺹱�*/
            firstCard.CloseCard();
            secondCard.CloseCard();
            TimePenalty();
        }

        Invoke("MatchInvoke", 1f); //1�� �� ��ġ�ǳ� ��� ���·� ����
        
        /*ī�� ������ �ʱ�ȭ*/
        firstCard = null;
        secondCard = null;
    }

    /*EndButton Ŭ����*/
    public void ReTry()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //�� ��ư ȿ����
        Time.timeScale = 1f; //�ð��帧 ����

        /*���� ����������� ����*/
        AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[0]; 
        AudioManager.Instance.audioSource[0].Play();

        StartBtn.isStartBtnPushed = false; //��ŸƮ��ư ������ �ٽ� ���۵ǵ��� �� ����

        /*��ŸƮ������ ȸ��*/
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

    /*���� Ŭ����� ������ ������ �޼���*/
    IEnumerator EndGame()
    {
        /*�� �������� �ر�.*/
        if (unlockLevel <= selectLevel)
        {
            PlayerPrefs.SetInt("stageLevel", selectLevel + 1);
        }

        /*����*/
        score = (int)(time * 100f) - 10 * tryNum; //���� ���.
        scoreText.text = score.ToString(); //���� UI�� ���� ������ ������� �ؽ�Ʈ�� �Ҵ�
        
        /*��ϰ��� Ȯ��*/
        if (score > bestScore) // ���� ������ �ְ� �������� ������?
        {
            PlayerPrefs.SetInt("Stage" + selectLevel + "_Score", score); // �ְ� ���� ������Ʈ���� ���� ���� �Ҵ�
        }
        if (time > bestTime) // ���� ���� �ð��� �ִ� ��Ϻ��� ������? (�� ���� Ŭ���� �ߴ°�?)
        {
            PlayerPrefs.SetFloat("Stage" + selectLevel + "_Time", time); // �ִ� ��� ������Ʈ���� ���� ���� �ð� �Ҵ�
        }

        yield return new WaitForSeconds(1f); //ī�� ������ �ð� ���
        
        endText.gameObject.SetActive(true); //�����ؽ�Ʈ Ȱ��ȭ.
        tryBoxAnim.SetBool("IsOver", true); //�õ� UI �ִϸ��̼� ������ ���.
        
        yield return new WaitForSeconds(2f); //�ִϸ��̼� ��� �ð� ���
        
        Time.timeScale = 0f; // �ð� ����
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

    void ExplosionAppear()
    {
        Instantiate(Explosion); //���� ������Ʈ ����
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[5]); //���� ȿ����.
    }

    void FireballAppear()
    {
        Instantiate(Fireball); //���̾ ������Ʈ ����
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[6]); //���� ȿ����.
    }
}
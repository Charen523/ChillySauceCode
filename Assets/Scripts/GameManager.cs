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
    public Animator anim; //TryBox�� �����̴� ���� ���� ����.

    /*UI ����*/
    public Image matchPanel; //¦�� ������ �� ���� ���
    public Text matchTxt; //¦�� ������ �� ���� text
    public Text tryTxt; //������ �õ��� Ƚ���� ������ text
    public Text timetext;
    public Text endText;
    public Text scoreText;  // ������ ǥ���� text

    AudioSource audioSource;

    float time;
    public float startTime = 60f;
    public float bgmChangeTime = 10f;

    public int cardCount;

    public bool isMatching;

    public int tryNum = 0; //ī�� �����⸦ �õ��� Ƚ���� �����ϴ� ����.

    // ���� ������ ī�� ���� �� ī�� ������Ʈ�� ����� ����.
    public Card firstCard;
    public Card secondCard;

    //Match���� ����
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //���н� �̹��� ����.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//������ �̹��� ����.
    Color WaitingColor = new Color(190 / 255f, 190 / 255f, 190 / 255f); //���� �̺��� ����.

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

        Invoke("MatchInvoke", 0f); //Match���� �ʱ�ȭ.
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
            Debug.Log("������");
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
        tryNum++; //�õ�Ƚ�� ī��Ʈ.
        tryTxt.text = tryNum.ToString(); //�õ�Ƚ�� ���.

        if (firstCard.idx == secondCard.idx) //ī�尡 ��ġ�ϴ� ���.
        {
            matchPanel.color = SuccessColor; //�ʷϻ����� ����.

            /*idx�� ���� ������ ��� �̸����� ����.*/
            switch (firstCard.idx % 4)
            {
                case 0:
                    matchTxt.text = "�迵��!";
                    break;
                case 1:
                    matchTxt.text = "�����!";
                    break;
                case 2:
                    matchTxt.text = "�̽¿�!";
                    break;
                case 3:
                    matchTxt.text = "���¿�!";
                    break;
                default:
                    Debug.Log("�ε��� ����.");
                    break;
            }

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            Invoke("SoundInvoke", 1f);

            cardCount -= 2;
        }
        else
        {
            matchPanel.color = FailColor; //��ġ�ǳ��� ���������� ����.
            matchTxt.text = "����..."; //��ġ�ؽ�Ʈ�� ���з� ����.

            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        Invoke("MatchInvoke", 1f); //1�� �� ��� ���·� ����.
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

    //Match ������ �ʱ���·� ������ �Լ�.
    void MatchInvoke()
    {
        matchPanel.color = WaitingColor; //������ ȸ������ ����.
        matchTxt.text = "ȭ����!"; //������ ȭ�������� ����.
    }

    //�������� �� TimeScale ������Ű�� �Լ�.
    void EndTimeInoke()
    {
        Time.timeScale = 0;
    }
}

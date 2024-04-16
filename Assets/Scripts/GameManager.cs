using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioClip clip;
    public Image matchPanel; //짝을 맞췄을 때 나올 배경
    public Text matchTxt; //짝을 맞췄을 때 나올 text

    AudioSource audioSource;

    public Text timetext;
    public Text endText;

    float time;
    public float startTime = 60f;
    public float bgmChangeTime = 10f;

    public int cardCount;

    public bool isMatching;

    // 추후 카드 오브젝트가 완성되면 넣어준다.
    public Card firstCard;
    public Card secondCard;

    //Match 배경색
    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //실패시 이미지 배경색.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//성공시 이미지 배경색.


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
        
        isMatching = false;

        time = startTime;

        matchPanel.color = FailColor;
        matchTxt.text = "실패...";
        matchPanel.enabled = false;
        matchTxt.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (time > 0)
        {
            TextColorUpdate();            
        }               

        if ( time < bgmChangeTime)
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
            StartCoroutine("EndGame");
        }
        if (time <= 0)
        {
            Time.timeScale = 0;
            endText.gameObject.SetActive(true);
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
        if ( firstCard.idx == secondCard.idx)
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
            matchPanel.enabled = true; //판넬 보이게 하기.
            matchTxt.enabled = true;

            firstCard.DestroyCard();
            secondCard.DestroyCard();
            Invoke("SoundInvoke", 1f);

            cardCount -= 2;
        }
        else
        {
            matchPanel.color = FailColor;
            matchTxt.text = "실패...";
            matchPanel.enabled = true;
            matchTxt.enabled = true;

            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        Invoke("MatchInvoke", 1f);
        firstCard = null;
        secondCard = null;
    }

    public void ReTry()
    {
        AudioManager.Instance.audioSource.clip = AudioManager.Instance.clips[0];
        AudioManager.Instance.audioSource.Play();

        SceneManager.LoadScene("MainScene");
    }

    void SoundInvoke()
    {
        audioSource.PlayOneShot(clip);
    }

    
    IEnumerator EndGame()
    {
        yield return new WaitForSecondsRealtime(1f);
        endText.gameObject.SetActive(true);
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

    void MatchInvoke()
    {
        matchPanel.enabled = false;
        matchTxt.enabled = false;
    }
}

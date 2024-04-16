using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public AudioClip clip;

    AudioSource audioSource;

    public Text timetext;
    public Text endText;

    float time;
    float startTime = 60f;


    public int cardCount;

    public bool isMatching;

    // 추후 카드 오브젝트가 완성되면 넣어준다.
    public Card firstCard;
    public Card secondCard;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            TextColorUpdate();            
        }               

        if ( time < 10)
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
            firstCard.DestroyCard();
            secondCard.DestroyCard();
            Invoke("SoundInvoke", 1f);

            cardCount -= 2;
        }
        else
        {
            firstCard.CloseCard();
            secondCard.CloseCard();
        }

        firstCard = null;
        secondCard = null;
    }

    public void ReTry()
    {
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
       /* if (changeMusic == false)
        {
            changeMusic = true;

            AudioManager.Instance.audioSource.clip = this.clip;
        }*/

        
    }
}

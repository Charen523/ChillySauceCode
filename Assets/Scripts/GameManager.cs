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

    float time = 0;
    float endTime = 10f;

    public int cardCount;

    public bool isMatching;

    // ���� ī�� ������Ʈ�� �ϼ��Ǹ� �־��ش�.
    public Card firstCard;
    public Card secondCard;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (cardCount > 0)
        {
            time += Time.deltaTime;

            timetext.text = time.ToString("N2");
        }
        else
        {
            Time.timeScale = 0f;
            endText.gameObject.SetActive(true);
        }
        /*if ( time > endTime)
        {
            Time.timeScale = 0;
            endText.gameObject.SetActive(true);
        }*/
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

            if ( cardCount == 0)
            {
                Time.timeScale = 0;
                endText.gameObject.SetActive(true);
            }
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
   
}

using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Text timetext;
    public Text endText;

    float time = 0;
    float endTime = 10f;

    public int cardCount;

    

    // 추후 카드 오브젝트가 완성되면 넣어준다.
    public Card firstCard;
    public Card secondCard;

    public void Awake()
    {
        Singleton();
    }
    // Start is called before the first frame update
    void Start()
    {
       Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        timetext.text = time.ToString("N2");

        if ( time > endTime)
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
            firstCard.DestoryCard();
            secondCard.DestoryCard();

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

    }
   
}

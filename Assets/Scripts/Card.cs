using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public AudioClip clip;

    public SpriteRenderer frontImg;
    public GameObject front;
    public GameObject back;
    public Animator anim;
    public Image backImg;

    public int idx;

    public bool isCardOpened;
    public GameObject backBtn;

    bool isCardDark;
    AudioSource audioSource;
    // Start is called before the first frame update

    private void Awake()
    {
        isCardDark = false;
    }

    void Start()
    {
        
        
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (isCardDark) backImg.color = new Color(0.8f, 0.8f, 0.8f);

        
        

    }
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}");
    }
    public void CardClick()
    {
        if (!GameManager.Instance.isMatching)
        {
            audioSource.PlayOneShot(clip);

            anim.SetBool("isOpen", true);
            front.SetActive(true);
            back.SetActive(false);
            isCardOpened = true;
            if (GameManager.Instance.firstCard == null) GameManager.Instance.firstCard = this;
            else
            {
                GameManager.Instance.secondCard = this;
                GameManager.Instance.Matched();
                GameManager.Instance.isMatching = true;
            }
        }
    }

    public void DestroyCard()
    {
        StartCoroutine("DestroyCardCoroutine");
    }

    public void CloseCard()
    {
        StartCoroutine("CloseCardCoroutine");
        
        if (isCardOpened)
        {
            changeColor();
        }
    }

    public IEnumerator DestroyCardCoroutine()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        GameManager.Instance.isMatching = false;
    }

    public IEnumerator CloseCardCoroutine()
    {
        yield return new WaitForSeconds(1f);
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
        
        GameManager.Instance.isMatching = false;
        isCardDark = true;
    }


    void changeColor()
    {
        backBtn.GetComponentInChildren<Image>().color = Color.gray;
    }

}

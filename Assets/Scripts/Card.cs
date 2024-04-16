using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
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

    bool onClick; // ù��° ī�尡 ������ �� ����.
    float time; // ī�尡 ���� ä�� �帥 �ð�.

    AudioSource audioSource;
    // Start is called before the first frame update

    void Start()
    {

        onClick = false;
        audioSource = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.firstCard != null)
        {
            if(onClick)
            {
                time += Time.deltaTime;
            }
            //if (time > 4f)
            //{
            //    GameManager.Instance.firstCard.CloseCard();
            //    GameManager.Instance.firstCard = null;           
            //}
        }
    }
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}");
    }
    public void CardClick()
    {
        // ī�� ����� �Ϸ���� �ʾҰų� �Ͻ����� �����̰ų� �̹� �����ִ� ī���� ��� Ŭ���� ����
        if (!Board.isCardGenerated || PauseBtn.isPaused || GameManager.Instance.isMatching)
        {
            return;
        }
        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);
        isCardOpened = true;
    
        if (GameManager.Instance.firstCard == null)
        {
            onClick = true;
            GameManager.Instance.firstCard = this;
        }
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();
            GameManager.Instance.isMatching = true;
        }
        audioSource.PlayOneShot(clip);
    }

    public void DestroyCard()
    {
        onClick = false;
        time = 0f;
        StartCoroutine("DestroyCardCoroutine");
    }

    public void CloseCard()
    {
        onClick = false;
        time = 0f;
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
    }


    void changeColor()
    {
        backBtn.GetComponentInChildren<Image>().color = Color.gray;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public AudioClip clip;

    public SpriteRenderer frontImg;
    public GameObject front;
    public GameObject back;
    public Animator anim;

    public int idx;

    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}");
    }
    public void CardClick()
    {
        anim.SetBool("isOpen", true);
        front.SetActive(true);
        back.SetActive(false);
        if (GameManager.Instance.firstCard == null) GameManager.Instance.firstCard = this;
        else
        {
            GameManager.Instance.secondCard = this;
            GameManager.Instance.Matched();
        }
    }

    public void DestroyCard()
    {
        StartCoroutine("DestroyCardCoroutine");
    }

    public void CloseCard()
    {
        StartCoroutine("CloseCardCoroutine");
    }

    public IEnumerator DestroyCardCoroutine()
    {
        Debug.Log("CloseCard 호출");
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    public IEnumerator CloseCardCoroutine()
    {
        Debug.Log("DestoryCard 호출");
        yield return new WaitForSeconds(1f);
        anim.SetBool("isOpen", false);
        front.SetActive(false);
        back.SetActive(true);
    }


}

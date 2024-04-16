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
    

    public int idx;

    public bool isCardOpened;
    public GameObject backBtn;

    bool onClick; // 첫번째 카드가 눌렸을 때 켜짐.
    float time; // 카드가 열린 채로 흐른 시간.

    AudioSource audioSource;
    // Start is called before the first frame update

    //카드 넘어가는 속도와 leanTween ease 타입변수입니다.
    public float m_Speed = 0.2f;
    public LeanTweenType leanTweenType;


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
            if (time > 4f)
            {
                GameManager.Instance.firstCard.CloseCard();
                GameManager.Instance.firstCard = null;
                Invoke("TimePenalty", 1f);
            }
        }
    }
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}");
    }
    public void CardClick()
    {
        
       if(Board.isCardGenerated)
        { 
            if (!GameManager.Instance.isMatching)
            {
                if (!PauseBtn.isPaused)
                {
                    RotateCard();
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
            }   
    }
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
        ReverseCard();

        GameManager.Instance.isMatching = false;
    }


    void changeColor()
    {
        backBtn.GetComponentInChildren<Image>().color = Color.gray;
    }

    void TimePenalty()
    {
        GameManager.Instance.TimePenalty();
    }


    public void RotateCard()
    {
        if (LeanTween.isTweening(gameObject))
            return;

        var rot = Mathf.Round(transform.localRotation.y) == 0f ? 180f : 0f;
        LeanTween.rotateY(gameObject, rot, m_Speed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(m_Speed / 2, () => ShowFront())
            .setEase(leanTweenType);
    }
    public void ReverseCard()
    {
        if (LeanTween.isTweening(gameObject))
            return;

        var rot = Mathf.Round(transform.localRotation.y) == 0f ? 180f : 0f;
        LeanTween.rotateY(gameObject, rot, m_Speed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(m_Speed / 2, () => ShowBack())
            .setEase(leanTweenType);
    }

    protected void ShowFront()
    {
        transform.Find("Back")?.gameObject.SetActive(false);
        transform.Find("Front")?.gameObject.SetActive(true);
        Debug.Log("사진");
    }

    protected void ShowBack()
    {
        transform.Find("Front")?.gameObject.SetActive(false);
        transform.Find("Back")?.gameObject.SetActive(true);
        Debug.Log("스파르타");
    }

    protected void ScaleCard(GameObject go, Vector3 scale) => LeanTween.scale(go, scale, m_Speed);


}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public AudioClip clip; // 카드 뒤집을 때의 효과음

    public SpriteRenderer frontImg; // 카드 앞면 스프라이트

    public int idx; // 카드 앞면을 구분하는 번호

    public bool isCardOpened; // 카드가 뒤집어졌는지를 나타내는 변수
    public GameObject backBtn; // 카드가 눌릴 뒷면 버튼

    bool onClick; // 첫번째 카드가 눌렸을 때 켜짐.
    float time; // 카드가 열린 채로 흐른 시간.

    // Start is called before the first frame update

    //카드 넘어가는 속도와 leanTween ease 타입변수입니다.
    public float m_Speed = 0.2f;
    public LeanTweenType leanTweenType;

    /*데이터 초기화*/
    void Start()
    {
        onClick = false;
    }

    void Update()
    {
        if (GameManager.Instance.firstCard != null) // 첫번째 카드가 null이 아닌가? (데이터가 들어왔는가?)
        {
            if (onClick) // 카드가 클릭되었는가?
            {
                time += Time.deltaTime; // 시간 증가
            }
            if (time > 4f) // 시간이 4초 이상 지났는가?
            {
                GameManager.Instance.firstCard.CloseCard(); // 첫번째 카드 닫기
                GameManager.Instance.firstCard = null; // 첫번째 카드 데이터 초기화
                Invoke("TimePenalty", 1f);
            }
        }
    }

    /*카드 앞면을 세팅하는 메서드*/
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}"); // 카드 앞면 스프라이트에 Resources/Images에서 해당하는 이미지를 로드
    }

    /*카드가 클릭되었을 때 호출되는 메서드*/
    public void CardClick()
    {
       if(Board.isCardGenerated) // 카드개 배부되었는가?
        { 
            if (!GameManager.Instance.isMatching) // 카드가 매칭중이 아닌가?
            {
                if (!PauseBtn.isPaused) // 지금 일시정지 상태가 아닌가?
                {
                    RotateCard();
                    isCardOpened = true; // 카드가 한번 열렸다.

                    if (GameManager.Instance.firstCard == null) // GameManager의 첫번째 카드가 null인가? (데이터가 없는가?)
                    {
                        onClick = true; // 카드가 클릭되었다.
                        GameManager.Instance.firstCard = this; // GameManager의 첫번째 카드에 현재 gameObject를 할당
                    }
                    else // 아닌가? (GameManager의 첫번째 카드에 데이터가 들어있는가?)
                    {
                        GameManager.Instance.secondCard = this; // GameManager의 두번째 카드에 현재 gameObject를 할당
                        GameManager.Instance.Matched();
                        GameManager.Instance.isMatching = true; // 현재 매칭중이다.
                    }
                    AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
                }
            }   
       }
    }

    /*카드를 파괴하는 메서드*/
    public void DestroyCard()
    {
        onClick = false; // 카드는 현재 클릭된 상태가 아니다.
        time = 0f; // 카드 대기 시간 초기화
        StartCoroutine("DestroyCardCoroutine");
    }

    /*카드를 다시 닫는 메서드*/
    public void CloseCard()
    {
        onClick = false; // 카드는 현재 클릭된 상태가 아니다.
        time = 0f; // 카드 대기 시간 초기화
        StartCoroutine("CloseCardCoroutine");

        if (isCardOpened) // 카드가 한번 열린적이 있는가?
        {
            changeColor();
        }
    }

    /*카드를 파괴하는 코루틴*/
    public IEnumerator DestroyCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        Destroy(gameObject); // 해당 gameObject 파괴
        GameManager.Instance.isMatching = false; // 카드는 현재 매칭중이 아니다.
        
    }

    /*카드를 다시 닫는 코루틴*/
    public IEnumerator CloseCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        ReverseCard();

        GameManager.Instance.isMatching = false; // 카드는 현재 매칭중이 아니다.
    }

    /*카드 뒷면의 색상을 어둡게 하는 메서드*/
    void changeColor()
    {
        backBtn.GetComponentInChildren<Image>().color = Color.gray; // 카드 뒷면의 Image의 색상을 회색으로 설정 (약간 어두워짐)
    }

    /*카드를 뒤집은 후 5초 대기시 페널티 타임을 부여하는 메서드*/
    void TimePenalty()
    {
        GameManager.Instance.TimePenalty();
    }

    /*카드를 여는 메서드*/
    public void RotateCard()
    {
        var rot = Mathf.Round(transform.localRotation.y) ==0f ? 180f : 0f;
        if (rot < 0f) rot = 0f;
        LeanTween.rotateY(gameObject, rot, m_Speed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(m_Speed / 2, () => ShowFront())
            .setEase(leanTweenType);
    }

    /*카드를 다시 닫는 메서드*/
    public void ReverseCard()
    {
        var rot = Mathf.Round(transform.localRotation.y) == 0f ? 180f : 0f;
               
        LeanTween.rotateY(gameObject, rot, m_Speed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(m_Speed / 2, () => ShowBack())
            .setEase(leanTweenType);
        Debug.Log(rot);
    }

    /*LeanTween 종류 후 카드 앞면을 보여주는 메서드*/
    protected void ShowFront()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(true);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(false);
    }

    /*LeanTween 종류 후 카드 뒷면을 보여주는 메서드*/
    protected void ShowBack()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(false);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(true);
        
    }

    protected void ScaleCard(GameObject go, Vector3 scale) => LeanTween.scale(go, scale, m_Speed);


}

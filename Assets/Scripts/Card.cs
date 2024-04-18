using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject backBtn; // 카드 뒷면(버튼)
    public SpriteRenderer frontImg; // 카드 앞면(스프라이트)

    public int idx; // 카드 앞면 이미지 인덱스

    /*카드 뒤집기 애니메이션 변수*/
    float cardFlipSpeed = 0.2f; //카드 뒤집는 속도
    LeanTweenType leanTweenType; //leanTween ease 타입변수

    bool isCardOpened; // 카드 최초 선택 여부
    bool firstCardOpened; // 첫번째 카드 오픈 여부
    float cardOpenedTime; // 카드가 열린 채로 흐른 시간

    void Start()
    {
        firstCardOpened = false;
    }

    void Update()
    {
        if (firstCardOpened)// 첫번째 카드 오픈 여부
        {
            cardOpenedTime += Time.deltaTime; // 시간 증가

            /*카드를 열고 일정 시간이 지나면 패널티 부여와 함께 첫 번째 카드 닫기*/
            if (cardOpenedTime > 1f) // 카드가 열린 시간 카운트
            {
                GameManager.Instance.firstCard.CloseCard(); // 첫번째 카드 닫기
                GameManager.Instance.firstCard = null; // 첫번째 카드 데이터 초기화
                Invoke("TimePenaltyInvoke", 1f); //1초 후 시간 패널티 부여
            }
        }
    }

    /*카드 앞면 세팅 메서드*/
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}"); // 카드 앞면에 Resources/Images에서 해당하는 이미지를 로드
    }

    /*카드가 클릭 시*/
    public void CardClick()
    {
        if (!GameManager.Instance.isMatching) // 카드가 매칭중 여부
        {
            if (!PauseBtn.isPaused) //일시정지 상태 여부
            {
                RotateCard(); //뒤집기 애니 호출
                isCardOpened = true; // 카드 열림

                if (GameManager.Instance.firstCard == null) // 첫 카드일 때
                {
                    firstCardOpened = true; // 첫번째 카드 할당 확인
                    GameManager.Instance.firstCard = this; // GameManager의 첫번째 카드에 현재 gameObject를 할당
                }
                else // 두 번째 카드일 때
                {
                    GameManager.Instance.secondCard = this; // GameManager의 두번째 카드에 현재 gameObject를 할당
                    GameManager.Instance.Matched(); //매칭 완료
                    GameManager.Instance.isMatching = true; // 매칭중
                }
                AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]); //카드 뒤집기 효과음
            }
        }
    }

    /*카드 파괴 메서드*/
    public void DestroyCard()
    {
        firstCardOpened = false; // 카드는 현재 클릭된 상태가 아니다.
        cardOpenedTime = 0f; // 카드 대기 시간 초기화
        StartCoroutine("DestroyCardCoroutine");
    }

    /*카드 닫기 메서드*/
    public void CloseCard()
    {
        firstCardOpened = false; // 카드는 현재 클릭된 상태가 아니다.
        cardOpenedTime = 0f; // 카드 대기 시간 초기화
        StartCoroutine("CloseCardCoroutine");

        if (isCardOpened) // 카드가 한번 열린적이 있는가?
        {
            changeColor();
        }
    }

    /*카드 파괴 코루틴*/
     IEnumerator DestroyCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        Destroy(gameObject); // 해당 gameObject 파괴
        GameManager.Instance.isMatching = false; // 카드는 현재 매칭중이 아니다.
        
    }

    /*카드 닫기 코루틴*/
     IEnumerator CloseCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1초 대기
        ReverseCard(); //카드 뒤집기
        GameManager.Instance.isMatching = false; // 카드는 현재 매칭중이 아니다.
    }

    /*카드 뒷면의 색상을 어둡게 하는 메서드*/
    void changeColor()
    {
        backBtn.GetComponentInChildren<Image>().color = Color.gray; // 카드 뒷면의 Image의 색상을 회색으로 설정
    }

    /*카드 시간초과시 페널티 부여하는 메서드*/
    void TimePenaltyInvoke()
    {
        GameManager.Instance.TimePenalty();
    }

    /*카드 열기 메서드*/
     void RotateCard()
    {
        var rot = Mathf.Round(transform.localRotation.y) ==0f ? 180f : 0f;

        if (rot < 0f) rot = 0f;
        
        LeanTween.rotateY(gameObject, rot, cardFlipSpeed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(cardFlipSpeed / 2, () => ShowFront())
            .setEase(leanTweenType);
    }

    /*카드 닫기 메서드*/
     void ReverseCard()
    {
        var rot = Mathf.Round(transform.localRotation.y) == 0f ? 180f : 0f;
               
        LeanTween.rotateY(gameObject, rot, cardFlipSpeed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(cardFlipSpeed / 2, () => ShowBack())
            .setEase(leanTweenType);
    }

    /*LeanTween 종류 후 카드 앞면을 보여주는 메서드*/
     void ShowFront()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(true);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(false);
    }

    /*LeanTween 종류 후 카드 뒷면을 보여주는 메서드*/
    void ShowBack()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(false);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(true);
        
    }

    void ScaleCard(GameObject go, Vector3 scale) => LeanTween.scale(go, scale, cardFlipSpeed);
}

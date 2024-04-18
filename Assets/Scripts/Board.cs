using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject card; //카드 오브젝트
       
    public static int cardArrayLength; //카드 배열 길이 변수

    int[] cardArray; //카드를 담을 배열
    float initX, initY, gapX, gapY, scale; // 스테이지마다의 초기 위치, 간격, 크기를 저장할 변수
    int div; // 한 줄에 넣을 카드 갯수

    private void Awake()
    {
        /*레벨에 따라 카드배열 조정*/
        if (LevelManager.Instance.selectLevel >= 3) //3레벨 이상
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };
            div = 6; //가로줄에 들어갈 카드 개수
            initX = 1.82f; //최좌하단 카드 x 위치
            initY = 2.1f; //최좌하단 카드 y 위치
            gapX = 0.72f; //카드 x 간격
            gapY = 1.08f; //카드 y 간격
            scale = 0.75f; //카드 크기
        }
        else //3레벨 미만
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            div = 4; //가로줄에 들어갈 카드 개수
            initX = 1.65f; //최좌하단 카드 x 위치
            initY = 3f; //최좌하단 카드 y 위치
            gapX = 1.1f; //카드 x 간격
            gapY = 1.4f; //카드 y 간격
            scale = 1; //카드 크기
        }
        cardArrayLength = cardArray.Length; // 카드 배열의 길이를 static 변수에 할당
    }

    /*스테이지 시작 시 카드 배부*/
    void Start()
    {
        cardArray = ShuffleArray(cardArray); //카드를 랜덤으로 섞어주기

        for (int i = 0; i < cardArray.Length; i++) // 카드 갯수 만큼 수행
        {
            /*카드를 보드의 가운데에 생성*/
            float x = (i % div) * gapX - initX; // 가로 좌표 지정
            float y = (i / div) * gapY - initY; // 세로 좌표 지정
            GameObject go = Instantiate(card, new Vector2(0, 0), Quaternion.identity, transform); // 카드를 화면 중앙에 소환하고 Board 게임 오브젝트의 자식으로 설정
            go.transform.Find("body").transform.localScale = Vector2.one * scale; // 카드의 크기 조정

            /*카드가 생성 후 목표 위치로 날아가는 애니메이션*/
            LeanTween.move(go, new Vector2(x, y), 0.5f) // 목표 위치까지 이동
                     .setEase(LeanTweenType.easeOutBounce) // 이징 효과 설정
                     .setDelay(i * 0.1f) // 카드마다 딜레이 추가
                     .setOnStart(() => PlayCardMoveSound()); //효과음 재생

            go.GetComponent<Card>().CardSpriteSetting(cardArray[i]); //카드 앞면 이미지 입히기
        }
    }

    /*카드 분배 효과음 함수*/
    void PlayCardMoveSound()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
    }

    /* 카드 리스트를 섞는 메서드 */
    private int[] ShuffleArray(int[] array)
    {
        int random1, random2, temp; // 랜덤 주소를 저장할 변수 2개 & 임시 저장 변수 1개

        for(int i = 0; i < array.Length; i++) //array.Length의 횟수 만큼 반복
        {
            random1 = Random.Range(0, array.Length); // 주소값 랜덤 할당
            random2 = Random.Range(0, array.Length); // 주소값 랜덤 할당
            
            /*random1과 random2값 스왑*/
            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array; // 섞기가 완료된 배열 반환
    }
}
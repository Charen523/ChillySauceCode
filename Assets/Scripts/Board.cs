using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject card; // 카드 프리펩
       
    [SerializeField]
    public static bool isCardGenerated; //카드가 다 생성 되었는지 확인
    public static int cardArrayLenght; //카드 갯수를 넘겨줄 변수

    int[] cardArray; // 스테이지마다 다른 카드 리스트를 담을 배열
    float initX, initY, gapX, gapY, scale; // 스테이지마다 다른 초기 pos값, 간격, 크기값을 저장할 변수
    int div; // 한 줄에 넣을 카드 갯수
    private void Awake()
    {
        if (LevelManager.Instance.selectLevel >= 3) // 스테이지가 3 이상인가? (해당 스테이지에 해당하는 데이터 할당)
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };
            div = 6;
            initX = 1.82f;
            initY = 2.1f;
            gapX = 0.72f;
            gapY = 1.08f;
            scale = 0.75f;
        }
        else // 스테이지가 3 이상이 아닌가? (해당 스테이지에 해당하는 데이터 할당)
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            div = 4;
            initX = 1.65f;
            initY = 3f;
            gapX = 1.1f;
            gapY = 1.4f;
            scale = 1;
        }
        cardArrayLenght = cardArray.Length; // 카드 배열의 길이를 static 변수에 할당
    }
    /*스테이지 시작 시 카드 배부*/
    void Start()
    {
        isCardGenerated = false; // 카드 배부가 되지 않았다.

        if (isCardGenerated == false) // 카드 배부가 되지 않았나?
        {
            cardArray = ShuffleArray(cardArray);
            for (int i = 0; i < cardArray.Length; i++) // 카드 갯수 만큼 수행
            {
                float x = (i % div) * gapX - initX; // 가로 좌표 지정
                float y = (i / div) * gapY - initY; // 세로 좌표 지정

                GameObject go = Instantiate(card, new Vector2(0, 0), Quaternion.identity, transform); // 카드를 화면 중앙에 소환하고 Board 게임 오브젝트의 자식으로 설정
                go.transform.Find("body").transform.localScale = Vector2.one * scale; // 카드의 크기 조정
                // 카드가 생성된 후 목표 위치로 날아가는 애니메이션 추가
                LeanTween.move(go, new Vector2(x, y), 0.5f) // 목표 위치까지 이동
                         .setEase(LeanTweenType.easeOutBounce) // 이징 효과 설정
                         .setDelay(i * 0.1f) // 카드마다 딜레이 추가
                         .setOnStart(() => PlayCardMoveSound()); // 애니메이션 완료 후 효과음 재생

                go.GetComponent<Card>().CardSpriteSetting(cardArray[i]);
            }
            
        }
        isCardGenerated = true; // 카드 배부가 끝났다.
    }

    // 카드 이동 효과음 재생 함수
    void PlayCardMoveSound()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
    }


    /* 카드 리스트를 섞는 메서드 */
    private int[] ShuffleArray(int[] array)
    {
        int random1, random2, temp; // 랜덤 주소를 저장할 변수 2개 & 임시 저장 변수 1개
        for(int i = 0; i < array.Length; i++) //array.Lenght의 횟수 만큼 수행
        {
            random1 = Random.Range(0, array.Length); // 주소값 랜덤 할당
            random2 = Random.Range(0, array.Length); // 주소값 랜덤 할당

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp; // array[random1]과 array[random2]의 값이 서로 교환된다.
        }
        return array; // 섞기가 완료된 배열 반환
    }
}

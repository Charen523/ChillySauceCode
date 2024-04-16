using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // x = -1.65f + i * 1.1f
    // y = -3f + j * 1.38f
    public GameObject card;
   
    public AudioClip cardMoveSound;
    //카드가 다 생성 되었는지 확인
    [SerializeField]
    public static bool isCardGenerated;

    void Start()
    {
        isCardGenerated = false;

        if (isCardGenerated == false)
        {
            int[] cardArray = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            cardArray = ShuffleArray(cardArray);

            for (int i = 0; i < cardArray.Length; i++)
            {
                float x = (i % 4) * 1.1f - 1.65f;
                float y = (i / 4) * 1.4f - 3f;

                GameObject go = Instantiate(card, new Vector2(0, 0), Quaternion.identity, transform);
                // 카드가 생성된 후 목표 위치로 날아가는 애니메이션 추가
                LeanTween.move(go, new Vector2(x, y), 0.5f) // 목표 위치까지 이동
                         .setEase(LeanTweenType.easeOutBounce) // 이징 효과 설정
                         .setDelay(i * 0.1f) // 카드마다 딜레이 추가
                         .setOnStart(() => PlayCardMoveSound()); // 애니메이션 완료 후 효과음 재생



                go.GetComponent<Card>().CardSpriteSetting(cardArray[i]);
            }
            
        }
        isCardGenerated = true;
    }

    // 카드 이동 효과음 재생 함수
    void PlayCardMoveSound()
    {
        AudioSource.PlayClipAtPoint(cardMoveSound, Camera.main.transform.position);
    }


    // 카드 리스트를 섞는 메서드
    private int[] ShuffleArray(int[] array)
    {
        int random1, random2, temp;
        for(int i = 0; i < array.Length; i++)
        {
            random1 = Random.Range(0, array.Length);
            random2 = Random.Range(0, array.Length);

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }
        return array;
    }
}

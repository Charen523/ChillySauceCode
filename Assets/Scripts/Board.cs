using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // x = -1.65f + i * 1.1f
    // y = -3f + j * 1.38f
    public GameObject card;
       
    //카드가 다 생성 되었는지 확인
    [SerializeField]
    public static bool isCardGenerated;
    public static int cardArrayLenght;

    int[] cardArray;
    float initX, initY, gapX, gapY, scale;
    int div;
    private void Awake()
    {
        if (LevelManager.Instance.selectLevel == 2)
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };
            div = 6;
            initX = 1.82f;
            initY = 2.1f;
            gapX = 0.72f;
            gapY = 1.08f;
            scale = 0.75f;
        }
        else
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            div = 4;
            initX = 1.65f;
            initY = 3f;
            gapX = 1.1f;
            gapY = 1.4f;
            scale = 1;
        }
        cardArrayLenght = cardArray.Length;
    }
    void Start()
    {
        isCardGenerated = false;

        if (isCardGenerated == false)
        {
            cardArray = ShuffleArray(cardArray);
            for (int i = 0; i < cardArray.Length; i++)
            {
                float x = (i % div) * gapX - initX;
                float y = (i / div) * gapY - initY;

                GameObject go = Instantiate(card, new Vector2(0, 0), Quaternion.identity, transform);
                go.transform.Find("body").transform.localScale = Vector2.one * scale;
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
       // AudioSource.PlayClipAtPoint(cardMoveSound, Camera.main.transform.position);

        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
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

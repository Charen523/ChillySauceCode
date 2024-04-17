using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // x = -1.65f + i * 1.1f
    // y = -3f + j * 1.38f
    public GameObject card;
       
    //ī�尡 �� ���� �Ǿ����� Ȯ��
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
                // ī�尡 ������ �� ��ǥ ��ġ�� ���ư��� �ִϸ��̼� �߰�
                LeanTween.move(go, new Vector2(x, y), 0.5f) // ��ǥ ��ġ���� �̵�
                         .setEase(LeanTweenType.easeOutBounce) // ��¡ ȿ�� ����
                         .setDelay(i * 0.1f) // ī�帶�� ������ �߰�
                         .setOnStart(() => PlayCardMoveSound()); // �ִϸ��̼� �Ϸ� �� ȿ���� ���



                go.GetComponent<Card>().CardSpriteSetting(cardArray[i]);
            }
            
        }
        isCardGenerated = true;
    }

    // ī�� �̵� ȿ���� ��� �Լ�
    void PlayCardMoveSound()
    {
       // AudioSource.PlayClipAtPoint(cardMoveSound, Camera.main.transform.position);

        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
    }


    // ī�� ����Ʈ�� ���� �޼���
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

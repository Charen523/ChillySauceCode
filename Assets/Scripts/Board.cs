using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    // x = -1.65f + i * 1.1f
    // y = -3f + j * 1.38f
    public GameObject card;
   
    public AudioClip cardMoveSound;
    //ī�尡 �� ���� �Ǿ����� Ȯ��
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
        AudioSource.PlayClipAtPoint(cardMoveSound, Camera.main.transform.position);
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

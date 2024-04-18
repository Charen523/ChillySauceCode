using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject card; // ī�� ������
       
    [SerializeField]
    public static bool isCardGenerated; //ī�尡 �� ���� �Ǿ����� Ȯ��
    public static int cardArrayLenght; //ī�� ������ �Ѱ��� ����

    int[] cardArray; // ������������ �ٸ� ī�� ����Ʈ�� ���� �迭
    float initX, initY, gapX, gapY, scale; // ������������ �ٸ� �ʱ� pos��, ����, ũ�Ⱚ�� ������ ����
    int div; // �� �ٿ� ���� ī�� ����
    private void Awake()
    {
        if (LevelManager.Instance.selectLevel >= 3) // ���������� 3 �̻��ΰ�? (�ش� ���������� �ش��ϴ� ������ �Ҵ�)
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };
            div = 6;
            initX = 1.82f;
            initY = 2.1f;
            gapX = 0.72f;
            gapY = 1.08f;
            scale = 0.75f;
        }
        else // ���������� 3 �̻��� �ƴѰ�? (�ش� ���������� �ش��ϴ� ������ �Ҵ�)
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            div = 4;
            initX = 1.65f;
            initY = 3f;
            gapX = 1.1f;
            gapY = 1.4f;
            scale = 1;
        }
        cardArrayLenght = cardArray.Length; // ī�� �迭�� ���̸� static ������ �Ҵ�
    }
    /*�������� ���� �� ī�� ���*/
    void Start()
    {
        isCardGenerated = false; // ī�� ��ΰ� ���� �ʾҴ�.

        if (isCardGenerated == false) // ī�� ��ΰ� ���� �ʾҳ�?
        {
            cardArray = ShuffleArray(cardArray);
            for (int i = 0; i < cardArray.Length; i++) // ī�� ���� ��ŭ ����
            {
                float x = (i % div) * gapX - initX; // ���� ��ǥ ����
                float y = (i / div) * gapY - initY; // ���� ��ǥ ����

                GameObject go = Instantiate(card, new Vector2(0, 0), Quaternion.identity, transform); // ī�带 ȭ�� �߾ӿ� ��ȯ�ϰ� Board ���� ������Ʈ�� �ڽ����� ����
                go.transform.Find("body").transform.localScale = Vector2.one * scale; // ī���� ũ�� ����
                // ī�尡 ������ �� ��ǥ ��ġ�� ���ư��� �ִϸ��̼� �߰�
                LeanTween.move(go, new Vector2(x, y), 0.5f) // ��ǥ ��ġ���� �̵�
                         .setEase(LeanTweenType.easeOutBounce) // ��¡ ȿ�� ����
                         .setDelay(i * 0.1f) // ī�帶�� ������ �߰�
                         .setOnStart(() => PlayCardMoveSound()); // �ִϸ��̼� �Ϸ� �� ȿ���� ���

                go.GetComponent<Card>().CardSpriteSetting(cardArray[i]);
            }
            
        }
        isCardGenerated = true; // ī�� ��ΰ� ������.
    }

    // ī�� �̵� ȿ���� ��� �Լ�
    void PlayCardMoveSound()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
    }


    /* ī�� ����Ʈ�� ���� �޼��� */
    private int[] ShuffleArray(int[] array)
    {
        int random1, random2, temp; // ���� �ּҸ� ������ ���� 2�� & �ӽ� ���� ���� 1��
        for(int i = 0; i < array.Length; i++) //array.Lenght�� Ƚ�� ��ŭ ����
        {
            random1 = Random.Range(0, array.Length); // �ּҰ� ���� �Ҵ�
            random2 = Random.Range(0, array.Length); // �ּҰ� ���� �Ҵ�

            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp; // array[random1]�� array[random2]�� ���� ���� ��ȯ�ȴ�.
        }
        return array; // ���Ⱑ �Ϸ�� �迭 ��ȯ
    }
}

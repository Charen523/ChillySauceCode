using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject card; //ī�� ������Ʈ
       
    public static int cardArrayLength; //ī�� �迭 ���� ����

    int[] cardArray; //ī�带 ���� �迭
    float initX, initY, gapX, gapY, scale; // �������������� �ʱ� ��ġ, ����, ũ�⸦ ������ ����
    int div; // �� �ٿ� ���� ī�� ����

    private void Awake()
    {
        /*������ ���� ī��迭 ����*/
        if (LevelManager.Instance.selectLevel >= 3) //3���� �̻�
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11 };
            div = 6; //�����ٿ� �� ī�� ����
            initX = 1.82f; //�����ϴ� ī�� x ��ġ
            initY = 2.1f; //�����ϴ� ī�� y ��ġ
            gapX = 0.72f; //ī�� x ����
            gapY = 1.08f; //ī�� y ����
            scale = 0.75f; //ī�� ũ��
        }
        else //3���� �̸�
        {
            cardArray = new int[] { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            div = 4; //�����ٿ� �� ī�� ����
            initX = 1.65f; //�����ϴ� ī�� x ��ġ
            initY = 3f; //�����ϴ� ī�� y ��ġ
            gapX = 1.1f; //ī�� x ����
            gapY = 1.4f; //ī�� y ����
            scale = 1; //ī�� ũ��
        }
        cardArrayLength = cardArray.Length; // ī�� �迭�� ���̸� static ������ �Ҵ�
    }

    /*�������� ���� �� ī�� ���*/
    void Start()
    {
        cardArray = ShuffleArray(cardArray); //ī�带 �������� �����ֱ�

        for (int i = 0; i < cardArray.Length; i++) // ī�� ���� ��ŭ ����
        {
            /*ī�带 ������ ����� ����*/
            float x = (i % div) * gapX - initX; // ���� ��ǥ ����
            float y = (i / div) * gapY - initY; // ���� ��ǥ ����
            GameObject go = Instantiate(card, new Vector2(0, 0), Quaternion.identity, transform); // ī�带 ȭ�� �߾ӿ� ��ȯ�ϰ� Board ���� ������Ʈ�� �ڽ����� ����
            go.transform.Find("body").transform.localScale = Vector2.one * scale; // ī���� ũ�� ����

            /*ī�尡 ���� �� ��ǥ ��ġ�� ���ư��� �ִϸ��̼�*/
            LeanTween.move(go, new Vector2(x, y), 0.5f) // ��ǥ ��ġ���� �̵�
                     .setEase(LeanTweenType.easeOutBounce) // ��¡ ȿ�� ����
                     .setDelay(i * 0.1f) // ī�帶�� ������ �߰�
                     .setOnStart(() => PlayCardMoveSound()); //ȿ���� ���

            go.GetComponent<Card>().CardSpriteSetting(cardArray[i]); //ī�� �ո� �̹��� ������
        }
    }

    /*ī�� �й� ȿ���� �Լ�*/
    void PlayCardMoveSound()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
    }

    /* ī�� ����Ʈ�� ���� �޼��� */
    private int[] ShuffleArray(int[] array)
    {
        int random1, random2, temp; // ���� �ּҸ� ������ ���� 2�� & �ӽ� ���� ���� 1��

        for(int i = 0; i < array.Length; i++) //array.Length�� Ƚ�� ��ŭ �ݺ�
        {
            random1 = Random.Range(0, array.Length); // �ּҰ� ���� �Ҵ�
            random2 = Random.Range(0, array.Length); // �ּҰ� ���� �Ҵ�
            
            /*random1�� random2�� ����*/
            temp = array[random1];
            array[random1] = array[random2];
            array[random2] = temp;
        }

        return array; // ���Ⱑ �Ϸ�� �迭 ��ȯ
    }
}
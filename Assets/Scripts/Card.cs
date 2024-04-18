using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public GameObject backBtn; // ī�� �޸�(��ư)
    public SpriteRenderer frontImg; // ī�� �ո�(��������Ʈ)

    public int idx; // ī�� �ո� �̹��� �ε���

    /*ī�� ������ �ִϸ��̼� ����*/
    float cardFlipSpeed = 0.2f; //ī�� ������ �ӵ�
    LeanTweenType leanTweenType; //leanTween ease Ÿ�Ժ���

    bool isCardOpened; // ī�� ���� ���� ����
    bool firstCardOpened; // ù��° ī�� ���� ����
    float cardOpenedTime; // ī�尡 ���� ä�� �帥 �ð�

    void Start()
    {
        firstCardOpened = false;
    }

    void Update()
    {
        if (firstCardOpened)// ù��° ī�� ���� ����
        {
            cardOpenedTime += Time.deltaTime; // �ð� ����

            /*ī�带 ���� ���� �ð��� ������ �г�Ƽ �ο��� �Բ� ù ��° ī�� �ݱ�*/
            if (cardOpenedTime > 1f) // ī�尡 ���� �ð� ī��Ʈ
            {
                GameManager.Instance.firstCard.CloseCard(); // ù��° ī�� �ݱ�
                GameManager.Instance.firstCard = null; // ù��° ī�� ������ �ʱ�ȭ
                Invoke("TimePenaltyInvoke", 1f); //1�� �� �ð� �г�Ƽ �ο�
            }
        }
    }

    /*ī�� �ո� ���� �޼���*/
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}"); // ī�� �ո鿡 Resources/Images���� �ش��ϴ� �̹����� �ε�
    }

    /*ī�尡 Ŭ�� ��*/
    public void CardClick()
    {
        if (!GameManager.Instance.isMatching) // ī�尡 ��Ī�� ����
        {
            if (!PauseBtn.isPaused) //�Ͻ����� ���� ����
            {
                RotateCard(); //������ �ִ� ȣ��
                isCardOpened = true; // ī�� ����

                if (GameManager.Instance.firstCard == null) // ù ī���� ��
                {
                    firstCardOpened = true; // ù��° ī�� �Ҵ� Ȯ��
                    GameManager.Instance.firstCard = this; // GameManager�� ù��° ī�忡 ���� gameObject�� �Ҵ�
                }
                else // �� ��° ī���� ��
                {
                    GameManager.Instance.secondCard = this; // GameManager�� �ι�° ī�忡 ���� gameObject�� �Ҵ�
                    GameManager.Instance.Matched(); //��Ī �Ϸ�
                    GameManager.Instance.isMatching = true; // ��Ī��
                }
                AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]); //ī�� ������ ȿ����
            }
        }
    }

    /*ī�� �ı� �޼���*/
    public void DestroyCard()
    {
        firstCardOpened = false; // ī��� ���� Ŭ���� ���°� �ƴϴ�.
        cardOpenedTime = 0f; // ī�� ��� �ð� �ʱ�ȭ
        StartCoroutine("DestroyCardCoroutine");
    }

    /*ī�� �ݱ� �޼���*/
    public void CloseCard()
    {
        firstCardOpened = false; // ī��� ���� Ŭ���� ���°� �ƴϴ�.
        cardOpenedTime = 0f; // ī�� ��� �ð� �ʱ�ȭ
        StartCoroutine("CloseCardCoroutine");

        if (isCardOpened) // ī�尡 �ѹ� �������� �ִ°�?
        {
            changeColor();
        }
    }

    /*ī�� �ı� �ڷ�ƾ*/
     IEnumerator DestroyCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1�� ���
        Destroy(gameObject); // �ش� gameObject �ı�
        GameManager.Instance.isMatching = false; // ī��� ���� ��Ī���� �ƴϴ�.
        
    }

    /*ī�� �ݱ� �ڷ�ƾ*/
     IEnumerator CloseCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1�� ���
        ReverseCard(); //ī�� ������
        GameManager.Instance.isMatching = false; // ī��� ���� ��Ī���� �ƴϴ�.
    }

    /*ī�� �޸��� ������ ��Ӱ� �ϴ� �޼���*/
    void changeColor()
    {
        backBtn.GetComponentInChildren<Image>().color = Color.gray; // ī�� �޸��� Image�� ������ ȸ������ ����
    }

    /*ī�� �ð��ʰ��� ���Ƽ �ο��ϴ� �޼���*/
    void TimePenaltyInvoke()
    {
        GameManager.Instance.TimePenalty();
    }

    /*ī�� ���� �޼���*/
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

    /*ī�� �ݱ� �޼���*/
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

    /*LeanTween ���� �� ī�� �ո��� �����ִ� �޼���*/
     void ShowFront()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(true);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(false);
    }

    /*LeanTween ���� �� ī�� �޸��� �����ִ� �޼���*/
    void ShowBack()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(false);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(true);
        
    }

    void ScaleCard(GameObject go, Vector3 scale) => LeanTween.scale(go, scale, cardFlipSpeed);
}

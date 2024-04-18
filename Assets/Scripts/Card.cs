using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public AudioClip clip; // ī�� ������ ���� ȿ����

    public SpriteRenderer frontImg; // ī�� �ո� ��������Ʈ

    public int idx; // ī�� �ո��� �����ϴ� ��ȣ

    public bool isCardOpened; // ī�尡 �������������� ��Ÿ���� ����
    public GameObject backBtn; // ī�尡 ���� �޸� ��ư

    bool onClick; // ù��° ī�尡 ������ �� ����.
    float time; // ī�尡 ���� ä�� �帥 �ð�.

    // Start is called before the first frame update

    //ī�� �Ѿ�� �ӵ��� leanTween ease Ÿ�Ժ����Դϴ�.
    public float m_Speed = 0.2f;
    public LeanTweenType leanTweenType;

    /*������ �ʱ�ȭ*/
    void Start()
    {
        onClick = false;
    }

    void Update()
    {
        if (GameManager.Instance.firstCard != null) // ù��° ī�尡 null�� �ƴѰ�? (�����Ͱ� ���Դ°�?)
        {
            if (onClick) // ī�尡 Ŭ���Ǿ��°�?
            {
                time += Time.deltaTime; // �ð� ����
            }
            if (time > 4f) // �ð��� 4�� �̻� �����°�?
            {
                GameManager.Instance.firstCard.CloseCard(); // ù��° ī�� �ݱ�
                GameManager.Instance.firstCard = null; // ù��° ī�� ������ �ʱ�ȭ
                Invoke("TimePenalty", 1f);
            }
        }
    }

    /*ī�� �ո��� �����ϴ� �޼���*/
    public void CardSpriteSetting(int number)
    {
        idx = number;
        frontImg.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}"); // ī�� �ո� ��������Ʈ�� Resources/Images���� �ش��ϴ� �̹����� �ε�
    }

    /*ī�尡 Ŭ���Ǿ��� �� ȣ��Ǵ� �޼���*/
    public void CardClick()
    {
       if(Board.isCardGenerated) // ī�尳 ��εǾ��°�?
        { 
            if (!GameManager.Instance.isMatching) // ī�尡 ��Ī���� �ƴѰ�?
            {
                if (!PauseBtn.isPaused) // ���� �Ͻ����� ���°� �ƴѰ�?
                {
                    RotateCard();
                    isCardOpened = true; // ī�尡 �ѹ� ���ȴ�.

                    if (GameManager.Instance.firstCard == null) // GameManager�� ù��° ī�尡 null�ΰ�? (�����Ͱ� ���°�?)
                    {
                        onClick = true; // ī�尡 Ŭ���Ǿ���.
                        GameManager.Instance.firstCard = this; // GameManager�� ù��° ī�忡 ���� gameObject�� �Ҵ�
                    }
                    else // �ƴѰ�? (GameManager�� ù��° ī�忡 �����Ͱ� ����ִ°�?)
                    {
                        GameManager.Instance.secondCard = this; // GameManager�� �ι�° ī�忡 ���� gameObject�� �Ҵ�
                        GameManager.Instance.Matched();
                        GameManager.Instance.isMatching = true; // ���� ��Ī���̴�.
                    }
                    AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[2]);
                }
            }   
       }
    }

    /*ī�带 �ı��ϴ� �޼���*/
    public void DestroyCard()
    {
        onClick = false; // ī��� ���� Ŭ���� ���°� �ƴϴ�.
        time = 0f; // ī�� ��� �ð� �ʱ�ȭ
        StartCoroutine("DestroyCardCoroutine");
    }

    /*ī�带 �ٽ� �ݴ� �޼���*/
    public void CloseCard()
    {
        onClick = false; // ī��� ���� Ŭ���� ���°� �ƴϴ�.
        time = 0f; // ī�� ��� �ð� �ʱ�ȭ
        StartCoroutine("CloseCardCoroutine");

        if (isCardOpened) // ī�尡 �ѹ� �������� �ִ°�?
        {
            changeColor();
        }
    }

    /*ī�带 �ı��ϴ� �ڷ�ƾ*/
    public IEnumerator DestroyCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1�� ���
        Destroy(gameObject); // �ش� gameObject �ı�
        GameManager.Instance.isMatching = false; // ī��� ���� ��Ī���� �ƴϴ�.
        
    }

    /*ī�带 �ٽ� �ݴ� �ڷ�ƾ*/
    public IEnumerator CloseCardCoroutine()
    {
        yield return new WaitForSeconds(1f); // 1�� ���
        ReverseCard();

        GameManager.Instance.isMatching = false; // ī��� ���� ��Ī���� �ƴϴ�.
    }

    /*ī�� �޸��� ������ ��Ӱ� �ϴ� �޼���*/
    void changeColor()
    {
        backBtn.GetComponentInChildren<Image>().color = Color.gray; // ī�� �޸��� Image�� ������ ȸ������ ���� (�ణ ��ο���)
    }

    /*ī�带 ������ �� 5�� ���� ���Ƽ Ÿ���� �ο��ϴ� �޼���*/
    void TimePenalty()
    {
        GameManager.Instance.TimePenalty();
    }

    /*ī�带 ���� �޼���*/
    public void RotateCard()
    {
        var rot = Mathf.Round(transform.localRotation.y) ==0f ? 180f : 0f;
        if (rot < 0f) rot = 0f;
        LeanTween.rotateY(gameObject, rot, m_Speed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(m_Speed / 2, () => ShowFront())
            .setEase(leanTweenType);
    }

    /*ī�带 �ٽ� �ݴ� �޼���*/
    public void ReverseCard()
    {
        var rot = Mathf.Round(transform.localRotation.y) == 0f ? 180f : 0f;
               
        LeanTween.rotateY(gameObject, rot, m_Speed)
            .setOnStart(() => ScaleCard(gameObject, Vector3.one * 1.1f))
            .setOnComplete(() => ScaleCard(gameObject, Vector3.one))
            .setEase(leanTweenType);
        LeanTween.delayedCall(m_Speed / 2, () => ShowBack())
            .setEase(leanTweenType);
        Debug.Log(rot);
    }

    /*LeanTween ���� �� ī�� �ո��� �����ִ� �޼���*/
    protected void ShowFront()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(true);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(false);
    }

    /*LeanTween ���� �� ī�� �޸��� �����ִ� �޼���*/
    protected void ShowBack()
    {
        transform.Find("body")?.transform.Find("Front")?.gameObject.SetActive(false);
        transform.Find("body")?.transform.Find("Back")?.gameObject.SetActive(true);
        
    }

    protected void ScaleCard(GameObject go, Vector3 scale) => LeanTween.scale(go, scale, m_Speed);


}

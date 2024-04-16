using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchResult : MonoBehaviour
{
    public GameManager manager; //���� ����.
    public Image matchPanel; //GM���� �̵��ؼ� �缱��.
    public Text matchTxt; //GM���� �̵��ؼ� �缱��.
    

    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //���н� �̹��� ����.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//������ �̹��� ����.

    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>(); //GameManager�� Start�Լ��� �ʿ�.
        StartCoroutine(MatchPanelDelay());
    }


    IEnumerator MatchPanelDelay()
    {
        if (manager.firstCard.idx == manager.secondCard.idx)
        {
            
        }
        else
        {
            
        }

        yield return new WaitForSeconds(1f);

        matchPanel.enabled = true;
    }
}

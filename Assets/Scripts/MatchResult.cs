using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchResult : MonoBehaviour
{
    public GameManager manager; //���� ����.
    public Image matchBackground; //GM���� �̵��ؼ� �缱��.
    public Text ResultTxt; //GM���� �̵��ؼ� �缱��.
    

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
            matchBackground.enabled = true;

            switch (manager.firstCard.idx % 4)
            {
                case 0:
                    ResultTxt.text = "�迵��!";
                    break;
                case 1:
                    ResultTxt.text = "�����!";
                    break;
                case 2:
                    ResultTxt.text = "�̽¿�!";
                    break;
                case 3:
                    ResultTxt.text = "���¿�!";
                    break;
                default:
                    Debug.Log("�ε��� ����.");
                    break;
            }
        }
        else
        {
            ResultTxt.text = "����...";
        }

        yield return new WaitForSeconds(1f);

        matchBackground.enabled = true;
    }
}

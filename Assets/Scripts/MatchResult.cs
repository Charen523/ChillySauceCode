using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchResult : MonoBehaviour
{
    public GameManager manager; //삭제 예정.
    public Image matchPanel; //GM으로 이동해서 재선언.
    public Text matchTxt; //GM으로 이동해서 재선언.
    

    Color FailColor = new Color(255 / 255f, 119 / 255f, 119 / 255f); //실패시 이미지 배경색.
    Color SuccessColor = new Color(154 / 255f, 255 / 255f, 154 / 255f);//성공시 이미지 배경색.

    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>(); //GameManager의 Start함수에 필요.
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

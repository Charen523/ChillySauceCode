using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    public int idx;
    // Start is called before the first frame update
    void Start()
    {
        //칠리소스코드 예제입니다. 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CardSpriteSetting(int number)
    {
        idx = number;
        spriteRenderer.sprite = Resources.Load<Sprite>($"Images/Sprite{idx}");
    }
    public void CardClick()
    {
        Debug.Log("카드 클릭");
    }    

    public void DestoryCard()
    {

    }

    public void CloseCard()
    {

    }

   
}

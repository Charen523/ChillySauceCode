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
        //ĥ���ҽ��ڵ� �����Դϴ�. 
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
        Debug.Log("ī�� Ŭ��");
    }    

    public void DestoryCard()
    {

    }

    public void CloseCard()
    {

    }

   
}

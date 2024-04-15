using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public AudioClip clip;

    AudioSource audioSource;    

    public int idx;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

    public void OpenCard()
    {
        audioSource.PlayOneShot(clip);
    }

    public void CloseCard()
    {

    }

   
}

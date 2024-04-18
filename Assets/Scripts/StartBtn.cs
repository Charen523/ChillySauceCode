using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    //스타트 버튼이 눌렸는지 확인
    public static bool isStartBtnPushed = false;

    void Start()
    {
        LevelManager.Instance.LevelCheck();
       

    }

    private void Update()
    {
        

    }

    public void StartGame()
    {
        
        if (PauseBtn.isPaused != true)
        {
            
            //SceneManager.LoadScene("MainScene");

            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);

            LevelManager.Instance.OpenCanvas();
            //스타트 버튼이 눌림
            isStartBtnPushed = true;
        }
    }

    

}

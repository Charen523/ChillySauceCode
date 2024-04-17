using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    private float time;

    void Start()
    {
        LevelManager.Instance.LevelCheck();
        
    }

    private void Update()
    {
        //  깜빡이는 효과 아직 미완
       /*
        if(time < 2.0f)
        {
            GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0 - time);
        }
        else
        {
            GetComponentInChildren<Text>().color = new Color(0, 0, 0, time);
            if(time > 1.0f)
            {
                time = 0f;
            }
        }
        time += Time.deltaTime;
       */
    }

    public void StartGame()
    {
        //일시정지 버튼이 눌리지 않았다면
        if (PauseBtn.isPaused != true)
        {
            // 메인씬으로 이동
            //SceneManager.LoadScene("MainScene");

            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);

            LevelManager.Instance.OpenCanvas();
        }
    }


}

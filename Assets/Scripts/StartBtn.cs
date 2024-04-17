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
        /*
        if(time < 1.0f)
        {
            GetComponentInChildren<Text>().color = new Color(1, 1, 1, 1 - time);
        }
        else
        {
            GetComponentInChildren<Text>().color = new Color(1, 1, 1, time);
            if(time > 1.0f)
            {
                time = 0f;
            }
        
        }
        */
    }

    public void StartGame()
    {
        //�Ͻ����� ��ư�� ������ �ʾҴٸ�
        if (PauseBtn.isPaused != true)
        {
            // ���ξ����� �̵�
            //SceneManager.LoadScene("MainScene");

            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);

            LevelManager.Instance.OpenCanvas();
        }
    }


}

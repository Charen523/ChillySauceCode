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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseMenu; //���� �޴�
    public static bool isPaused = false; //Pause ���� ����
    
    Scene currentScene; //���� ��

    /*Pause ��ư Ŭ�� ��*/
    public void PushPauseMenu()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //��ư ȿ����
        currentScene = SceneManager.GetActiveScene(); //���� ȭ�� �ҷ�����
        
        if (!isPaused) //Pause ���°� �ƴ϶��
        {
            isPaused = true; //Pause ���� Ȱ��ȭ
            Time.timeScale = 0.0f; //�ð� ����
            PauseMenu.SetActive(true); //Pause�޴� Ȱ��ȭ
        }
        else //Pause ���¶��
        {
            isPaused = false; //Pause ���� ��Ȱ��ȭ
            Time.timeScale = 1.0f; //�ð� �簳
            PauseMenu.SetActive(false); //Pause�޴� ��Ȱ��ȭ
        }
    }

    /*���θ޴� ��ư Ŭ�� ��*/
    public void GoBackToStartScene()
    {
        if (currentScene.name != "StartScene") //��ŸƮ���� �ƴ� ���� ��ư Ȱ��ȭ
        {
            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //��ư ȿ����

            /*������� ���*/
            AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[0]; 
            AudioManager.Instance.audioSource[0].Play();
            
            /*���� �޴� ���ֱ�*/
            isPaused = false; //Pause ���� ��Ȱ��ȭ
            Time.timeScale = 1f; //�ð� �簳
            PauseMenu.SetActive(false); //Pause�޴� ��Ȱ��ȭ

            StartBtn.isStartBtnPushed = false; //��ŸƮ��ư ������ �ٽ� ���
            
            SceneManager.LoadScene("StartScene"); //��ŸƮ������ �ε�
        }
    }

}

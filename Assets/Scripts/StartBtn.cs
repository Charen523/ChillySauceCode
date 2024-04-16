using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StratBtn : MonoBehaviour
{
    public AudioClip buttonClip;

    AudioSource audioSource;

    private void Start()
    {
        LevelManager.Instance.LevelCheck();
        audioSource = GetComponent<AudioSource>();
    }

    public void StartGame()
    {
        //�Ͻ����� ��ư�� ������ �ʾҴٸ�
        if (PauseBtn.isPaused != true)
        {
            // ���ξ����� �̵�
            //SceneManager.LoadScene("MainScene");

            audioSource.PlayOneShot(buttonClip);
            LevelManager.Instance.OpenCanvas();
        }
    }
}

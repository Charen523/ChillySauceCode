using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseMenu;
    public AudioClip buttonClip;

    public static bool isPaused = false;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();  
    }

    public void PushPauseMenu()
    {
        audioSource.PlayOneShot(buttonClip);

        if (!isPaused)
        {
            // �Ͻ� ���� ��ư�� ���� isPaused = false�� ������ ����
            Time.timeScale = 0.0f;
            isPaused = true;
            // �� Ȱ��ȭ ���ѵξ��� PauseMenu�� ����
            PauseMenu.SetActive(true);

        }
        else
        {
            // isPaused = true �� �ٽ� ������ �簳
            Time.timeScale = 1.0f;
            isPaused = false;
            // Ȱ��ȭ �Ǿ��� PasueMenu�� �ٽ� ��Ȱ��ȭ
            PauseMenu.SetActive(false);

        }
    }
}

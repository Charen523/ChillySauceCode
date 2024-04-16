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
            // 일시 정지 버튼을 눌러 isPaused = false면 게임을 멈춤
            Time.timeScale = 0.0f;
            isPaused = true;
            // 비 활성화 시켜두었던 PauseMenu가 등장
            PauseMenu.SetActive(true);

        }
        else
        {
            // isPaused = true 면 다시 게임을 재개
            Time.timeScale = 1.0f;
            isPaused = false;
            // 활성화 되었던 PasueMenu가 다시 비활성화
            PauseMenu.SetActive(false);

        }
    }
}

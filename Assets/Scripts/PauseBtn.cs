using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseMenu;


    public static bool isPaused = false;
    Scene currentScene;

    private void Start()
    {


    }

    public void PushPauseMenu()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);
        currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name);

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

    public void GoBackToStartScene()
    {
        if (currentScene.name != "StartScene")
        {

            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);
            Time.timeScale = 1f;
            AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[0];
            AudioManager.Instance.audioSource[0].Play();
            Board.isCardGenerated = false;
            PauseMenu.SetActive(false);
            isPaused = false;
            SceneManager.LoadScene("StartScene");
            //스타트버튼 깜빡임 다시 시작되도록 값 변경
            StartBtn.isStartBtnPushed = false;
        }
    }

}

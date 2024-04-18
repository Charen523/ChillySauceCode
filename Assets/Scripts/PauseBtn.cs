using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseMenu; //퍼즈 메뉴
    public static bool isPaused = false; //Pause 상태 전달
    
    Scene currentScene; //현재 씬

    /*Pause 버튼 클릭 시*/
    public void PushPauseMenu()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //버튼 효과음
        currentScene = SceneManager.GetActiveScene(); //현재 화면 불러오기
        
        if (!isPaused) //Pause 상태가 아니라면
        {
            isPaused = true; //Pause 상태 활성화
            Time.timeScale = 0.0f; //시간 멈춤
            PauseMenu.SetActive(true); //Pause메뉴 활성화
        }
        else //Pause 상태라면
        {
            isPaused = false; //Pause 상태 비활성화
            Time.timeScale = 1.0f; //시간 재개
            PauseMenu.SetActive(false); //Pause메뉴 비활성화
        }
    }

    /*메인메뉴 버튼 클릭 시*/
    public void GoBackToStartScene()
    {
        if (currentScene.name != "StartScene") //스타트씬이 아닐 때만 버튼 활성화
        {
            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //버튼 효과음

            /*배경음악 재생*/
            AudioManager.Instance.audioSource[0].clip = AudioManager.Instance.bgmClips[0]; 
            AudioManager.Instance.audioSource[0].Play();
            
            /*퍼즈 메뉴 없애기*/
            isPaused = false; //Pause 상태 비활성화
            Time.timeScale = 1f; //시간 재개
            PauseMenu.SetActive(false); //Pause메뉴 비활성화

            StartBtn.isStartBtnPushed = false; //스타트버튼 깜빡임 다시 재생
            
            SceneManager.LoadScene("StartScene"); //스타트씬으로 로드
        }
    }

}

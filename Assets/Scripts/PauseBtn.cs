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
            //��ŸƮ��ư ������ �ٽ� ���۵ǵ��� �� ����
            StartBtn.isStartBtnPushed = false;
        }
    }

}

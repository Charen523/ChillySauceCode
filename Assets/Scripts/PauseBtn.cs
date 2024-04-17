using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseMenu;

    public static bool isPaused = false;

    

    private void Start()
    {
        
    }

    public void PushPauseMenu()
    {
        AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]);

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

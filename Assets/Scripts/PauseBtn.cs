using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseBtn : MonoBehaviour
{
    public GameObject PauseMenu;

    public bool isPaused = false;
    public void PushPauseMenu()
    {
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

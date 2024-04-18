using UnityEngine;

public class StartBtn : MonoBehaviour
{
    public static bool isStartBtnPushed = false; //스타트 버튼 활성화 여부 감지

    void Start()
    {
        LevelManager.Instance.LevelCheck(); //해금 레벨 재확인
    }

    /*스타트 버튼 클릭 시*/
    public void StartGame()
    {
        if (PauseBtn.isPaused != true) //Pause 상태가 아니라면
        {
            AudioManager.Instance.audioSource[1].PlayOneShot(AudioManager.Instance.sfxClips[0]); //버튼 효과음
            LevelManager.Instance.OpenCanvas(); //레벨판넬 활성화

            isStartBtnPushed = true; //스타트 버튼 비활성화 (애니메이션 재생 방지)
        }
    }
}

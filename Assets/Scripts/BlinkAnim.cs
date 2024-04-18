using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkAnim : MonoBehaviour
{
    float time;
    bool isFadingIn = false;

    // Update is called once per frame
    void Update()
    {
        if (StartBtn.isStartBtnPushed == false)
        {

            if (isFadingIn)
            {
                time += Time.deltaTime * 2; // 텍스트가 점진적으로 나타나는 속도를 조절하기 위해 2를 곱합니다. 
            }
            else
            {
                time -= Time.deltaTime * 2; // 텍스트가 점진적으로 사라지는 속도를 조절하기 위해 2를 곱합니다.
            }

            // 텍스트가 완전히 나타나거나 완전히 사라지면 상태를 변경합니다.
            if (time >= 1f)
            {
                isFadingIn = false;
                time = 1f;
            }
            else if (time <= 0.5f)
            {
                isFadingIn = true;
                time = 0.5f;
            }

            // 시간에 따른 투명도 조절
            GetComponent<Text>().color = new Color(0, 0, 0, time);
        }
    }
}

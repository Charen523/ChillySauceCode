using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer AudioMixer; // 오디오 믹서
    public Slider BGMSilder; // BGM 슬라이더
    public Slider SFXSilder; // 효과음 슬라이더

    public AudioClip[] bgmClips; // BGM 클립 배열
    public AudioClip[] sfxClips; // 효과음 클립 배열

    public AudioSource[] audioSource; //BGM 오디오 소스, 효과음 오디오 소스를 저장하는 오디오 소스 배열

    float bgmVolume; // BGM 볼륨
    float sfxVolume; // 효과음 볼륨

    /*인스턴스 화 및 파괴 방지*/
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /*데이터 초기화*/
    void Start()
    {
        audioSource = GetComponentsInChildren<AudioSource>(); //AudioManager 자식 중 AudioSource의 컴포넌트를 검색 후 순서대로 배열로 저장

        // 볼륨 값 불러오기
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        // 오디오 믹서에 볼륨 설정
        SetBgmVolum(bgmVolume);
        SetSfxVolum(sfxVolume);

        audioSource[0].clip = bgmClips[0]; //첫번째 AudioSource (BGM Audio Source)의 AudioClip에 AudioManager의 첫번째 BGM Clip를 할당 (배경음악)
        audioSource[0].Play(); //해당 AudioSource (BGM Audio Source)의 AudioClip 재생
    }

    //Bgm 볼륨 조절
    public void SetBgmVolum(float volume)
    {
        float bgSound = BGMSilder.value; //현재 BGM 슬라이더의 수치를 할당

        if (bgSound == -40f) AudioMixer.SetFloat("BGM", -80); //해당 변수의 값이 -40f일 때 (슬라이더 최저치일 때) 음소거
        else AudioMixer.SetFloat("BGM", bgSound); // 아닌 경우 슬라이더 값을 오디오 믹서의 BGM 음량에 적용

        // 볼륨 값 저장
        PlayerPrefs.SetFloat("BGMVolume", bgSound); // 해당 값을 레지스트리에 저장
        PlayerPrefs.Save(); // 변경 사항 저장
    }

    //효과음 볼륨 조절
    public void SetSfxVolum(float volume)
    {
        float sfxSound = SFXSilder.value; //현재 효과음 슬라이드의 수치를 할당

        if (sfxSound == -40f) AudioMixer.SetFloat("SFX", -80); //해당 변수의 값이 -40f일 때 (슬라이더 최저치일 때) 음소거
        else AudioMixer.SetFloat("SFX", sfxSound); // 아닌 경우 슬라이더 값을 오디오 믹서의 SFX 음량에 적용

        // 볼륨 값 저장
        PlayerPrefs.SetFloat("SFXVolume", sfxSound); // 해당 값을 레지스트리에 저장
        PlayerPrefs.Save(); // 변경 사항 저장
    }
}

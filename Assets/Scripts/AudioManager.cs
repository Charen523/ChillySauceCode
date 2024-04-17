using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer AudioMixer;
    public Slider BGMSilder;
    public Slider SFXSilder;


    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    public AudioSource[] audioSource;

    float bgmVolume;
    float sfxVolume;

    float bgmSound;
    float sfxSound;

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

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponentsInChildren<AudioSource>();

        // 볼륨 값 불러오기
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        BGMSilder.value = bgmSound;
        SFXSilder.value = sfxSound;

        // 오디오 믹서에 볼륨 설정
        SetBgmVolum(bgmVolume);
        SetSfxVolum(sfxVolume);

        audioSource[0].clip = bgmClips[0];
        audioSource[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
    }
    //Bgm 볼륨 조절
    public void SetBgmVolum(float volume)
    {
        float bgSound = BGMSilder.value;

        if (bgSound == -40f) AudioMixer.SetFloat("BGM", -80);
        else AudioMixer.SetFloat("BGM", bgSound);



    }
    //효과음 볼륨 조절
    public void SetSfxVolum(float volume)
    {
        float sfxSound = SFXSilder.value;

        if (sfxSound == -40f) AudioMixer.SetFloat("SFX", -80);
        else AudioMixer.SetFloat("SFX", sfxSound);

    }



}

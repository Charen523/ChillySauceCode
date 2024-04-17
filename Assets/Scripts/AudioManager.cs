using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioClip[] bgmClips;
    public AudioClip[] sfxClips;

    public AudioSource[] audioSource;

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

        audioSource[0].clip = bgmClips[0];
        audioSource[0].Play();
    }

    // Update is called once per frame
    void Update()
    {
        PauseBGM();
    }
    public void PauseBGM()
    {
        if (PauseBtn.isPaused == true)
        {
            audioSource[0].Pause();
        }
        else if (PauseBtn.isPaused == false)
        {
            audioSource[0].UnPause();
        }
    }

}

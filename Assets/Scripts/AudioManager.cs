using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioMixer AudioMixer; // ����� �ͼ�
    public Slider BGMSilder; // BGM �����̴�
    public Slider SFXSilder; // ȿ���� �����̴�

    public AudioClip[] bgmClips; // BGM Ŭ�� �迭
    public AudioClip[] sfxClips; // ȿ���� Ŭ�� �迭

    public AudioSource[] audioSource; //BGM ����� �ҽ�, ȿ���� ����� �ҽ��� �����ϴ� ����� �ҽ� �迭

    float bgmVolume; // BGM ����
    float sfxVolume; // ȿ���� ����

    /*�ν��Ͻ� ȭ �� �ı� ����*/
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

    /*������ �ʱ�ȭ*/
    void Start()
    {
        audioSource = GetComponentsInChildren<AudioSource>(); //AudioManager �ڽ� �� AudioSource�� ������Ʈ�� �˻� �� ������� �迭�� ����

        // ���� �� �ҷ�����
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume");

        // ����� �ͼ��� ���� ����
        SetBgmVolum(bgmVolume);
        SetSfxVolum(sfxVolume);

        audioSource[0].clip = bgmClips[0]; //ù��° AudioSource (BGM Audio Source)�� AudioClip�� AudioManager�� ù��° BGM Clip�� �Ҵ� (�������)
        audioSource[0].Play(); //�ش� AudioSource (BGM Audio Source)�� AudioClip ���
    }

    //Bgm ���� ����
    public void SetBgmVolum(float volume)
    {
        float bgSound = BGMSilder.value; //���� BGM �����̴��� ��ġ�� �Ҵ�

        if (bgSound == -40f) AudioMixer.SetFloat("BGM", -80); //�ش� ������ ���� -40f�� �� (�����̴� ����ġ�� ��) ���Ұ�
        else AudioMixer.SetFloat("BGM", bgSound); // �ƴ� ��� �����̴� ���� ����� �ͼ��� BGM ������ ����

        // ���� �� ����
        PlayerPrefs.SetFloat("BGMVolume", bgSound); // �ش� ���� ������Ʈ���� ����
        PlayerPrefs.Save(); // ���� ���� ����
    }

    //ȿ���� ���� ����
    public void SetSfxVolum(float volume)
    {
        float sfxSound = SFXSilder.value; //���� ȿ���� �����̵��� ��ġ�� �Ҵ�

        if (sfxSound == -40f) AudioMixer.SetFloat("SFX", -80); //�ش� ������ ���� -40f�� �� (�����̴� ����ġ�� ��) ���Ұ�
        else AudioMixer.SetFloat("SFX", sfxSound); // �ƴ� ��� �����̴� ���� ����� �ͼ��� SFX ������ ����

        // ���� �� ����
        PlayerPrefs.SetFloat("SFXVolume", sfxSound); // �ش� ���� ������Ʈ���� ����
        PlayerPrefs.Save(); // ���� ���� ����
    }
}

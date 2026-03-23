using UnityEngine;
using Utilities;

public class SoundManager : MonoSingleton<SoundManager>
{
    [SerializeField]
    private float _bgmVolume = 0.5f;
    [SerializeField]
    private float _sfxVolume = 1;
   
    private AudioSource _audioSource;

    [SerializeField]
    private AudioClip _bgm;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayBGM();
    }

    public void PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip, _sfxVolume);
    }

    private void PlayBGM()
    {
        if(!_audioSource.isPlaying.Equals(_bgm))
        {
            _audioSource.PlayOneShot(_bgm, _bgmVolume);   
        }
    }

    protected override void OnInit()
    {
        
    }

}

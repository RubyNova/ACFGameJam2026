using GameAudio;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    private SoundManagerConfig _config;


    public void ChangeMasterVolume(float volume)
    {
        _config.AudioMixer.SetFloat("MasterVolume", volume);
    }

    public void ChangeBgmVolume(float volume)
    {
        _config.AudioMixer.SetFloat("BgmVolume", volume);
    }

    public void ChangeSfxVolume(float volume)
    {
        _config.AudioMixer.SetFloat("SfxVolume", volume);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _config = Resources.Load<SoundManagerConfig>("GameAudioConfigData/DefaultSoundConfig");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

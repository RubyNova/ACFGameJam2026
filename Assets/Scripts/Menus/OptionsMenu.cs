using GameAudio;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private SoundManagerConfig _config;

    [SerializeField] private Slider _masterVolumeSlider;
    [SerializeField] private Slider _bgmVolumeSlider;
    [SerializeField] private Slider _sfxVolumeSlider;
    [SerializeField] private GameObject _optionsUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _config = Resources.Load<SoundManagerConfig>("GameAudioConfigData/DefaultSoundConfig");
        LoadVolumeData();
        _optionsUI.SetActive(false);  //  PlayerPrefs not loading in if the options menu was disabled on game start
    }

    public void ChangeMasterVolume(float volume)
    {
        _config.AudioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MasterSavedVolume", volume);
    }

    public void ChangeBgmVolume(float volume)
    {
        _config.AudioMixer.SetFloat("BgmVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("BgmSavedVolume", volume);
    }

    public void ChangeSfxVolume(float volume)
    {
        _config.AudioMixer.SetFloat("SfxVolume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SfxSavedVolume", volume);
    }

    private void LoadVolumeData()
    {
        ChangeMasterVolume(PlayerPrefs.GetFloat("MasterSavedVolume", 1f));
        _masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterSavedVolume");

        ChangeBgmVolume(PlayerPrefs.GetFloat("BgmSavedVolume", 1f));
        _bgmVolumeSlider.value = PlayerPrefs.GetFloat("BgmSavedVolume");

        ChangeSfxVolume(PlayerPrefs.GetFloat("SfxSavedVolume", 1f));
        _sfxVolumeSlider.value = PlayerPrefs.GetFloat("SfxSavedVolume");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}

using UnityEngine;
using Utilities;

namespace GameAudio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private SoundManagerConfig _config;

        private AudioSource _audioSource;

        [SerializeField]
        private AudioSource _audioSource2;

        [SerializeField]
        private AudioClip _startButtonAudio;  //  Didnt want to have to put this in two places (main menu and pause menu)

        public AudioSource SfxAudioSource => _audioSource2;

        void Start()
        {
            LoadVolumeData();
            if (_audioSource != null && _config.Bgm != null)
            {
                PlayBGM();
            }
        }

        public void PlayAudioClip(AudioClip audioClip)
        {
            if(_audioSource2 != null)
            {
                _audioSource2.PlayOneShot(audioClip, _config.SfxVolume);
            }
        }

        public void PlayBGM(bool endOfShift = false)
        {
            if (_audioSource != null) 
            {
                _audioSource.clip = !endOfShift ? _config.Bgm : _config.EndOfShiftBgm;
                _audioSource.volume = _config.BgmVolume;
                _audioSource.loop = true;   
                _audioSource.ignoreListenerPause = true; 
                _audioSource.Play();
            }
        }

        private void LoadVolumeData()
        {
            float tempHolder = 0;

            tempHolder = PlayerPrefs.GetFloat("MasterSavedVolume", 1f);
            _config.AudioMixer.SetFloat("MasterVolume", Mathf.Log10(tempHolder) * 20);

            tempHolder = PlayerPrefs.GetFloat("BgmSavedVolume", 1f);
            _config.AudioMixer.SetFloat("BgmVolume", Mathf.Log10(tempHolder) * 20);

            tempHolder = PlayerPrefs.GetFloat("SfxSavedVolume", 1f);
            _config.AudioMixer.SetFloat("SfxVolume", Mathf.Log10(tempHolder) * 20);
        }

        public void PlayStartAudio()
        {
            PlayAudioClip(_startButtonAudio);
        }

        protected override void OnInit()
        {
            _audioSource = GetComponent<AudioSource>();
            _config = Resources.Load<SoundManagerConfig>("GameAudioConfigData/DefaultSoundConfig");
        }

    }
}

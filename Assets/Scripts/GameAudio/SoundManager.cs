using UnityEngine;
using Utilities;

namespace GameAudio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private SoundManagerConfig _config;

        private AudioSource _audioSource;
        [SerializeField] private AudioSource _audioSource2;

        void Start()
        {
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
    
        protected override void OnInit()
        {
            _audioSource = GetComponent<AudioSource>();
            _config = Resources.Load<SoundManagerConfig>("GameAudioConfigData/DefaultSoundConfig");
        }

    }
}

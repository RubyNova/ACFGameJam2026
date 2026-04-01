using UnityEngine;
using Utilities;

namespace GameAudio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoSingleton<SoundManager>
    {
        private SoundManagerConfig _config;

        private AudioSource _audioSource;

        // Update is called once per frame
        void Update()
        {
            if (_audioSource != null && _config.Bgm != null)
            {
                PlayBGM();
            }
        }

        public void PlayAudioClip(AudioClip audioClip)
        {
            _audioSource.PlayOneShot(audioClip, _config.SfxVolume);
        }

        private void PlayBGM()
        {
            if (!_audioSource.isPlaying.Equals(_config.Bgm))
            {
                _audioSource?.PlayOneShot(_config.Bgm, _config.BgmVolume);
            }
        }

        protected override void OnInit()
        {
            _audioSource = GetComponent<AudioSource>();
            _config = Resources.Load<SoundManagerConfig>("GameAudioConfigData/DefaultSoundConfig");
        }

    }
}

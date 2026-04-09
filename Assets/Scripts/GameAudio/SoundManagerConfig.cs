using UnityEngine;
using UnityEngine.Audio;

namespace GameAudio
{
    [CreateAssetMenu(fileName = "NewSoundManagerConfig", menuName = "GameAudio/Create New SoundManagerConfig", order = 1)]
    public class SoundManagerConfig : ScriptableObject
    {
        
        [SerializeField] private AudioMixer _mixer1;

        [SerializeField]
        private float _bgmVolume = 0.5f;

        [SerializeField]
        private float _sfxVolume = 1;

        [SerializeField]
        private AudioClip _bgm;

        public AudioMixer AudioMixer => _mixer1;
        public float BgmVolume => _bgmVolume;

        public float SfxVolume => _sfxVolume;

        public AudioClip Bgm => _bgm;

    }
}
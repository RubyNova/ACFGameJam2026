using UnityEngine;

namespace GameAudio
{
    [CreateAssetMenu(fileName = "NewSoundManagerConfig", menuName = "GameAudio/Create New SoundManagerConfig", order = 1)]
    public class SoundManagerConfig : ScriptableObject
    {
        [SerializeField]
        private float _bgmVolume = 0.5f;

        [SerializeField]
        private float _sfxVolume = 1;

        [SerializeField]
        private AudioClip _bgm;

        public float BgmVolume => _bgmVolume;

        public float SfxVolume => _sfxVolume;

        public AudioClip Bgm => _bgm;
    }
}
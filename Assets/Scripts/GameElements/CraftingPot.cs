using System;
using System.Collections.Generic;
using CraftingAPI;
using UnityEngine;
using GameAudio;
using UnityEngine.Events;

namespace GameElements
{
    public class CraftingPot : MonoBehaviour
    {
        [SerializeField] private AudioClip _ingredientInPot;
        [SerializeField] private AudioClip _createdPotion;
        [SerializeField] private AudioClip _emptyPot;

        public event Action<AudioClip> playSound;

        [SerializeField]
        private GameObject _itemInstancePrefab;

        private List<ItemConfig> _itemsInPot;

        private GameObject _itemToIgnore;
        private bool _paused = false;

        public bool IsPaused => _paused;

        private void Awake()
        {
            _itemsInPot = new();
            playSound += SoundManager.Instance.PlayAudioClip;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!_paused)
            {
                if (collision.gameObject.TryGetComponent<ItemInstance>(out var instance))
                {
                    _itemsInPot.Add(instance.BackingConfig);
                    playSound?.Invoke(_ingredientInPot);
                    Destroy(collision.gameObject);
                    var result = ItemDatabase.Instance.TryCraft(_itemsInPot);
                    
                    if (result.Success)
                    {
                        playSound?.Invoke(_createdPotion);
                        Instantiate(_itemInstancePrefab).GetComponent<ItemInstance>().InitialiseWithItemConfig(result.Item);
                        _itemsInPot.Clear();
                    }                
                }
            }
        }

        public void EmptyPot()
        {
            playSound.Invoke(_emptyPot);
            _itemsInPot.Clear();
        }
        
        public void FlipPause() => _paused = !_paused;
    }
}

using System;
using System.Collections.Generic;
using CraftingAPI;
using UnityEngine;

namespace GameElements
{
    public class CraftingPot : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _ingredientInPot;
        [SerializeField]
        private AudioClip _createdPotion;

        public event Action<AudioClip> playSound;

        [SerializeField]
        private GameObject _itemInstancePrefab;

        private List<ItemConfig> _itemsInPot;

        private GameObject _itemToIgnore;

        private void Awake()
        {
            _itemsInPot = new();
            _itemToIgnore = null;
            playSound += SoundManager.Instance.PlayAudioClip;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _itemToIgnore)
            {
                return;
            }

            if (collision.gameObject.TryGetComponent<ItemInstance>(out var instance))
            {
                _itemsInPot.Add(instance.BackingConfig);
                playSound?.Invoke(_ingredientInPot);
                Destroy(collision.gameObject);
                var result = ItemDatabase.Instance.TryCraft(_itemsInPot);
                
                if (result.Success)
                {
                    playSound?.Invoke(_createdPotion);
                    _itemToIgnore = Instantiate(_itemInstancePrefab);
                    _itemToIgnore.GetComponent<ItemInstance>().InitialiseWithItemConfig(result.Item);
                    _itemsInPot.Clear();
                }                
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject != _itemToIgnore)
            {
                return;
            }

            _itemToIgnore = null;
        }
    }
}

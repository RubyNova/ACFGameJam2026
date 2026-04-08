using System.Linq;
using ACHNarrativeDriver;
using UnityEngine;

namespace NPC
{
    public class NPCSpawner : MonoBehaviour
    {
        [SerializeField]
        private NPCCharacter[] _charactersToSpawn;

        [SerializeField]
        private GameObject _spawnLocationObject;

        [SerializeField]
        private GameObject _NPCPrefab;

        [SerializeField]
        private AutoNarrativeController _narrativeController;

        [SerializeField]
        private GameObject _mainCraftingUI;

        private bool _characterSpawned = false;
        private bool _noMoreNpcs = false;

        private int _characterSpawnIndex;

        private NPCController _currentCharacterController;

        public bool MoreNPCsAvailable => !_noMoreNpcs;

        void Start()
        {
            _characterSpawnIndex = 0;
        }


        void Update()
        {
            if(!_characterSpawned)
            {
                if(_characterSpawnIndex + 1 > _charactersToSpawn.Count())
                {
                    _characterSpawnIndex = 0;
                    _characterSpawned = true;
                    _noMoreNpcs = true;
                    return;
                }

                var npc = Instantiate(_NPCPrefab, _spawnLocationObject.transform.position, Quaternion.identity, transform);
                _currentCharacterController = npc.GetComponent<NPCController>();
                _currentCharacterController.NarrativeController = _narrativeController;
                _currentCharacterController.InitialiseWithNPCConfiguration(_charactersToSpawn[_characterSpawnIndex], _mainCraftingUI);
                _currentCharacterController.CharacterGoneEvent.AddListener(CharacterGone);
                
                _characterSpawnIndex++;
                _characterSpawned = true;
            }
        }

        public void CharacterGone()
        {
            _currentCharacterController?.CharacterGoneEvent.RemoveListener(CharacterGone);
            _characterSpawned = false;
        }
    }
}

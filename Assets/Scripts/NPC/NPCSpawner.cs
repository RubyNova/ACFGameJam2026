using System.Linq;
using ACHNarrativeDriver;
using GameAudio;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC
{
    public class NPCSpawner : MonoBehaviour
    {
        [SerializeField]
        private bool _challengeMode;
        [SerializeField]
        private bool _storyMode;

        [SerializeField]
        private NPCCharacter[] _randomNpcsToSpawn;

        [SerializeField]
        private NPCCharacter[] _dedicatedNpcsToSpawn;

        [SerializeField]
        private int _numberOfRandomNpcsToSpawn;

        [SerializeField]
        private GameObject _spawnLocationObject;

        [SerializeField]
        private GameObject _NPCPrefab;

        [SerializeField]
        private GameObject _expandedNPCPrefab;

        [SerializeField]
        private AutoNarrativeController _narrativeController;

        [SerializeField]
        private GameObject _mainCraftingUI;

        [SerializeField]
        private AudioClip _customerWalkingAudio;

        private bool _characterSpawned = false;
        private bool _noMoreNpcs = false;

        private int _numberOfRandomNpcsInCollection;
        private int _characterSpawnIndex;
        private int _numberOfRandomNpcsSpawned = 0;
        private int _numberOfDedicatedNpcsToSpawn = 0;
        private int _numberOfDedicatedNpcsSpawned = 0;
        private int _totalExpectedNpcsToSpawn = 0;
        private float _informedItemsDiscoveredValue = 0f;
        private float _informedItemsCraftedValue = 0f;
        private float _informedNpcSuccessRateValue = 0f;
        private float _skippedDedicatedNpcs = 0f;
        private int _goodDeliveries;
        private int _badDeliveries;

        private NPCController _currentCharacterController;

        public bool MoreNPCsAvailable => !_noMoreNpcs;
        public float NPCSuccessRate => _informedNpcSuccessRateValue;
        public int ItemsDeliveredSuccessfully => _goodDeliveries;
        public int ItemsDelivered => _goodDeliveries + _badDeliveries;

        void Start()
        {
            _characterSpawnIndex = 0;
            _numberOfRandomNpcsInCollection = _randomNpcsToSpawn.Count();
            _numberOfDedicatedNpcsToSpawn = _dedicatedNpcsToSpawn.Count();
        
            _totalExpectedNpcsToSpawn = _numberOfDedicatedNpcsToSpawn + _numberOfRandomNpcsToSpawn;

            if (_totalExpectedNpcsToSpawn < 0)
            {
                throw new System.Exception("Total NPC count for this level cannot be 0!");
            }
        }

        void Update()
        {
            if(!_characterSpawned)
            {
                if(!_challengeMode)
                {
                    if(
                        (_numberOfDedicatedNpcsToSpawn <= _skippedDedicatedNpcs + _numberOfDedicatedNpcsSpawned) &&
                        (_numberOfRandomNpcsToSpawn <= _numberOfRandomNpcsSpawned)
                    )
                    {
                        _characterSpawnIndex = 0;
                        _characterSpawned = true;
                        _noMoreNpcs = true;
                        return;
                    }
                }
                var character = DetermineNextNpc();
                SpawnCharacter(character);
            }

            if(!_challengeMode)
            {
                _informedNpcSuccessRateValue = _goodDeliveries / _totalExpectedNpcsToSpawn;
            }
            else if (ItemsDelivered > 0)
            {
                _informedNpcSuccessRateValue = _goodDeliveries / ItemsDelivered;
            }
            else
            {
                _informedNpcSuccessRateValue = 0;
            }
            
        }

        public void CharacterGone(bool happyCustomer)
        {
            _currentCharacterController?.CharacterGoneEvent.RemoveListener(CharacterGone);
            _characterSpawned = false;
            if(happyCustomer)
            {
                _goodDeliveries++;
            }
            else
            {
                _badDeliveries++;
            }
        }

        public void UpdateItemsCraftedValue(float value) => _informedItemsCraftedValue = value;
        
        public void UpdateItemsDeliveredValue(float value) => _informedItemsDiscoveredValue = value;

        private NPCCharacter DetermineNextNpc()
        {
            if(!_challengeMode)
            {
                if (
                    (_numberOfDedicatedNpcsToSpawn > 0  && _characterSpawnIndex < _numberOfDedicatedNpcsToSpawn && _dedicatedNpcsToSpawn[_characterSpawnIndex].NumberOfRandomNpcAppearancesBeforeAllowedToShow == 0) ||
                    (_numberOfRandomNpcsToSpawn == 0 && _numberOfDedicatedNpcsToSpawn == 1))
                {
                    var character = _dedicatedNpcsToSpawn[_characterSpawnIndex];
                    _characterSpawnIndex++;
                    _numberOfDedicatedNpcsSpawned++;
                    return character;
                }
                else if (_numberOfRandomNpcsSpawned <= 0 || 
                    _numberOfDedicatedNpcsToSpawn <= 0 || 
                    _numberOfDedicatedNpcsSpawned >= _numberOfDedicatedNpcsToSpawn)
                {
                    _numberOfRandomNpcsSpawned++;
                    return _randomNpcsToSpawn[Random.Range(0, _numberOfRandomNpcsInCollection)];
                }
                else
                {
                    var character = _dedicatedNpcsToSpawn[_characterSpawnIndex];
                    if(_numberOfRandomNpcsSpawned >= character.NumberOfRandomNpcAppearancesBeforeAllowedToShow)
                    {
                        switch(character.ConditionForAppearing.Condition)
                        {
                            case NPCAppearanceConditionType.None:
                            {
                                _characterSpawnIndex++;
                                _numberOfDedicatedNpcsSpawned++;
                                return character;
                            }
                            case NPCAppearanceConditionType.ItemsDiscovered:
                            case NPCAppearanceConditionType.ItemsCrafted:
                            case NPCAppearanceConditionType.NPCSuccessRate:
                            {
                                float valueToCompare = GetComparingValue(character.ConditionForAppearing.Condition);
                                if(ValueComparision(valueToCompare, character.ConditionForAppearing.ComparisonValue, character.ConditionForAppearing.ComparisonType))
                                {
                                    _characterSpawnIndex++;
                                    _numberOfDedicatedNpcsSpawned++;
                                    return character;
                                }

                                //if here we skip the npcs;
                                _skippedDedicatedNpcs++;
                                _characterSpawnIndex++;

                                break;
                            }
                        }
                    }
                    _numberOfRandomNpcsSpawned++;
                    return _randomNpcsToSpawn[Random.Range(0, _numberOfRandomNpcsInCollection-1)];
                }
            }
            else
            {
                _numberOfRandomNpcsSpawned++;
                return _randomNpcsToSpawn[Random.Range(0, _numberOfRandomNpcsInCollection)];
            }
        }

        private float GetComparingValue(NPCAppearanceConditionType valueType)
        {
            switch(valueType)
            {
                case NPCAppearanceConditionType.ItemsDiscovered:
                {
                    return _informedItemsDiscoveredValue;
                }
                case NPCAppearanceConditionType.ItemsCrafted:
                {
                    return _informedItemsCraftedValue;
                }
                case NPCAppearanceConditionType.NPCSuccessRate:
                {
                    return _informedNpcSuccessRateValue;
                }
                default:
                {
                    return 0f;
                }
            }
        }
        
        private bool ValueComparision(float currentValue, float valueToCompareTo, NPCAppearanceComparisonType comparisonType)
        {
            switch(comparisonType)
            {
                case NPCAppearanceComparisonType.EqualTo:
                {
                    return currentValue == valueToCompareTo;
                }
                case NPCAppearanceComparisonType.NotEqualTo:
                {
                    return currentValue != valueToCompareTo;
                }
                case NPCAppearanceComparisonType.GreaterThanOrEqualTo:
                {
                    return currentValue >= valueToCompareTo;
                }
                case NPCAppearanceComparisonType.LessThanOrEqualTo:
                {
                    return currentValue <= valueToCompareTo;
                }
                case NPCAppearanceComparisonType.LessThan:
                {
                    return currentValue < valueToCompareTo;
                }
                case NPCAppearanceComparisonType.GreaterThan:
                {
                    return currentValue > valueToCompareTo;
                }
                default:
                {
                    return true;
                }
            }
        }

        private void SpawnCharacter(NPCCharacter character)
        {
                var prefab = character.UseExpandedSprite ? _expandedNPCPrefab : _NPCPrefab;
                var npc = Instantiate(prefab, _spawnLocationObject.transform.position, Quaternion.identity, transform);
                _currentCharacterController = npc.GetComponent<NPCController>();
                _currentCharacterController.NarrativeController = _narrativeController;
                _currentCharacterController.StoryMode = _storyMode;
                _currentCharacterController.InitialiseWithNPCConfiguration(character, _mainCraftingUI);
                _currentCharacterController.CharacterGoneEvent.AddListener(CharacterGone);

                _characterSpawned = true;
                //SoundManager.Instance.PlayAudioClip(_customerWalkingAudio);
            
        }
    }
}

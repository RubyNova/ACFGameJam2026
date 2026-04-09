using System;
using System.Linq;
using ACHNarrativeDriver;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC
{
    public class NPCSpawner : MonoBehaviour
    {
        [SerializeField]
        private NPCCharacter[] _randomNpcsToSpawn;

        [SerializeField]
        private NPCCharacter[] _dedicatedNpcsToSpawn;

        [SerializeField]
        private int _numberOfRandomNpcsToSpawn;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float _randomizationThreshold = 0.5f;

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
            if (_numberOfRandomNpcsToSpawn > _numberOfRandomNpcsInCollection)
            {
                throw new System.Exception("Set number of NPCs to spawn does not match random NPC count for this level!");
            }

            _totalExpectedNpcsToSpawn = _numberOfDedicatedNpcsToSpawn + _numberOfRandomNpcsToSpawn;
        }

        void Update()
        {
            if(!_characterSpawned)
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

                var character = DetermineNextNpc();
                SpawnCharacter(character);
            }

            _informedNpcSuccessRateValue = _goodDeliveries / _totalExpectedNpcsToSpawn;
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
            if (_numberOfRandomNpcsSpawned <= 0 || 
                _numberOfDedicatedNpcsToSpawn <= 0 || 
                _numberOfDedicatedNpcsSpawned >= _numberOfDedicatedNpcsToSpawn)
            {
                _numberOfRandomNpcsSpawned++;
                return _randomNpcsToSpawn[Random.Range(0, _numberOfRandomNpcsInCollection-1)];
            }
            else
            {
                if(Random.value >= _randomizationThreshold)
                {
                    var character = _dedicatedNpcsToSpawn[_characterSpawnIndex];
                    
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
                var npc = Instantiate(_NPCPrefab, _spawnLocationObject.transform.position, Quaternion.identity, transform);
                _currentCharacterController = npc.GetComponent<NPCController>();
                _currentCharacterController.NarrativeController = _narrativeController;
                _currentCharacterController.InitialiseWithNPCConfiguration(character, _mainCraftingUI);
                _currentCharacterController.CharacterGoneEvent.AddListener(CharacterGone);

                _characterSpawned = true;
            
        }
    }
}

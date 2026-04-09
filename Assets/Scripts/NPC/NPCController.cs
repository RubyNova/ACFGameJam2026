using System.Collections;
using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using GameElements;
using UnityEngine;
using UnityEngine.Events;

namespace NPC
{
    public class NPCController : MonoBehaviour
    {
        // [Header("Data Fields")]
        // [SerializeField]
        // private float _delayBeforeStartingDialogue = 0.0f;

        // [SerializeField]
        // private NPCPerformanceData _performanceData;

        [Header("Required Objects")]
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private Collider2D _collider;

        private GameObject _mainCrafingUI;

        private const string _spawnOutTriggerName = "Leaving";

        public UnityEvent<bool> CharacterGoneEvent = new();
        public UnityEvent ItemDeliveredEvent = new();
        public AutoNarrativeController NarrativeController;
        //TODO: refactor these bools into an enum or something l8r
        private bool _entering = false;
        private bool _hasEntered = false;
        private bool _introduction = false;
        private bool _leaving = false;
        private bool _beingServed = false;
        private bool _paused = false;
        private float _deltaTimeSeconds = 0.0f;
        private int _badDeliveries = 0;
        public bool HappyCustomer => _badDeliveries < NPCConfiguration.NegativeInteractionsBeforeLeavingUnhappy;

        public NPCCharacter NPCConfiguration { get; private set; }

        public void InitialiseWithNPCConfiguration(NPCCharacter npc, GameObject mainCraftingUI)
        {
            _mainCrafingUI = mainCraftingUI;
            NPCConfiguration = npc;
            _spriteRenderer.sprite = NPCConfiguration.IdleSprite;
            _animator.runtimeAnimatorController = NPCConfiguration.AnimController;
        }

        void Update()
        {
            if(!_paused)
            {
                //animator state
                if(!_hasEntered)
                {
                    _entering = _animator.GetCurrentAnimatorStateInfo(0).IsName(NPCConfiguration.SpawnInAnimationClip.name);
                    _collider.enabled = false;
                    if(!_entering)
                    {
                        _hasEntered = true;
                        _introduction = true;
                    }  
                } 

                //check logic
                if(_introduction)
                {
                    _collider.enabled = false;
                    NarrativeController?.Finished.AddListener(PrepForServing);
                    NarrativeController?.ExecuteSequence(NPCConfiguration.ArrivalSequence);
                    _introduction = false;
                }

                if(_leaving && NPCConfiguration.DepartingSequence != null)
                {
                    _collider.enabled = false;
                    NarrativeController.Finished.AddListener(RunGoodbyeAnim);

                    if (NPCConfiguration.NegativeInteractionsBeforeLeavingUnhappy <= _badDeliveries)
                    {
                        NarrativeController.ExecuteSequence(NPCConfiguration.NegativeSequences[Random.Range(0, NPCConfiguration.NegativeSequenceCount)]);
                    }
                    else
                    {
                        NarrativeController.ExecuteSequence(NPCConfiguration.DepartingSequence);
                    }
                    
                    _leaving = false;
                }
                else if(_leaving)
                {
                    _leaving = false;
                }

                if(_beingServed)
                {
                    _deltaTimeSeconds += Time.deltaTime;
                }
            }
        }

        public void LaunchNPCArrivalDialogue()
        {
            StartCoroutine(LaunchDialogueOnDelay());
        }

        public void LaunchNPCDepartureDialogue()
        {
            _leaving = true;
        }

        private IEnumerator LaunchDialogueOnDelay()
        {
            yield return new WaitForSeconds(NPCConfiguration.DelayBeforeStartingDialogue);
            _introduction = true;
        }

        private void PrepForServing(bool _)
        {
            NarrativeController.Finished.RemoveListener(PrepForServing);
            _mainCrafingUI.SetActive(true);
            _collider.enabled = true;
            _beingServed = true;
        }

        private void RunGoodbyeAnim(bool _)
        {
            NarrativeController.Finished.RemoveListener(RunGoodbyeAnim);
            _animator.SetBool(_spawnOutTriggerName, true);
        }
    
        public void DeleteSelf()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(!_paused)
            {
                if(collision.gameObject.TryGetComponent<ItemInstance>(out var itemInstance))
                {
                    ItemDeliveredEvent.Invoke();
                    if(!NPCConfiguration.DesiredItem.ItemName.Equals(itemInstance.BackingConfig.ItemName, System.StringComparison.InvariantCultureIgnoreCase))
                    {
                        if(NPCConfiguration.NegativeSequenceCount > 0)
                        {
                            var sequence = NPCConfiguration.NegativeSequences[Random.Range(0, NPCConfiguration.NegativeSequenceCount - 1)];
                            
                            if(NarrativeController.SequenceIsPlaying)
                            {
                                NarrativeController.EndCurrentSequence();
                            }
                            
                            NarrativeController.ExecuteSequence(sequence);
                            
                            
                            if(_badDeliveries < NPCConfiguration.NegativeInteractionsBeforeLeavingUnhappy)
                            {
                                _badDeliveries++;
                            }
                            else
                            {
                                _beingServed = false;
                                _leaving = true;    
                                _mainCrafingUI.SetActive(false);
                            }
                        }
                    }
                    else
                    {
                        _beingServed = false;
                        _leaving = true;    
                        _mainCrafingUI.SetActive(false);
                    }
                }

                Destroy(collision.gameObject);
            }
        }

        public void FlipPause() => _paused = !_paused;
    }
}

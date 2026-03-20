using System.Collections;
using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using GameElements;
using UnityEngine;

namespace NPC
{
    public class NPCController : MonoBehaviour
    {
        [Header("Data Fields")]
        [SerializeField]
        private float _delayBeforeStartingDialogue = 0.0f;

        [SerializeField]
        private NPCPerformanceData _performanceData;

        [Header("Required Objects")]
        [SerializeField]
        private AutoNarrativeController _narrativeController;
        
        [SerializeField]
        private NarrativeSequence _arrivalNarrativeSequence;
        
        [SerializeField]
        private NarrativeSequence _departureNarrativeSequence;
        
        [SerializeField]
        private Animator _animator;

        [Header("Reference Objects")]
        [SerializeField]
        private AnimationClip _SpawnInAnimation;

        private const string _spawnOutTriggerName = "Leaving";


        //TODO: refactor these bools into an enum or something l8r
        private bool _entering = false;
        private bool _hasEntered = false;
        private bool _introduction = false;
        private bool _leaving = false;
        private bool _beingServed = false;
        private float _deltaTimeSeconds = 0.0f;


        void Update()
        {
            //animator state
            if(!_hasEntered)
            {
                _entering = _animator.GetCurrentAnimatorStateInfo(0).IsName(_SpawnInAnimation.name);
                if(!_entering)
                {
                    _hasEntered = true;
                    _introduction = true;
                }  
            } 

            //check logic
            if(_introduction)
            {
                _narrativeController?.Finished.AddListener(PrepForServing);
                _narrativeController?.ExecuteSequence(_arrivalNarrativeSequence);
                _introduction = false;
            }

            if(_leaving && _departureNarrativeSequence != null)
            {
                _narrativeController.Finished.AddListener(RunGoodbyeAnim);
                _narrativeController?.ExecuteSequence(_departureNarrativeSequence);
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
            yield return new WaitForSeconds(_delayBeforeStartingDialogue);

            _introduction = true;
        }

        private void PrepForServing()
        {
            _narrativeController.Finished.RemoveListener(PrepForServing);
            _beingServed = true;
        }

        private void RunGoodbyeAnim()
        {
            _narrativeController.Finished.RemoveListener(RunGoodbyeAnim);
            _animator.SetBool(_spawnOutTriggerName, true);
        }
    
        public void DeleteSelf()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.gameObject.GetComponent<ItemInstance>() != null)
            {
                //item logic checks can go here but for now we'll just accept anything
                Destroy(collision.gameObject);
                _beingServed = false;
                _leaving = true;
            }
        }
    }
}

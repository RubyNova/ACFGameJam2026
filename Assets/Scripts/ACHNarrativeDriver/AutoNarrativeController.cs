using System.Collections;
using System.Text;
using ACHNarrativeDriver.Api;
using ACHNarrativeDriver.ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ACHNarrativeDriver
{
    public class AutoNarrativeController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _TextBox;
        [SerializeField] private GameObject _TextBubblePrefab;
        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private GameObject _nextButton;
        [SerializeField] private GameObject _dialoguePanel;
        [SerializeField] private UnityEvent _preNarrativeEvent;
        [SerializeField] private UnityEvent _postNarrativeEvent;
        
        //private AudioController _audioController; // this is such a hack reeeeeeee
        public UnityEvent listNextEvent;

        private Coroutine _rollingTextRoutine;
        private readonly WaitForSeconds _rollingCharacterTime = new(0.04f);
        private NarrativeSequence _currentNarrativeSequence;
        private bool _isCurrentlyExecuting;
        private bool _nextDialogueLineRequested;
        private int _currentDialogueIndex;
        private Interpreter _narrativeInterpreter;
        private RuntimeVariables _narrativeRuntimeVariables;
        
        public NarrativeSequence LastPlayedSequence { get; private set; }
        public bool SequenceIsPlaying => _isCurrentlyExecuting;
        
        public UnityEvent Finished;
        
        private void Awake()
        {
            Finished = new();
            _isCurrentlyExecuting = false;
            _currentDialogueIndex = 0;
            _narrativeInterpreter = new();
            _narrativeRuntimeVariables = FindFirstObjectByType<RuntimeVariables>();

            _dialoguePanel.SetActive(false);
        }

        private void Update()
        {
            if (!_isCurrentlyExecuting || !_nextDialogueLineRequested)
            {
                return;
            }

            if (_currentDialogueIndex >= _currentNarrativeSequence.CharacterDialoguePairs.Count && _rollingTextRoutine is null)
            {
                _currentDialogueIndex = 0;

                if (_currentNarrativeSequence.NextSequence is null)
                {
                    LastPlayedSequence = _currentNarrativeSequence;
                    listNextEvent.Invoke();
                    _dialoguePanel.SetActive(false);
                    _isCurrentlyExecuting = false;
                    _currentNarrativeSequence = null;
                    Finished.Invoke();
                    return;
                }

                _currentNarrativeSequence = _currentNarrativeSequence.NextSequence;
            }

            _nextDialogueLineRequested = false;

            if (_rollingTextRoutine is not null)
            {
                ResetRollingTextRoutine();
                _TextBox.text =
                    _currentNarrativeSequence.CharacterDialoguePairs[_currentDialogueIndex - 1].Text;
                return;
            }

            var characterDialogueInfo = _currentNarrativeSequence.CharacterDialoguePairs[_currentDialogueIndex];

            _TextBubblePrefab.SetActive(true);
             
            _rollingTextRoutine =
                StartCoroutine(
                    PerformRollingText(characterDialogueInfo));

            _currentDialogueIndex++;
        }

        private void ResetRollingTextRoutine()
        {
            StopCoroutine(_rollingTextRoutine);
            _rollingTextRoutine = null;
        }

        private IEnumerator PerformRollingText(NarrativeSequence.CharacterDialogueInfo targetDialogueInfo)
        {
            StringBuilder sb = new();

            var resolvedText = _narrativeInterpreter.ResolveRuntimeVariables(targetDialogueInfo.Text, _narrativeRuntimeVariables != null ? _narrativeRuntimeVariables.ReadOnlyVariableView : null);

            foreach (var character in resolvedText)
            {
                sb.Append(character);
                _TextBox.text = sb.ToString();
                yield return _rollingCharacterTime;
            }

            if(targetDialogueInfo.DelayBeforeContinuingInSeconds > -0.1f)
            {
                yield return new WaitForSeconds(targetDialogueInfo.DelayBeforeContinuingInSeconds);
                ExecuteNextDialogueLine();
            }

            _rollingTextRoutine = null;
        }

        public void ExecuteSequence(NarrativeSequence targetSequence)
        {
            if (_rollingTextRoutine is not null)
            {
                ResetRollingTextRoutine();
            }

            if(_preNarrativeEvent is not null)
                _preNarrativeEvent.Invoke();

            _dialoguePanel.SetActive(true);
            _TextBox.text = string.Empty;
            _currentNarrativeSequence = targetSequence;
            _currentDialogueIndex = 0;
            _nextDialogueLineRequested = true;
            _isCurrentlyExecuting = true;
        }

        public void EndCurrentSequence()
        {
            _currentDialogueIndex = _currentNarrativeSequence.CharacterDialoguePairs.Count; 
            ResetRollingTextRoutine();
            _currentNarrativeSequence.NextSequence = null;
        }

        public void ExecuteNextDialogueLine()
        {
            if (_currentNarrativeSequence is null)
            {
                return;
            }

            _nextDialogueLineRequested = true;
        }
    }
}
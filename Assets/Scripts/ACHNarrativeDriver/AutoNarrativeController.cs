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
        [SerializeField] private float _delayBetweenDialogLines;
        [SerializeField] private TMP_Text _TextBox;
        [SerializeField] private GameObject _TextBubblePrefab;
        [SerializeField] private TMP_Text _MainCharacterText;
        [SerializeField] private GameObject _MainCharacterTextPrefab;

        [SerializeField] private GameObject _buttonPrefab;
        [SerializeField] private GameObject _nextButton;
        [SerializeField] private GameObject _dialoguePanel;
        public UnityEvent PreNarrativeEvent;
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
        
        public UnityEvent<bool> Finished;
        private bool _isEndForCharacter = false;
        
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
                    Finished.Invoke(_isEndForCharacter);
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

            if(characterDialogueInfo.NarratorSpeaking)
            {
                _MainCharacterTextPrefab.SetActive(true);
                _TextBubblePrefab.SetActive(false);
            }
            else
            {
                _MainCharacterTextPrefab.SetActive(false);
                _TextBubblePrefab.SetActive(true);                
            }
             
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

            for (int i = 0; i < resolvedText.Length; i++)
            {
                char character = resolvedText[i];
                sb.Append(character);

                if (character == '<' && (resolvedText.Length > i + 2 && resolvedText[i + 2] == '>') || (resolvedText.Length > i + 3 && resolvedText[i + 3] == '>'))
                {
                    sb.Append(resolvedText[i + 1]);
                    sb.Append(resolvedText[i + 2]);

                    if (resolvedText[i + 1] == '/')
                    {
                        sb.Append(resolvedText[i + 3]);
                        i += 3;
                    }
                    else
                    {
                        i += 2;
                    }
                }
                
                if(targetDialogueInfo.NarratorSpeaking)
                {
                    _MainCharacterText.text = sb.ToString();
                }
                else
                {
                    _TextBox.text = sb.ToString();
                }
                yield return _rollingCharacterTime;
            }

            if (targetDialogueInfo.DelayBeforeContinuingInSeconds > 0.0f)
            {
                if (targetDialogueInfo.DelayBeforeContinuingInSeconds > -0.1f)
                {
                    yield return new WaitForSeconds(targetDialogueInfo.DelayBeforeContinuingInSeconds);
                    ExecuteNextDialogueLine();
                }
            }
            else
            {
                yield return new WaitForSeconds(_delayBetweenDialogLines);
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

            if(PreNarrativeEvent is not null)
                PreNarrativeEvent.Invoke();
            _isEndForCharacter = targetSequence.IsDepartingSequence;
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
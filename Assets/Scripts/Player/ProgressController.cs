using ACHNarrativeDriver;
using CraftingAPI;
using GameElements;
using UnityEngine;
using NPC;
using TMPro;
using GameAudio;
using Utilities;
using Saveables;
using System.Linq;

namespace Player
{
    public class ProgressController : MonoBehaviour
    {
        private int _itemsDiscovered;
        private int _itemsCrafted;
        private int _itemsDelivered;
        private int _itemsDeliveredSuccessfully;
        private float _timeRemainingInSeconds;
        private bool _paused;

        [Header("Progress Settings")]
        [SerializeField]
        private bool _perfMode = false;

        [SerializeField]
        private bool _challengeMode = false;

        [SerializeField]
        private int _scoreMultiplierForItemsCrafted = 10;
        
        [SerializeField]
        private int _scoreMultiplierForItemsDiscovered = 250;
        
        [SerializeField]
        private int _scoreMultiplierForTimeRemaining = 100;
        
        [SerializeField]
        private int _scoreMultiplierForCustomerHappiness = 100;

        [Header("Required Objects")]

        [SerializeField]
        private string _bestEndingSceneName;

        [SerializeField]
        private string _okayEndingSceneName;

        [SerializeField]
        [Range(0, 100)]
        private int _customerHappinessPercentageForBestEnding;

        [SerializeField]
        private int _levelNumber;

        [SerializeField]
        private GameClock _clock;

        [SerializeField]
        private AutoNarrativeController _narrative;
        
        [SerializeField]
        private WorldController _playerController;

        [SerializeField]
        private CraftingPot _pot;

        [SerializeField]
        private NPCSpawner _spawner;
        
        [SerializeField]
        private GameObject _levelOverObject;
        
        [SerializeField]
        private TMP_Text _timeRemainingText;

        [SerializeField]
        private TMP_Text _itemsCraftedText;

        [SerializeField]
        private TMP_Text _itemsDiscoveredText;

        [SerializeField]
        private TMP_Text _customerHappinessRateText;

        [SerializeField]
        private GameObject _customerHappinessRateLabel;
        
        [SerializeField]
        private TMP_Text _scoreText;

        [SerializeField]
        private GameObject _scoreLabel;

        [SerializeField]
        private GameObject _recipeBookUI;

        public string Mode => _challengeMode ? "Challenge" : _perfMode ? "Perf Tracking" : "w/o Perf Tracking";

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            ItemDatabase.Instance.ItemCraftAttempt.AddListener(IncrementItemsCrafted);
            ItemDatabase.Instance.ItemDiscovered += IncrementItemsDiscovered;
            _narrative.Finished.AddListener(ResumeObjects);
            _narrative.PreNarrativeEvent.AddListener(SetPause);
        }

        // Update is called once per frame
        void Update()
        {
            if(_paused)
            {
                if(!_clock.IsPaused)
                {
                    _clock.StopTimer(true, false);
                }
                if(!_playerController.IsPaused)
                {
                    _playerController.FlipPause();
                }
                if(!_pot.IsPaused)
                {
                    _pot.FlipPause();
                }
            }
            else
            {
                _timeRemainingInSeconds = _clock.RemainingTimeInSeconds;
                
                if((!_spawner.MoreNPCsAvailable && !_challengeMode) || _timeRemainingInSeconds <= 0)
                {
                    LevelOverProcess();
                }
            }

            _itemsDelivered = _spawner.ItemsDelivered;
            _itemsDeliveredSuccessfully = _spawner.ItemsDeliveredSuccessfully;
        }

        void OnDestroy()
        {
            _narrative?.Finished.RemoveListener(ResumeObjects);
            ItemDatabase.Instance.ItemDiscovered -= IncrementItemsDiscovered;
            ItemDatabase.Instance.ItemCraftAttempt.RemoveListener(IncrementItemsCrafted);
        }

        private void ResumeObjects(bool isFinished)
        {
            if(_clock.IsPaused)
            {
                _clock.StartTimer();
            }
            if(_playerController.IsPaused && !isFinished && !_recipeBookUI.activeInHierarchy)
            {
                _playerController.FlipPause();
            }
            if(_pot.IsPaused)
            {
                _pot.FlipPause();
            }

            FlipPause();
        }

        public void FlipPause() => _paused = !_paused;
        
        public void SetPause() => _paused = true;

        public void IncrementItemsDiscovered(ItemConfig _) => _itemsDiscovered++;
        
        public void IncrementItemsCrafted()
        { 
            _itemsCrafted++;
            _spawner.UpdateItemsCraftedValue(_itemsCrafted);
        }

        public void SetTimeRemainingInSeconds(float timeRemaining) => _timeRemainingInSeconds = timeRemaining;

        private int GenerateScore()
        {
            int score = 0;
            if(_timeRemainingInSeconds > 0)
            {
                score += (int)(_timeRemainingInSeconds * _scoreMultiplierForTimeRemaining);
            }

            score += (int)(_itemsCrafted * _scoreMultiplierForItemsCrafted);
            score += (int)(_itemsDiscovered * _scoreMultiplierForItemsDiscovered);
            
            int percent = (int)(_itemsDeliveredSuccessfully / _itemsDelivered * 100);
            if(percent >= 50)
            {
                int overage = 100 - percent;
                score += overage * _scoreMultiplierForCustomerHappiness;
            }

            return score;
        }

        private void LevelOverProcess()
        {
            //Stop controls
            _clock.StopTimer(false, true);
            _playerController.FlipPause();
            _pot.FlipPause();
            _paused = true;

            //Change Audio
            SoundManager.Instance.PlayBGM(true);

            if(!_challengeMode)
            {
                _timeRemainingText.text = $"{(int)_timeRemainingInSeconds}s";
            }

            _itemsCraftedText.text = $"{_itemsCrafted}";
            _itemsDiscoveredText.text = $"{_itemsDiscovered}";

            int customerHappinessAmt = (int)((_itemsDeliveredSuccessfully / _itemsDelivered)*100);

            var score = GenerateScore();
            if(_perfMode)
            {
                _customerHappinessRateLabel.SetActive(true);
                _customerHappinessRateText.gameObject.SetActive(true);
                _scoreLabel.SetActive(true);
                _scoreText.gameObject.SetActive(true);

                _customerHappinessRateText.text = $"{customerHappinessAmt} %";
                _scoreText.text = $"{score}";
            }
            else
            {
                _customerHappinessRateLabel.SetActive(false);
                _customerHappinessRateText.gameObject.SetActive(false);
                _scoreLabel.SetActive(false);
                _scoreText.gameObject.SetActive(false);
            }
            
            SaveStoryScores(_levelNumber, (int)_timeRemainingInSeconds, _itemsCrafted, _itemsDiscovered, customerHappinessAmt, score);

            _levelOverObject.SetActive(true);
        }

        public void ContinueToNextLevel(string sceneName)
        {
            LevelManager.Instance.LoadScene(sceneName);
        }

        public void ContinueToEndScene()
        {
            var run = PreferencesManager.Instance.GetLatestRun();
            var overallHappiness = run.levelScores.Average(level => level.CustomerHappinessPercentage);

            if(overallHappiness >= _customerHappinessPercentageForBestEnding)
            {
                ContinueToNextLevel(_bestEndingSceneName);
            }
            else
            {
                ContinueToNextLevel(_okayEndingSceneName);
            }
        }

        private void SaveStoryScores(int level, int timeRemaining, int crafted, int discovered, int customerHappinessAmt, float score)
        {
            LevelScore levelData = new()
            {
              Level = level,
              TimeRemaining = timeRemaining,
              ItemsCrafted = _itemsCrafted,
              ItemsDiscovered = _itemsDiscovered,
              CustomerHappinessPercentage = customerHappinessAmt,
              TotalScore = score  
            };

            var run = PreferencesManager.Instance.GetLatestRun();
            run.levelScores.Add(levelData);

            PreferencesManager.Instance.SaveScores(run);
        }
    }
}
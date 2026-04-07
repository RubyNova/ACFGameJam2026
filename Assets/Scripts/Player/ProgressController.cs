using ACHNarrativeDriver;
using CraftingAPI;
using GameElements;
using UnityEngine;

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

        [SerializeField]
        private GameClock _clock;

        [SerializeField]
        private AutoNarrativeController _narrative;
        
        [SerializeField]
        private WorldController _playerController;

        [SerializeField]
        private CraftingPot _pot;

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
            }
        }

        void OnDestroy()
        {
            _narrative?.Finished.RemoveListener(ResumeObjects);
            ItemDatabase.Instance.ItemDiscovered -= IncrementItemsDiscovered;
            ItemDatabase.Instance.ItemCraftAttempt.RemoveListener(IncrementItemsCrafted);
        }

        private void ResumeObjects()
        {
            if(_clock.IsPaused)
            {
                _clock.StartTimer();
            }
            if(_playerController.IsPaused)
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
        
        public void IncrementItemsDelivered() => _itemsDelivered++;
        
        public void IncrementItemsDeliveredSuccessfully() => _itemsDeliveredSuccessfully++;
        
        public void IncrementItemsCrafted() => _itemsCrafted++;

        public void SetTimeRemainingInSeconds(float timeRemaining) => _timeRemainingInSeconds = timeRemaining;
    }
}
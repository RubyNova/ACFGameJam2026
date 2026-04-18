using UnityEngine;
using CraftingAPI;
using System;
using System.Collections.Generic;
using GameAudio;

namespace GameUI
{
    public class RecipeBookController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _craftableItemDisplayPrefab;

        [SerializeField]
        private RectTransform _listParent;

        [SerializeField]
        private RectTransform _listScrollView;

        [SerializeField]
        private GameObject _closeButton;

        [SerializeField]
        private GameObject _backButton;

        [SerializeField]
        private ItemDetailsController _detailsController;

        [SerializeField]
        private AudioClip _openRecipeBook;

        [SerializeField]
        private AudioClip _closeRecipeBook;

        private Stack<Action> _browseHistory;

        public event Action RecipeUIActive;
        public event Action RecipeUIInactive;

        private void Awake()
        {
            _browseHistory = new();
        }

        private void Start()
        {

            ItemDatabase.Instance.ItemDiscovered += OnItemDiscovered;

            foreach (var item in ItemDatabase.Instance.DiscoveredItems)
            {
                OnItemDiscovered(item);
            }
        }

        private void OnEnable()
        {
            SoundManager.Instance.PlayAudioClip(_openRecipeBook);
            RecipeUIActive?.Invoke();
        }

        private void OnDisable()
        {
            SoundManager.Instance.PlayAudioClip(_closeRecipeBook);
            RecipeUIInactive?.Invoke();
        }

        private void OnDestroy()
        {
            if (RecipeUIActive is not null)
            {
                foreach (var actionObject in RecipeUIActive.GetInvocationList())
                {
                    RecipeUIActive -= (Action)actionObject;
                }
            }

            if (RecipeUIInactive is not null)
            {
                foreach (var actionObject in RecipeUIInactive.GetInvocationList())
                {
                    RecipeUIInactive -= (Action)actionObject;
                }
            }

            ItemDatabase.Instance.ItemDiscovered -= OnItemDiscovered;
        }

        private void OnItemDiscovered(ItemConfig item)
        {
            var newObject = Instantiate(_craftableItemDisplayPrefab, _listParent);
            newObject.GetComponent<CraftableItemDisplayController>().Init(item, this);
        }

        public void OpenCraftableItemInfoAndLeaveItemList(ItemConfig item)
        {
            CacheBrowseBackAction(() =>
            {
                _listScrollView.gameObject.SetActive(true);
                _detailsController.gameObject.SetActive(false);
                _closeButton.gameObject.SetActive(true);
                _backButton.gameObject.SetActive(true);
            });

            LeaveItemListNoBackCache(item);
        }

        public void LeaveItemListNoBackCache(ItemConfig item)
        {
            _listScrollView.gameObject.SetActive(false);
            _closeButton.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _detailsController.Init(item, this);
            _detailsController.gameObject.SetActive(true);
        }

        public void CacheBrowseBackAction(Action action) => _browseHistory.Push(action);

        internal void EnterItemListView()
        {
            _listScrollView.gameObject.SetActive(true);
            _closeButton.gameObject.SetActive(true);
            _backButton.gameObject.SetActive(true);
        }

        public void ProcessBackAction()
        {
            if (_browseHistory.TryPop(out var function))
            {
                function();
            }
        }
    }
}

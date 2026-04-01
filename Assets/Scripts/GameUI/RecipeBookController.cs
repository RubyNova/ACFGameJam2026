using UnityEngine;
using CraftingAPI;
using System;

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
        private ItemDetailsController _detailsController;

        private void Start()
        {
            ItemDatabase.Instance.ItemDiscovered += OnItemDiscovered;

            foreach (var item in ItemDatabase.Instance.DiscoveredItems)
            {
                OnItemDiscovered(item);
            }
        }

        private void OnItemDiscovered(ItemConfig item)
        {
            var newObject = Instantiate(_craftableItemDisplayPrefab, _listParent);
            newObject.GetComponent<CraftableItemDisplayController>().Init(item, this);
        }

        public void OpenCraftableItemInfo(ItemConfig item)
        {
            _listScrollView.gameObject.SetActive(false);
            _detailsController.Init(item);
            _detailsController.gameObject.SetActive(true);
        }
    }
}

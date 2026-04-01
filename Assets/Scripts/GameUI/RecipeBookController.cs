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
        private ItemDetailsController _detailsController;

        private void Start() => ItemDatabase.Instance.ItemDiscovered += OnItemDiscovered;

        private void OnItemDiscovered(ItemConfig item)
        {
            var newObject = Instantiate(_craftableItemDisplayPrefab, _listParent);
            newObject.GetComponent<CraftableItemDisplayController>().Init(item, this);
        }

        public void OpenCraftableItemInfo(ItemConfig item)
        {
            _detailsController.Init(item);
            _detailsController.gameObject.SetActive(true);
        }
    }
}

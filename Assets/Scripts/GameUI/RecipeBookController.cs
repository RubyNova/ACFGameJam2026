using UnityEngine;
using CraftingAPI;

namespace GameUI
{
    public class RecipeBookController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _craftableItemDisplayPrefab;

        [SerializeField]
        RectTransform _parent;

        private void Start()
        {
            ItemDatabase.Instance.ItemDiscovered += OnItemDiscovered;
        }

        private void OnItemDiscovered(ItemConfig item)
        {
            var newObject = Instantiate(_craftableItemDisplayPrefab, _parent);
            newObject.GetComponent<CraftableItemDisplayController>().Init(item);
        }
    }
}

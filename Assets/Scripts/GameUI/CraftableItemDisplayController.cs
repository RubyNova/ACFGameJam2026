using CraftingAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class CraftableItemDisplayController : MonoBehaviour
    {
        [SerializeField]
        private Image _iconRenderer;

        [SerializeField]
        private TextMeshProUGUI _titleRenderer;

        [SerializeField]
        private TextMeshProUGUI _shortDescriptionRenderer;

        private ItemConfig _item;

        private RecipeBookController _owningController;

        public void Init(ItemConfig item, RecipeBookController owningController)
        {
            _item = item;
            _owningController = owningController;
            _iconRenderer.sprite = item.ItemIcon;
            _titleRenderer.text = item.ItemName;
            _shortDescriptionRenderer.text = item.ShortDescription;
        }

        public void OpenCraftableItemInfo()
        {
            gameObject.SetActive(false);
            _owningController.OpenCraftableItemInfo(_item);
        }
    }
}

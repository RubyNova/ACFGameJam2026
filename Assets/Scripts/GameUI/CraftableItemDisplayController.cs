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

        public void Init(ItemConfig item)
        {
            _iconRenderer.sprite = item.ItemIcon;
            _titleRenderer.text = item.ItemName;
            _shortDescriptionRenderer.text = item.ShortDescription;
        }
    }
}

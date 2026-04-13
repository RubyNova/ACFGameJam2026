using CraftingAPI;
using UnityEngine;

namespace GameElements
{
    public class ItemInstance : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _itemRenderer;

        public ItemConfig BackingConfig { get; private set; }

        public void InitialiseWithItemConfig(ItemConfig config)
        {
            BackingConfig = config;
            _itemRenderer.sprite = BackingConfig.ItemIconBackground;
        }
    }
}
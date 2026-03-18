using System.Collections.Generic;
using UnityEngine;

namespace CraftingAPI
{
    [CreateAssetMenu(fileName = "NewItemConfig", menuName = "Crafting API/Create New ItemConfig", order = 1)]
    public class ItemConfig : ScriptableObject
    {
        [SerializeField]
        private string _itemName;

        [SerializeField]
        private string _shortDescription;

        [SerializeField]
        private Sprite _itemIcon;

        [SerializeField, TextArea]
        private string _recipeBookEntryDescription;

        [SerializeField]
        private ItemConfig[] _recipe;

        public string ItemName => _itemName;

        public string ShortDescription => _shortDescription;

        public Sprite ItemIcon => _itemIcon;

        public string RecipeBookEntryDescription => _recipeBookEntryDescription;

        public IReadOnlyList<ItemConfig> Recipe => _recipe;
    }
}
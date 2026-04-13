using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace CraftingAPI
{
    [CreateAssetMenu(fileName = "NewItemConfig", menuName = "Crafting API/Create New ItemConfig", order = 1)]
    public class ItemConfig : ScriptableObject, IComparable<ItemConfig>
    {
        public int TransientId { get; set; }

        [SerializeField]
        private string _itemName;

        [SerializeField]
        private string _shortDescription;

        [SerializeField, FormerlySerializedAs("_itemIcon")]
        private Sprite _itemIconBackground;
        
        [SerializeField]
        private Sprite _itemIconMiddle;
        
        [SerializeField]
        private Sprite _itemIconForeground;

        [SerializeField]
        private Color _backgroundColourTint;
        
        [SerializeField]
        private Color _middleColourTint;
        
        [SerializeField]
        private Color _foregroundColourTint;

        [SerializeField, TextArea]
        private string _recipeBookEntryDescription;

        [SerializeField]
        private ItemConfig[] _recipe;

        public string ItemName => _itemName;

        public string ShortDescription => _shortDescription;

        public Sprite ItemIconBackground => _itemIconBackground;
        
        public Sprite ItemIconMiddle => _itemIconMiddle;
        
        public Sprite ItemIconForeground => _itemIconForeground;

        public string RecipeBookEntryDescription => _recipeBookEntryDescription;

        public IReadOnlyList<ItemConfig> Recipe => _recipe;

        public Color BackgroundColourTint => _backgroundColourTint;
        
        public Color MiddleColourTInt => _middleColourTint;
        
        public Color ForegroundColourTint => _foregroundColourTint;

        public int CompareTo(ItemConfig other)
        {
            return other.TransientId.CompareTo(this.TransientId);
        }
    }
}
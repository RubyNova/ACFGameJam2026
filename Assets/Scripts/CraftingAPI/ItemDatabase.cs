using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

namespace CraftingAPI
{
    public class ItemDatabase : PocoSingleton<ItemDatabase>
    {
        private ItemConfig[] _itemData;
        private List<ItemConfig> _discoveredItems;

        public event Action<ItemConfig> ItemDiscovered;

        public IReadOnlyList<ItemConfig> DiscoveredItems => _discoveredItems;

        public ItemDatabase()
        {
            _discoveredItems = new();
            _itemData = Resources.LoadAll<ItemConfig>("ItemConfigData");
            for (int transientId = 0; transientId < _itemData.Length; transientId++)
            {
                _itemData[transientId].TransientId = transientId;
            }
            
        }

        public CraftingResult TryCraft(IReadOnlyList<ItemConfig> ingredients)
        {

            var data = _itemData.FirstOrDefault(x => x.Recipe.Count == ingredients.Count && x.Recipe.OrderBy(d => d).SequenceEqual(ingredients.OrderBy(d => d)));

            if (data != null && !_discoveredItems.Contains(data))
            {
                _discoveredItems.Add(data);
                ItemDiscovered?.Invoke(data);
            }

            return new(data != null, data);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
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
            _itemData = Resources.LoadAll<ItemConfig>("ItemConfigData");
            _discoveredItems = new();
        }

        public CraftingResult TryCraft(IReadOnlyList<ItemConfig> ingredients)
        {
            var data = _itemData.FirstOrDefault(x => x.Recipe.Count == ingredients.Count && x.Recipe.All(y => ingredients.Contains(y)));

            if (data != null && !_discoveredItems.Contains(data))
            {
                _discoveredItems.Add(data);
                ItemDiscovered?.Invoke(data);
            }

            return new(data != null, data);
        }
    }
}
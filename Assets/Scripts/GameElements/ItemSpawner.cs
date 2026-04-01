using CraftingAPI;
using UnityEngine;

namespace GameElements
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField]
        private ItemConfig _itemToSpawn;

        [SerializeField]
        private GameObject _itemInstancePrefab;

        public ItemInstance Spawn(Vector2 position)
        {
            var componentToReturn = Instantiate(_itemInstancePrefab, position, Quaternion.identity).GetComponent<ItemInstance>();
            componentToReturn.InitialiseWithItemConfig(_itemToSpawn);

            return componentToReturn;
        }
    }
}
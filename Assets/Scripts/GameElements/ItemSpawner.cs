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

        public void Spawn(Vector2 position)
        {
            Instantiate(_itemInstancePrefab, position, Quaternion.identity).GetComponent<ItemInstance>().InitialiseWithItemConfig(_itemToSpawn);
        }
    }
}
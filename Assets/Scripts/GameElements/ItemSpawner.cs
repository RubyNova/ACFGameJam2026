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

        [SerializeField]
        private float _zPosition;

        public ItemInstance Spawn(Vector2 position)
        {
            Vector3 properPosition = new(position.x, position.y, _zPosition);
            var componentToReturn = Instantiate(_itemInstancePrefab, properPosition, Quaternion.identity).GetComponent<ItemInstance>();
            componentToReturn.InitialiseWithItemConfig(_itemToSpawn);

            return componentToReturn;
        }
    }
}
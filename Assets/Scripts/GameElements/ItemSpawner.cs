using CraftingAPI;
using UnityEngine;

namespace GameElements
{
    public class ItemSpawner : MonoBehaviour
    {
        [Header("Required Configurations")]
        [SerializeField]
        private ItemConfig _itemToSpawn;

        [SerializeField]
        private GameObject _itemInstancePrefab;

        [SerializeField]
        private float _zPosition;

        [SerializeField]
        private SpriteRenderer _itemRenderer;

        [Header("Optional Configurations")]
        [SerializeField]
        private Material _material;

        private Texture2D _clearTexture;

        public ItemInstance Spawn(Vector2 position)
        {
            Vector3 properPosition = new(position.x, position.y, _zPosition);
            var componentToReturn = Instantiate(_itemInstancePrefab, properPosition, Quaternion.identity).GetComponent<ItemInstance>();
            componentToReturn.InitialiseWithItemConfig(_itemToSpawn);

            return componentToReturn;
        }

        public void Start()
        {
            if(_material != null)
            {
                _clearTexture = new Texture2D(1, 1);
                _clearTexture.SetPixel(0, 0, Color.clear);
                _clearTexture.Apply();

                _itemRenderer.sprite = _itemToSpawn.ItemIconBackground;

                if (_itemToSpawn.ItemIconMiddle != null)
                {
                    _material.SetTexture("_ItemIconMiddle", _itemToSpawn.ItemIconMiddle.texture);
                }
                else
                {
                    _material.SetTexture("_ItemIconMiddle", _clearTexture);
                }

                if (_itemToSpawn.ItemIconForeground != null)
                {
                    _material.SetTexture("_ItemIconForeground", _itemToSpawn.ItemIconForeground.texture);
                }
                else
                {
                    _material.SetTexture("_ItemIconForeground", _clearTexture);
                }

                _material.SetColor("_BaseMapColour", _itemToSpawn.BackgroundColourTint);
                _material.SetColor("_ItemIconMiddleColourTint", _itemToSpawn.MiddleColourTint);
                _material.SetColor("_ItemIconForegroundColourTint", _itemToSpawn.ForegroundColourTint);
            }
        }
    }
}
using CraftingAPI;
using UnityEngine;

namespace GameElements
{
    public class ItemInstance : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _itemRenderer;

        private Material _runtimeMaterial;

        private Texture2D _clearTexture;

        private bool _hasStarted = false;

        public ItemConfig BackingConfig { get; private set; }

        private void Start()
        {
            if (_hasStarted)
            {
                return;
            }

            _hasStarted = true;

            _runtimeMaterial = Instantiate(_itemRenderer.material);
            _itemRenderer.material = _runtimeMaterial;

            _clearTexture = new Texture2D(1, 1);
            _clearTexture.SetPixel(0, 0, Color.clear);
            _clearTexture.Apply();
        }

        private void OnDestroy()
        {
            Destroy(_runtimeMaterial);
        }

        public void InitialiseWithItemConfig(ItemConfig config)
        {
            Start();
            BackingConfig = config;
            _itemRenderer.sprite = BackingConfig.ItemIconBackground;

            if (BackingConfig.ItemIconMiddle != null)
            {
                _runtimeMaterial.SetTexture("_ItemIconMiddle", BackingConfig.ItemIconMiddle.texture);
            }
            else
            {
                _runtimeMaterial.SetTexture("_ItemIconMiddle", _clearTexture);
            }

            if (BackingConfig.ItemIconForeground != null)
            {
                _runtimeMaterial.SetTexture("_ItemIconForeground", BackingConfig.ItemIconForeground.texture);
            }
            else
            {
                _runtimeMaterial.SetTexture("_ItemIconForeground", _clearTexture);
            }

            _runtimeMaterial.SetColor("_BaseMapColour", BackingConfig.BackgroundColourTint);
            _runtimeMaterial.SetColor("_ItemIconMiddleColourTint", BackingConfig.MiddleColourTint);
            _runtimeMaterial.SetColor("_ItemIconForegroundColourTint", BackingConfig.ForegroundColourTint);
        }
    }
}
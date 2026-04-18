using CraftingAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class CraftableItemDisplayController : MonoBehaviour
    {
        [SerializeField]
        private Image _iconRenderer;

        [SerializeField]
        private TextMeshProUGUI _titleRenderer;

        [SerializeField]
        private TextMeshProUGUI _shortDescriptionRenderer;

        private ItemConfig _item;

        private RecipeBookController _owningController;

        private bool _hasFirstTimeInitialised = false;
        
        private Material _runtimeMaterial;

        private Texture2D _clearTexture;
        
        private void OnDestroy() => Destroy(_runtimeMaterial);

        public void Init(ItemConfig item, RecipeBookController owningController)
        {
            if (!_hasFirstTimeInitialised)
            {
                _hasFirstTimeInitialised = true;

                _runtimeMaterial = Instantiate(_iconRenderer.material);
                _iconRenderer.material = _runtimeMaterial;

                _clearTexture = new Texture2D(1, 1);
                _clearTexture.SetPixel(0, 0, Color.clear);
                _clearTexture.Apply();
            }

            _item = item;
            _owningController = owningController;
            _iconRenderer.sprite = item.ItemIconBackground;
            _titleRenderer.text = item.ItemName;
            _shortDescriptionRenderer.text = item.ShortDescription;
            
            if (_item.ItemIconMiddle != null)
            {
                _runtimeMaterial.SetTexture("_ItemIconMiddle", _item.ItemIconMiddle.texture);
            }
            else
            {
                _runtimeMaterial.SetTexture("_ItemIconMiddle", _clearTexture);
            }

            if (_item.ItemIconForeground != null)
            {
                _runtimeMaterial.SetTexture("_ItemIconForeground", _item.ItemIconForeground.texture);
            }
            else
            {
                _runtimeMaterial.SetTexture("_ItemIconForeground", _clearTexture);
            }

            _runtimeMaterial.SetColor("_BaseMapColour", _item.BackgroundColourTint);
            _runtimeMaterial.SetColor("_ItemIconMiddleColour", _item.MiddleColourTint);
            _runtimeMaterial.SetColor("_ItemIconForegroundColour", _item.ForegroundColourTint);
        }

        public void OpenCraftableItemInfo()
        {
            _owningController.OpenCraftableItemInfoAndLeaveItemList(_item);
        }
    }
}

using CraftingAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{


    public class ItemDetailsController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _ingredientButtonPrefab;

        [SerializeField]
        private RectTransform _recipeItemIconContainer;

        [SerializeField]
        private Image _largeIconRenderer;

        [SerializeField]
        private TextMeshProUGUI _titleTextRenderer;

        [SerializeField]
        private TextMeshProUGUI _detailsTextRenderer;

        [SerializeField]
        private Material _layeredSpriteMaterial;

        private RecipeBookController _recipeBookController;

        private ItemConfig _itemConfig;

        private bool _hasFirstTimeInitialised = false;

        private Material _runtimeBGMaterial;

        private Texture2D _clearTexture;

        private void OnDestroy() => DestroyManagedChildrenData();

        private void DestroyManagedChildrenData()
        {
            foreach (RectTransform child in _recipeItemIconContainer)
            {
                child.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(child.GetComponent<Image>().material);
                Destroy(child.gameObject);
            }
        }

        public void Init(ItemConfig item, RecipeBookController recipeBookController)
        {
            if (!_hasFirstTimeInitialised)
            {
                _hasFirstTimeInitialised = true;

                _clearTexture = new Texture2D(1, 1);
                _clearTexture.SetPixel(0, 0, Color.clear);
                _clearTexture.Apply();

                _runtimeBGMaterial = Instantiate(_largeIconRenderer.material);
                _largeIconRenderer.material = _runtimeBGMaterial;
            }

            _itemConfig = item;
            _recipeBookController = recipeBookController;
            _titleTextRenderer.text = item.ItemName;

            UpdateImageRenderer(item, _largeIconRenderer);

            _largeIconRenderer.sprite = item.ItemIconBackground;
            _detailsTextRenderer.text = item.RecipeBookEntryDescription;

            var ingredients = item.Recipe;

            DestroyManagedChildrenData();

            foreach (var ingredient in ingredients)
            {
                // I hate C# lambda capture rules. This is so arse.
                var itemLocal = item;
                var controllerLocal = recipeBookController;
                var newObject = Instantiate(_ingredientButtonPrefab, _recipeItemIconContainer);

                var imageInstance = newObject.GetComponent<Image>();
                imageInstance.material = Instantiate(_layeredSpriteMaterial);
                imageInstance.sprite = ingredient.ItemIconBackground;

                UpdateImageRenderer(ingredient, imageInstance);

                newObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _recipeBookController.CacheBrowseBackAction(() => Init(itemLocal, controllerLocal));
                    Init(ingredient, _recipeBookController);
                });
            }
        }

        private void UpdateImageRenderer(ItemConfig ingredient, Image imageInstance)
        {
            if (ingredient.ItemIconMiddle != null)
            {
                imageInstance.material.SetTexture("_ItemIconMiddle", ingredient.ItemIconMiddle.texture);
            }
            else
            {
                imageInstance.material.SetTexture("_ItemIconMiddle", _clearTexture);
            }

            if (ingredient.ItemIconForeground != null)
            {
                imageInstance.material.SetTexture("_ItemIconForeground", ingredient.ItemIconForeground.texture);
            }
            else
            {
                imageInstance.material.SetTexture("_ItemIconForeground", _clearTexture);
            }

            imageInstance.material.SetColor("_BaseMapColour", ingredient.BackgroundColourTint);
            imageInstance.material.SetColor("_ItemIconMiddleColour", ingredient.MiddleColourTint);
            imageInstance.material.SetColor("_ItemIconForegroundColour", ingredient.ForegroundColourTint);
        }

        public void ReturnToItemList()
        {
            // more lambda capture rule nonsense. Yippie!
            var itemLocal = _itemConfig;
            _recipeBookController.CacheBrowseBackAction(() => _recipeBookController.LeaveItemListNoBackCache(itemLocal));
            gameObject.SetActive(false);
            _recipeBookController.EnterItemListView();
        }
    }
}
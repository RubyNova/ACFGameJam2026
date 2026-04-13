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

        private RecipeBookController _recipeBookController;

        private ItemConfig _itemConfig;

        public void Init(ItemConfig item, RecipeBookController recipeBookController)
        {
            _itemConfig = item;
            _recipeBookController = recipeBookController;
            _titleTextRenderer.text = item.ItemName;
            _largeIconRenderer.sprite = item.ItemIconBackground;
            _detailsTextRenderer.text = item.RecipeBookEntryDescription;

            var ingredients = item.Recipe;

            foreach (RectTransform child in _recipeItemIconContainer)
            {
                child.GetComponent<Button>().onClick.RemoveAllListeners();
                Destroy(child.gameObject);
            }

            foreach (var ingredient in ingredients)
            {
                // I hate C# lambda capture rules. This is so arse.
                var itemLocal = item;
                var controllerLocal = recipeBookController;  
                var newObject = Instantiate(_ingredientButtonPrefab, _recipeItemIconContainer);

                newObject.GetComponent<Image>().sprite = ingredient.ItemIconBackground;
                newObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    _recipeBookController.CacheBrowseBackAction(() => Init(itemLocal, controllerLocal));
                    Init(ingredient, _recipeBookController);
                });
            }
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
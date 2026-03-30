using CraftingAPI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    public void Init(ItemConfig item)
    {
        _titleTextRenderer.text = item.ItemName;
        _largeIconRenderer.sprite = item.ItemIcon;
        _detailsTextRenderer.text = item.RecipeBookEntryDescription;

        var ingredients = item.Recipe;

        foreach (RectTransform child in _recipeItemIconContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var ingredient in ingredients)
        {
            var newObject = Instantiate(_ingredientButtonPrefab, _recipeItemIconContainer);
            newObject.GetComponent<Image>().sprite = ingredient.ItemIcon;
        }

    }
}

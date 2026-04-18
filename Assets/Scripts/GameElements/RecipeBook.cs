using CraftingAPI;
using TMPro;
using UnityEngine;

namespace GameElements
{
    public class RecipeBook : MonoBehaviour
    {
        [SerializeField]
        private Animator _discoveryAnimator;

        [SerializeField]
        private TMP_Text _discoveryTextBox;

        private void Start() => ItemDatabase.Instance.ItemDiscovered += DiscoveryEvent;

        private void OnDestroy() => ItemDatabase.Instance.ItemDiscovered -= DiscoveryEvent;

        private void DiscoveryEvent(ItemConfig data)
        {
            if(data != null)
            {
                Debug.Log("We found a new recipe: "+ data.ItemName);
                _discoveryTextBox.text = data.ItemName;
                _discoveryAnimator.SetTrigger("Discover");
            }
        }

    }
}

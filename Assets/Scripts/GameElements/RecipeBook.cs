using CraftingAPI;
using UnityEngine;

namespace GameElements
{
    public class RecipeBook : MonoBehaviour
    {


        private void Start()
        {
            ItemDatabase.Instance.ItemDiscovered += DiscoveryEvent;
        }

        private void DiscoveryEvent(ItemConfig data)
        {
            if(data != null)
                Debug.Log("We found a new recipe: "+ data.ItemName);
        }

    }
}

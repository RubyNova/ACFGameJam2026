using System.Collections.Generic;
using System.Collections;
using System.Linq;
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

        [SerializeField]
        private float _notificationAnimTime = 4f;

        private Queue<ItemConfig> _itemsToNotify = new();

        private Coroutine _notifyRoutine = null;

        private void Start() => ItemDatabase.Instance.ItemDiscovered += DiscoveryEvent;

        private void OnDestroy() => ItemDatabase.Instance.ItemDiscovered -= DiscoveryEvent;

        private void DiscoveryEvent(ItemConfig data)
        {
            if (data == null)
            {
                return;
            }

            Debug.Log("We found a new recipe: " + data.ItemName);
            _itemsToNotify.Enqueue(data);
        }

        private IEnumerator NotifyRoutine()
        {
            var item = _itemsToNotify.Dequeue();

            _discoveryTextBox.text = item.ItemName;
            _discoveryAnimator.SetTrigger("Discover");

            yield return new WaitForSeconds(_notificationAnimTime);

            _notifyRoutine = null;
        }

        private void Update()
        {
            if (_notifyRoutine != null || _itemsToNotify.Count == 0)
            {
                return;
            }

            _notifyRoutine = StartCoroutine(NotifyRoutine());
        }

    }
}

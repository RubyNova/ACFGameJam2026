using GameElements;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class WorldController : MonoBehaviour
    {
        private bool _shouldTryDrag;
        private Vector2 _mousePosition;
        private Camera _camera;

        private ItemInstance _lastHitObject;

        private void Awake()
        {
            _lastHitObject = null;
            _shouldTryDrag = false;
            _mousePosition = Vector2.zero;
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (!_shouldTryDrag)
            {
                return;
            }

            var mousePositionInWorld = _camera.ScreenToWorldPoint(_mousePosition);
            var result = Physics2D.Raycast(mousePositionInWorld, _camera.transform.forward);

            if (result.collider == null && _lastHitObject == null)
            {
                return;
            }

            if (_lastHitObject != null)
            {
                MoveObject(mousePositionInWorld, _lastHitObject);
                return;
            }

            if (result.transform.gameObject.TryGetComponent<ItemInstance>(out var item))
            {
                MoveObject(mousePositionInWorld, item);
                _lastHitObject = item;
            }
            else if (result.transform.gameObject.TryGetComponent<ItemSpawner>(out var spawner))
            {
                spawner.Spawn(mousePositionInWorld);
                _lastHitObject = null;
            }

        }

        private void MoveObject(Vector3 mousePositionInWorld, ItemInstance item)
        {
            var oldPosition = item.transform.position;
            oldPosition.x = mousePositionInWorld.x;
            oldPosition.y = mousePositionInWorld.y;
            item.transform.position = oldPosition;
        }

        public void OnMousePositionPoll(InputAction.CallbackContext context)
        {
            _mousePosition = context.ReadValue<Vector2>();
        }

        public void OnMouseLMBDown(InputAction.CallbackContext context)
        {
            _shouldTryDrag = context.ReadValueAsButton();
        }
    }
}
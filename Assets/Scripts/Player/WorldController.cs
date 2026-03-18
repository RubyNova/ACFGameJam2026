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

        private void Awake()
        {
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

            if (result.collider == null)
            {
                return;
            }

            if (result.transform.gameObject.GetComponent<ItemInstance>() != null)
            {
                var oldPosition = result.transform.position;
                oldPosition.x = mousePositionInWorld.x;
                oldPosition.y = mousePositionInWorld.y;
                result.transform.position = oldPosition;
            }
            else if (result.transform.gameObject.TryGetComponent<ItemSpawner>(out var spawner))
            {
                spawner.Spawn(mousePositionInWorld);
            }

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
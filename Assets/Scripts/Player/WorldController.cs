using GameElements;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace Player
{
    public class WorldController : MonoBehaviour
    {
        private bool _shouldTryDrag;
        private Vector2 _mousePosition;
        private Camera _camera;
        private bool _paused = false;
        private ItemInstance _lastHitObject;

        public bool IsPaused => _paused;

        private void Awake()
        {
            _lastHitObject = null;
            _shouldTryDrag = false;
            _mousePosition = Vector2.zero;

            EnhancedTouchSupport.Enable();
        }

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if(!_paused)
            {
                if(EnhancedTouchSupport.enabled)
                {
                    //Experimental touch support
                    var activeTouches = Touch.activeTouches;
                    if(activeTouches.Count > 0)
                    {
                        _mousePosition = activeTouches[0].finger.screenPosition;
                    }
                }

                if (!_shouldTryDrag)
                {
                    _lastHitObject = null;
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
                    _lastHitObject = spawner.Spawn(mousePositionInWorld);
                }
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

        protected void OnEnable()
        {
            if(!EnhancedTouchSupport.enabled)
            {
                EnhancedTouchSupport.Enable();
            }
        }

        protected void OnDisable()
        {
            EnhancedTouchSupport.Disable();
        }

        public void FlipPause() => _paused = !_paused;
    }
}
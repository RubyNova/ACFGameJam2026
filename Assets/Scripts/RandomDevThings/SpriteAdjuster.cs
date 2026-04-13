using UnityEngine;

namespace RandomDevThings
{
    public class SpriteAdjuster : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _renderer;

        private Material _runtimeMaterial;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            _runtimeMaterial = Instantiate(_renderer.material);
            _renderer.material = _runtimeMaterial;

            _runtimeMaterial.SetColor("_ItemIconMiddleColour", new Color(0, 0, 1, 1));
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
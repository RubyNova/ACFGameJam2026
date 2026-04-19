using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField]
    private float _scrollSpeed = 40f;

    private RectTransform _rectTransform;

    private int _yAxisStopPoint = 6284;
    private int _startingPoint = -1950;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_rectTransform.anchoredPosition.y >= _yAxisStopPoint)
        {
            LoadMainMenu();
            SendCreditsToNarnia();  // calls to load the main menu overlap, so i just reset the position so it only calls once
        }
        else
        {
            _rectTransform.anchoredPosition += new Vector2(0, _scrollSpeed * Time.deltaTime);
        }
    }

    private void LoadMainMenu()
    {
        LevelManager.Instance.LoadMainMenu();
    }

    private void SendCreditsToNarnia()
    {
        _rectTransform.anchoredPosition = new Vector2(1400, _startingPoint);
    }
}

using TMPro;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _textBox;
    
    [SerializeField]
    private GameClock _clock;

    public void TickEvent()
    {
        _textBox.text = $"Time Remaining: {(int)_clock.RemainingTimeInSeconds}s";
    }

}

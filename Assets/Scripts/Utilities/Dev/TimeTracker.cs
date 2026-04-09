using Player;
using TMPro;
using UnityEngine;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] 
    private TMP_Text _textBox;

    [SerializeField] 
    private TMP_Text _modeBox;
    
    [SerializeField]
    private GameClock _clock;

    [SerializeField]
    private ProgressController _controller;

    public void Update()
    {
        _textBox.text = $"Time Remaining: {(int)_clock.RemainingTimeInSeconds}s";
        _modeBox.text = $"Mode: {_controller.Mode}"; 
    }

    public void TickEvent()
    {
        
    }

}

using Player;
using System;
using UnityEngine;
using UnityEngine.Events;

public class GameClock : MonoBehaviour
{
    [Header("Required - Time Settings")]
    [SerializeField]
    private float _secondsBeforeLevelIsOver;

    [Header("Required - Clock Hand Settings")]
    [SerializeField]
    private Transform _minuteHandTransform;

    [SerializeField]
    private Transform _hourHandTransform;

    [Header("Optional - Skyroll Settings")]
    [SerializeField]
    private float _finalXPositionForNightSky;

    [SerializeField]
    private Transform _skyrollTransform;

    [Header("Reference Only - do not preset!")]
    [SerializeField]
    private bool _timerStarted;

    private float _secondsElapsed = 0f;
    
    private float _currentMinuteRotation = 0f;
    private float _currentHourRotation = 0f;
    private float _hourSpeed = 0f;
    private float _minuteSpeed = 0f;
    private const float _hourFinalDegreesOfRotation = -360f;
    private const float _minuteFinalDegreesOfRotation = -360f * 12f;  

    private float _skyrollSpeed = 0f;
    private bool _moveSkyroll = false;
    private bool _paused = false;
    
    

    public UnityEvent TickEvent = new();
    public UnityEvent TimerFinished = new();
    public float RemainingTimeInSeconds => (_secondsElapsed >= _secondsBeforeLevelIsOver) ? 0 : _secondsBeforeLevelIsOver - _secondsElapsed;
    public bool IsPaused => !_timerStarted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentMinuteRotation = _minuteHandTransform.rotation.z;
        _currentHourRotation = _hourHandTransform.rotation.z;

        _hourSpeed = _hourFinalDegreesOfRotation / _secondsBeforeLevelIsOver;
        _minuteSpeed = _minuteFinalDegreesOfRotation / _secondsBeforeLevelIsOver;

        if(_skyrollTransform != null)
        {
            _moveSkyroll = true;
            _skyrollSpeed = Math.Abs(_skyrollTransform.localPosition.x - _finalXPositionForNightSky) / _secondsBeforeLevelIsOver;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerStarted && !_paused)
        {
            _secondsElapsed += Time.deltaTime;

            var minuteRotation = _minuteSpeed * Time.deltaTime;
            _currentMinuteRotation += minuteRotation;
            _minuteHandTransform.localRotation = Quaternion.Euler(0, 0, _currentMinuteRotation);

            var hourRotation = _hourSpeed * Time.deltaTime;
            _currentHourRotation += hourRotation;            
            _hourHandTransform.localRotation = Quaternion.Euler(0, 0, _currentHourRotation);

            if(_moveSkyroll)
            {
                var skyrollPosition = -_skyrollSpeed * Time.deltaTime;
                _skyrollTransform.localPosition += new Vector3(skyrollPosition,0,0);
            }

            if(_secondsElapsed > _secondsBeforeLevelIsOver)
            {
                StopTimer(false, true);
            }
        }

        TickEvent.Invoke();
    }

    public void StopTimer(bool paused = false, bool completed = false)
    {
        _timerStarted = false;
        _paused = paused;

        if(completed)
        {
            TimerFinished.Invoke();
            //_secondsElapsed = 0f;
        }
    }

    public void StartTimer()
    {
        _timerStarted = true;
        _paused = false;
    }
}

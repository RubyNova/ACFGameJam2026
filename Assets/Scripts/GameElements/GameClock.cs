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

    public UnityEvent TimerFinished = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentMinuteRotation = _minuteHandTransform.rotation.z;
        _currentHourRotation = _hourHandTransform.rotation.z;

        _hourSpeed = _hourFinalDegreesOfRotation / _secondsBeforeLevelIsOver;
        _minuteSpeed = _minuteFinalDegreesOfRotation / _secondsBeforeLevelIsOver;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerStarted)
        {
            _secondsElapsed += Time.deltaTime;
            Debug.Log("Time: "+_secondsElapsed);

            var minuteRotation = _minuteSpeed * Time.deltaTime;
            _currentMinuteRotation += minuteRotation;
            _minuteHandTransform.localRotation = Quaternion.Euler(0, 0, _currentMinuteRotation);

            var hourRotation = _hourSpeed * Time.deltaTime;
            _currentHourRotation += hourRotation;            
            _hourHandTransform.localRotation = Quaternion.Euler(0, 0, _currentHourRotation);

            if(_secondsElapsed > _secondsBeforeLevelIsOver)
            {
                StopTimer(false, true);
            }
        }

    }

    public void StopTimer(bool paused = false, bool completed = false)
    {
        _timerStarted = false;

        if(completed)
        {
            TimerFinished.Invoke();
            _secondsElapsed = 0f;
        }
    }
}

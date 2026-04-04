using UnityEngine;
using UnityEngine.Events;

public class GameClock : MonoBehaviour
{
    [Header("Required - Time Settings")]
    [SerializeField]
    private float _secondsBeforeLevelIsOver;

    [SerializeField]
    private float _timescale;

    [Header("Required - Clock Hand Settings")]
    [SerializeField]
    private Transform _minuteHandTransform;

    [SerializeField]
    private Transform _hourHandTransform;

    [SerializeField]
    private float _hourHandDegreesPerInterval;

    [SerializeField]
    private float _minuteHandDegreesPerInterval;

    [Header("Reference Only - do not preset!")]
    [SerializeField]
    private bool _timerStarted;

    private float _secondsElapsed = 0f;
    
    private float _currentMinuteRotation = 0f;
    private float _currentHourRotation = 0f;

    public UnityEvent TimerFinished = new();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentMinuteRotation = _minuteHandTransform.rotation.z;
        _currentHourRotation = _hourHandTransform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        if(_timerStarted)
        {
            _secondsElapsed += Time.deltaTime;
            Debug.Log("Time: "+_secondsElapsed);

            var minuteRotation = _minuteHandDegreesPerInterval * _timescale * Time.deltaTime;
            _currentMinuteRotation -= minuteRotation;
            _minuteHandTransform.localRotation = Quaternion.Euler(0, 0, _currentMinuteRotation);

            var hourRotation = _hourHandDegreesPerInterval * _timescale * Time.deltaTime;
            _currentHourRotation -= hourRotation;            
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

using System.Collections;
using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private AutoNarrativeController _narrativeController;
    
    [SerializeField]
    private NarrativeSequence _arrivalNarrativeSequence;
    
    [SerializeField]
    private NarrativeSequence _departureNarrativeSequence;
    
    [SerializeField]
    private float _delayBeforeStartingDialogue = 0.0f;

    private bool _readyToTalk;
    private bool _readyToLeave;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(_readyToTalk)
        {
            _narrativeController?.ExecuteSequence(_arrivalNarrativeSequence);
            _readyToTalk = false;
        }

        if(_readyToLeave && _departureNarrativeSequence != null)
        {
            _narrativeController?.ExecuteSequence(_departureNarrativeSequence);
            _readyToLeave = false;
        }
        else if(_readyToLeave)
        {
            _readyToLeave = false;
        }
    }

    public void LaunchNPCArrivalDialogue()
    {
        StartCoroutine(LaunchDialogueOnDelay());
    }

    public void LaunchNPCDepartureDialogue()
    {
        _readyToLeave = true;
    }

    private IEnumerator LaunchDialogueOnDelay()
    {
        yield return new WaitForSeconds(_delayBeforeStartingDialogue);

        _readyToTalk = true;
    }
}

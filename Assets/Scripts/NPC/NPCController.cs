using System.Collections;
using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    private AutoNarrativeController _narrativeController;
    
    [SerializeField]
    private NarrativeSequence _narrativeSequence;
    
    [SerializeField]
    private bool _readyToTalk;
    
    [SerializeField]
    private float _delayBeforeStartingDialogue = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(LaunchDialogueOnDelay());
    }

    // Update is called once per frame
    void Update()
    {
        if(_readyToTalk)
        {
            _narrativeController?.ExecuteSequence(_narrativeSequence);
            _readyToTalk = false;
        }
    }

    public IEnumerator LaunchDialogueOnDelay()
    {
        yield return new WaitForSeconds(_delayBeforeStartingDialogue);

        _readyToTalk = true;
    }
}

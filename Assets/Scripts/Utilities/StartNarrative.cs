using ACHNarrativeDriver;
using ACHNarrativeDriver.ScriptableObjects;
using UnityEngine;

public class StartNarrative : MonoBehaviour
{
    [SerializeField]
    private NarrativeUIController _controller;

    [SerializeField]
    private NarrativeSequence _sequenceToStart;

    public void Start()
    {
        StartNarrativeSequence();
    }

    private void StartNarrativeSequence()
    {
        _controller.ExecuteSequence(_sequenceToStart);
    }
}

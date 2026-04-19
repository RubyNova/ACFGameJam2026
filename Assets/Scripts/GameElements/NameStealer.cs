using ACHNarrativeDriver.Api;
using UnityEngine;
using UnityEngine.UI;

public class NameStealer : MonoBehaviour
{
    [SerializeField]
    private string _inputText;

    [SerializeField]
    private RuntimeVariables _runtimeVariables;


    public void GetFromInputField(string input)
    {
        _inputText = input;
        _runtimeVariables.UpdateVariable("PlayerName", _inputText);
        LoadDay1();
    }

    private void LoadDay1()
    {
        LevelManager.Instance.LoadScene("Day1");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

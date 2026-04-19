using ACHNarrativeDriver.Api;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NameStealer : MonoBehaviour
{
    [SerializeField]
    private string _inputText;

    [SerializeField]
    private RuntimeVariables _runtimeVariables;

    [SerializeField]
    private RectTransform _paperSprite;

    private int _delay = 3;

    private int _shrinkSpeed;

    private float _scale = 1f;

    public void GetFromInputField(string input)
    {
        _inputText = input;
        _runtimeVariables.UpdateVariable("PlayerName", _inputText);
        StartCoroutine(LoadingProcess());
    }

    private void LoadDay1()
    {
        LevelManager.Instance.LoadScene("Day1");
    }

    private IEnumerator LoadingProcess()
    {
        yield return new WaitForSecondsRealtime(1);

        while(_scale > 0.01f)
        {
            _scale -= 0.03f;
            _paperSprite.localScale = new Vector3(_scale, _scale, 0);
            yield return new WaitForSecondsRealtime(0.04f);
        };

        LoadDay1();
        Object.Destroy(_paperSprite.gameObject);
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

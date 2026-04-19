using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class FadeManager : MonoBehaviour
{
    [SerializeField]
    private WaitForSeconds _waitForSeconds = new(5);

    [SerializeField]
    private Animator _fade;
    
    [SerializeField]
    private string _creditsSceneName;

    private IEnumerator StartFadeInternal()
    {
        if (_fade != null)
        {
            _fade.SetTrigger("Start");
            yield return _waitForSeconds;
            LevelManager.Instance.LoadScene(_creditsSceneName);
        }
    }
    
    public void FadeOut()
    {
        StartCoroutine(StartFadeInternal());
    }

}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class LevelManager : MonoSingleton<LevelManager>
{
    private static WaitForSeconds _waitForSeconds = new(1);

    [SerializeField]
    private Animator _fade;

    protected override void OnInit()
    {
        
    }

    private IEnumerator LoadSceneInternal(string sceneName)
    {
        _fade.SetTrigger("Start");
        yield return _waitForSeconds;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneInternal("LukeDevScene"));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneInternal(sceneName));
    }
}

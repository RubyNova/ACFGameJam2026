using GameAudio;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class LevelManager : MonoSingleton<LevelManager>
{
    private static WaitForSeconds _waitForSeconds = new(1);

    [SerializeField]
    private Animator _fade;

    private bool _changeBgm;

    protected override void OnInit()
    {

    }

    private IEnumerator LoadSceneInternal(string sceneName)
    {
        _fade.SetTrigger("Start");
        yield return _waitForSeconds;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if (SceneManager.GetActiveScene().name != "MainMenuDevScene")
        {
            _changeBgm = true;
        }
        else
        {
            _changeBgm = false;
        }

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        if (_changeBgm)
        {
            SoundManager.Instance.PlayBGM(false);
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneInternal("MainMenuDevScene"));
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneInternal(sceneName));
    }
}

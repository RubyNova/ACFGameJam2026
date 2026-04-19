using GameAudio;
using System;
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
        if (_fade != null)
        {
            _fade.SetTrigger("Start");
            yield return _waitForSeconds;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        if (asyncLoad is null)
        {
            throw new InvalidOperationException($"The scene name {sceneName} is not configured to be part of the game build. Please check the game's build settings, and try again.");
        }

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

        _fade.SetTrigger("End");
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadSceneInternal("MainMenuDevScene"));
    }

    public void LoadScene(string sceneName)
    {
        if(!sceneName.Contains("Between"))
        {
            if (_changeBgm)
            {
                SoundManager.Instance.PlayBGM(false);
            }
        }
        StartCoroutine(LoadSceneInternal(sceneName));
    }
}

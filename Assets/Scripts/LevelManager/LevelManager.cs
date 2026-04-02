using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class LevelManager : MonoSingleton<LevelManager>
{
    [SerializeField]
    private Animator _fade;

    protected override void OnInit()
    {
        
    }

    public IEnumerator LoadScene(string sceneName)
    {
        _fade.SetTrigger("Start");
        yield return new WaitForSeconds(1);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void LoadMainMenu()
    {
        StartCoroutine(LoadScene("LukeDevScene"));
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

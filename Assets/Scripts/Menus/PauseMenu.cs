using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool gamePaused = false;

    [SerializeField]
    private GameObject _pauseMenuUI;

    private float _timeScale = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.escapeKey.wasPressedThisFrame)
        {     
            // Reloading the main menu while its the current scene causes Mono singleton errors with the sound manager (creates multiple)
            //if (SceneManager.GetActiveScene().name != "LukeDevScene")             // also you cant usually pause a game on the main menu
            {
                TogglePause();
            }
        }
    }

    public void PauseGame()
    {
        if(_pauseMenuUI != null)
        {
            _pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gamePaused = true;
        }
    }

    public void ResumeGame()
    {
        if(_pauseMenuUI != null)
        {
            _pauseMenuUI.SetActive(false);
            Time.timeScale = _timeScale;
            gamePaused = false;
        }  
    }

    public void TogglePause()
    {
        if(gamePaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

}

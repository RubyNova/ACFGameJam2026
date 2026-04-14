using GameAudio;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenuUI;
    [SerializeField]
    private GameObject _optionsMenuUI;

    [SerializeField]
    private GameObject _recipeBookUI;

    private WorldController _worldController;

    private float _timeScale = 1f;

    private bool gamePaused = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetWorldController();
    }

    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.pKey.wasPressedThisFrame)
        {     
            // Reloading the main menu while its the current scene causes Mono singleton errors with the sound manager (creates multiple)
            if (SceneManager.GetActiveScene().name != "MainMenuDevScene")             // also you cant usually pause a game on the main menu
            {
                TogglePause();
            }
        }
    }

    public void PauseGame()
    {
        if(_pauseMenuUI != null && !_optionsMenuUI.activeInHierarchy)
        {
            _pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            gamePaused = true;
            if (!_worldController.IsPaused && !_recipeBookUI.activeInHierarchy)
            {
                _worldController.FlipPause();
            }
        }
    }

    public void ResumeGame()
    {
        if(_pauseMenuUI != null && !_optionsMenuUI.activeInHierarchy)
        {
            _pauseMenuUI.SetActive(false);
            Time.timeScale = _timeScale;
            gamePaused = false;
            if(_worldController.IsPaused && !_recipeBookUI.activeInHierarchy)
            {
                _worldController.FlipPause();
            }
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

    public void LeaveOptionsMenu()
    {
        if(SceneManager.GetActiveScene().name == "MainMenuDevScene")
        {
            _optionsMenuUI.SetActive(false);
            _pauseMenuUI.SetActive(false);
        }
        else
        {
            _optionsMenuUI.SetActive(false);
            _pauseMenuUI.SetActive(true);
        }
    }

    public void ReturnToMainMenu()
    {
        var lvlManagerObject = Object.FindFirstObjectByType<LevelManager>();
        if(lvlManagerObject != null)
        {
            var lvlManager = lvlManagerObject.GetComponent<LevelManager>();
            lvlManager.LoadMainMenu();
        }
    }

    private void GetWorldController()
    {
        _worldController = Object.FindFirstObjectByType<WorldController>();
    }

}

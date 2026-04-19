using GameAudio;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        LevelManager.Instance.LoadScene("NameStealingScene");
        SoundManager.Instance.PlayStartAudio();
    }

    public void StartEndlessGame()
    {
        LevelManager.Instance.LoadScene("Endless");
        SoundManager.Instance.PlayStartAudio();
    }

    public void LoadCreditsScene()
    {
        LevelManager.Instance.LoadScene("CreditsScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


}

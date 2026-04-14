using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        LevelManager.Instance.LoadScene("KennyDevScene");
    }

    public void StartEndlessGame()
    {
        LevelManager.Instance.LoadScene("KennyPerfScene");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

}

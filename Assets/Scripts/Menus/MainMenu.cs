using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public void StartGame()
    {
        StartCoroutine(LevelManager.Instance.LoadScene("KennyDevScene"));
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
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

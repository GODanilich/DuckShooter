using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private DifficultySettings difficultySettings;
    [SerializeField] private Canvas settings;
    [SerializeField] private Canvas mainMenu;

    public void Start()
    {
        difficultySettings = FindObjectOfType<DifficultySettings>();
    }
    public void StartGame()
    {   
        SceneManager.LoadScene("GameScene"); // Замените на имя вашей игровой сцены
    }

    public void ShowSettings()
    {
        mainMenu.gameObject.SetActive(false);
        settings.gameObject.SetActive(true);
    }

    public void HideSettings()
    {
        settings.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(true);
    }

    public void SetEasyDifficulty(int difficulty)
    {
        difficultySettings.SetEasyDifficulty();
    }

    public void SetMediumDifficulty(int difficulty)
    {
        difficultySettings.SetMediumDifficulty();
    }

    public void SetHardDifficulty(int difficulty)
    {
        difficultySettings.SetHardDifficulty();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
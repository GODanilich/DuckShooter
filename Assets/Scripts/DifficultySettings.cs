using UnityEngine;

public class DifficultySettings : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip easyMusic;
    [SerializeField] private AudioClip normalMusic;
    [SerializeField] private AudioClip doomMusic;

    public DifficultyLevel currentDifficulty = DifficultyLevel.Easy;

    private static DifficultySettings instance;

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    void Awake()
    {
        audioSource.clip = easyMusic;
        audioSource.loop = true;
        audioSource.Play();

        // Проверяем, существует ли уже экземпляр
        if (instance == null)
        {
            // Если нет, делаем этот объект единственным экземпляром
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Если экземпляр уже существует, уничтожаем новый дубликат
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject); // Объект сохраняется при смене сцены
    }

    public void SetEasyDifficulty()
    {
        audioSource.clip = easyMusic;
        audioSource.loop = true;
        audioSource.Play();
        currentDifficulty = DifficultyLevel.Easy;
    }
    
    public void SetMediumDifficulty()
    {
        audioSource.clip = normalMusic;
        audioSource.loop = true;
        audioSource.Play();
        currentDifficulty = DifficultyLevel.Medium;
    }
 
    public void SetHardDifficulty()
    {
        audioSource.clip = doomMusic;
        audioSource.loop = true;
        audioSource.Play();
        currentDifficulty = DifficultyLevel.Hard;
    }
}
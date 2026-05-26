using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Level Settings")]
    public LevelData levelData;

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI failText;
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    [HideInInspector] public int totalOrders;
    [HideInInspector] public int maxFails;

    private int score = 0;
    private int fails = 0;
    private int processed = 0;
    private float timeLeft;
    private bool isGameOver = false;

    void Awake() => Instance = this;

    void Start()
    {
        totalOrders = levelData.totalOrders;
        maxFails = levelData.maxFails;
        timeLeft = levelData.levelTime;
        gameOverPanel.SetActive(false);
        victoryPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (isGameOver) return;
        timeLeft -= Time.deltaTime;
        UpdateTimerUI();
        if (timeLeft <= 0) { timeLeft = 0; TriggerGameOver(); }
    }

    public void RegisterSuccess()
    {
        score += 10;
        processed++;
        UpdateUI();
        CheckEnd();
    }

    public void RegisterFail()
    {
        fails++;
        processed++;
        UpdateUI();
        if (fails >= maxFails) TriggerGameOver();
        else CheckEnd();
    }

    void CheckEnd()
    {
        if (processed >= totalOrders) TriggerVictory();
    }

    void TriggerGameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    void TriggerVictory()
    {
        string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (sceneName == "Level1") PlayerPrefs.SetInt("Level1Complete", 1);
        else if (sceneName == "Level2") PlayerPrefs.SetInt("Level2Complete", 1);
        PlayerPrefs.Save();
        isGameOver = true;
        victoryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1;
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(next);
        else
            SceneManager.LoadScene(0);
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);
        timerText.text = $"Time: {minutes:00}:{seconds:00}";
    }

    void UpdateUI()
        {
            scoreText.text = $"Score: {score}";
            failText.text = $"Fails: {fails}/{maxFails}";
        }

    public void QuitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
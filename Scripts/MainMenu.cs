using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;
    public TextMeshProUGUI level2Text;
    public TextMeshProUGUI level3Text;

    void Start()
    {
        bool level2Unlocked = PlayerPrefs.GetInt("Level1Complete", 0) == 1;
        bool level3Unlocked = PlayerPrefs.GetInt("Level2Complete", 0) == 1;

        level2Button.interactable = level2Unlocked;
        level3Button.interactable = level3Unlocked;

        level2Text.text = level2Unlocked ? "Level 2 ✓" : "Level 2 🔒";
        level3Text.text = level3Unlocked ? "Level 3 ✓" : "Level 3 🔒";
    }

    public void LoadLevel1() => SceneManager.LoadScene("Level1");
    public void LoadLevel2() => SceneManager.LoadScene("Level2");
    public void LoadLevel3() => SceneManager.LoadScene("Level3");
}
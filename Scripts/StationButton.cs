using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum StationType { Instant, Timed }

public class StationButton : MonoBehaviour
{
    public StationType stationType;
    public Ingredient ingredient;
    public float processingTime = 3f;

    public Button button;
    public TextMeshProUGUI label;
    public Slider progressSlider;
    public Image buttonImage;
    public Sprite defaultSprite;
    public Sprite readySprite;

    private bool isReady = false;

    void Start()
    {
        button.onClick.AddListener(OnClick);
        if (progressSlider != null)
            progressSlider.gameObject.SetActive(false);
        if (buttonImage != null && defaultSprite != null)
            buttonImage.sprite = defaultSprite;
    }

    void OnClick()
    {
        if (stationType == StationType.Instant)
        {
            TrayManager.Instance.AddIngredient(ingredient);
        }
        else
        {
            if (!isReady)
                StartCoroutine(Process());
            else
                CollectIngredient();
        }
    }

    IEnumerator Process()
    {
        button.interactable = false;
        if (progressSlider != null)
        {
            progressSlider.gameObject.SetActive(true);
            progressSlider.value = 0;
        }

        float elapsed = 0;
        while (elapsed < processingTime)
        {
            elapsed += Time.deltaTime;
            if (progressSlider != null)
                progressSlider.value = elapsed / processingTime;
            yield return null;
        }

        isReady = true;
        button.interactable = true;
        if (buttonImage != null && readySprite != null)
            buttonImage.sprite = readySprite;
        if (progressSlider != null)
            progressSlider.gameObject.SetActive(false);
    }

    void CollectIngredient()
    {
        TrayManager.Instance.AddIngredient(ingredient);
        isReady = false;
        if (buttonImage != null && defaultSprite != null)
            buttonImage.sprite = defaultSprite;
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TrayManager : MonoBehaviour
{
    public static TrayManager Instance;

    public Transform trayIconContainer;
    public GameObject iconPrefab;

    public Sprite coffeeSprite;
    public Sprite sandwichSprite;
    public Sprite croissantSprite;

    private List<Ingredient> currentTray = new List<Ingredient>();

    void Awake() => Instance = this;

    public void AddIngredient(Ingredient ingredient)
{
    if (currentTray.Count >= 3) return;
    currentTray.Add(ingredient);
    AddIcon(ingredient);
}

void AddIcon(Ingredient ingredient)
{
    var go = Instantiate(iconPrefab, trayIconContainer);
    var img = go.GetComponent<Image>();
    if (ingredient == Ingredient.Coffee) img.sprite = coffeeSprite;
    else if (ingredient == Ingredient.Sandwich) img.sprite = sandwichSprite;
    else if (ingredient == Ingredient.Croissant) img.sprite = croissantSprite;
}

    public List<Ingredient> GetTrayContents() => new List<Ingredient>(currentTray);

    public void ClearTray()
    {
        currentTray.Clear();
        foreach (Transform child in trayIconContainer)
            Destroy(child.gameObject);
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OrderCard : MonoBehaviour
{
    [HideInInspector] public List<Ingredient> requiredIngredients;

    public TextMeshProUGUI ingredientListText;
    public Slider patienceSlider;

    private float patienceTime = 20f;
    private float remaining;
    private bool isActive = true;

    public void Initialize(List<Ingredient> ingredients, float patience)
    {
        requiredIngredients = ingredients;
        patienceTime = patience;
        remaining = patience;
        patienceSlider.maxValue = patience;
        patienceSlider.value = patience;
        ingredientListText.text = string.Join("\n", ingredients);
    }

    void Update()
    {
        if (!isActive) return;
        remaining -= Time.deltaTime;
        patienceSlider.value = remaining;
        if (remaining <= 0) Expire();
    }

    void Expire()
    {
        isActive = false;
        OrderManager.Instance.RemoveOrder(this);
        GameManager.Instance.RegisterFail();
        Destroy(gameObject);
    }

    public void Deactivate() => isActive = false;
}
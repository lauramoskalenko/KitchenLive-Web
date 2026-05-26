using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;

    public GameObject orderCardPrefab;
    public Transform orderQueue;

    private float spawnInterval;
    private float patienceTime;
    private int maxSimultaneous;

    private List<OrderCard> activeOrders = new List<OrderCard>();
    private int ordersSpawned = 0;

    void Awake() => Instance = this;

    void Start()
    {
        var data = GameManager.Instance.levelData;
        spawnInterval = data.spawnInterval;
        patienceTime = data.patienceTime;
        maxSimultaneous = data.maxSimultaneous;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (ordersSpawned < GameManager.Instance.totalOrders)
        {
            if (activeOrders.Count < maxSimultaneous)
                SpawnOrder();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOrder()
    {
        var recipes = GameManager.Instance.levelData.recipes;
        var recipe = recipes[Random.Range(0, recipes.Count)];
        var go = Instantiate(orderCardPrefab, orderQueue);
        var card = go.GetComponent<OrderCard>();
        card.Initialize(new List<Ingredient>(recipe.ingredients), patienceTime);
        activeOrders.Add(card);
        ordersSpawned++;
    }

    public void RemoveOrder(OrderCard card) => activeOrders.Remove(card);

    public void TryServe()
    {
        if (activeOrders.Count == 0) return;

        var tray = TrayManager.Instance.GetTrayContents();
        var required = new List<Ingredient>(activeOrders[0].requiredIngredients);

        tray.Sort();
        required.Sort();

        bool match = tray.Count == required.Count;
        if (match)
            for (int i = 0; i < tray.Count; i++)
                if (tray[i] != required[i]) { match = false; break; }

        if (match)
        {
            activeOrders[0].Deactivate();
            Destroy(activeOrders[0].gameObject);
            activeOrders.RemoveAt(0);
            GameManager.Instance.RegisterSuccess();
        }
        else
        {
            GameManager.Instance.RegisterFail();
        }

        TrayManager.Instance.ClearTray();
    }
}
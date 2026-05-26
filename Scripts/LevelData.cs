using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "KitchenLive/LevelData")]
public class LevelData : ScriptableObject
{
    public int totalOrders = 8;
    public int maxFails = 3;
    public float levelTime = 120f;
    public float spawnInterval = 6f;
    public float patienceTime = 20f;
    public int maxSimultaneous = 2;

    [System.Serializable]
    public class Recipe
    {
        public List<Ingredient> ingredients;
    }

    public List<Recipe> recipes;
}
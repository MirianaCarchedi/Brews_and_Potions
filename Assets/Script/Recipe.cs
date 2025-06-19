using UnityEngine;

[System.Serializable]
public class Recipe
{
    public string name;
    public Sprite icon;
    public string description;
    public string ingredient1;
    public string ingredient2;
    public bool isDiscovered = false; // <-- sbloccata o no
}


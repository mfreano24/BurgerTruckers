using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class Cooking
{
    public List<Recipe> gameRecipes;

    public Cooking()
    {
        gameRecipes = ParseRecipesForGame();
    }
    public List<Recipe> ParseRecipesForGame()
    {
        List<Recipe> t = new List<Recipe>();
        using (var reader = new StreamReader(Path.Combine(Application.streamingAssetsPath, "Recipes.csv")))
        {
            string ln = reader.ReadLine();
            do
            {

                string[] breakdown = ln.Split(',');
                List<string> temp_ingr = new List<string>();
                temp_ingr = breakdown.ToList<string>();
                temp_ingr.RemoveAt(0);
                Recipe curr = new Recipe(breakdown[0], temp_ingr);
                t.Add(curr);
                ln = reader.ReadLine();

            } while (ln != null);
            
        }
        
        foreach(Recipe r in t)
        {
            Debug.Log(r.ToString());
        }
        return t;
    }
}

public class Recipe
{
    //out of control ingredients?
    public enum Ingredient
    {
        Bottom_Bun, Burger, Cheese, Onion, Lettuce, Mushroom, Tomato, Chopped_Onion, Chopped_Mushroom, Chopped_Tomato, Top_Bun, NONE
    }
    public string name;
    public List<Ingredient> ingr;
    public int score = 0;
    public int baseScore = 500;
    public Recipe(string n, List<string> ing)
    {
        name = n;
        
        List<Ingredient> i = new List<Ingredient>();
        foreach(string s in ing)
        {
            Ingredient curr = Ingredient.Bottom_Bun;
            switch (s)
            {
                case "Bottom Bun": curr = Ingredient.Bottom_Bun; break;
                case "Top Bun": curr = Ingredient.Top_Bun; break;
                case "Burger": curr = Ingredient.Burger; break;
                case "Cheese": curr = Ingredient.Cheese; break;
                case "Onion": curr = Ingredient.Chopped_Onion; break;
                case "Lettuce": curr = Ingredient.Lettuce; break;
                case "Mushroom": curr = Ingredient.Chopped_Mushroom; break;
                case "Tomato": curr = Ingredient.Chopped_Tomato; break;
                default: curr = Ingredient.NONE; break;
            }
            if(curr != Ingredient.NONE)
            {
                i.Add(curr);
            }
        }
        ingr = new List<Ingredient>();
        ingr = i;
    }

    public override string ToString()
    {
        string ret = name + ": ";
        foreach(Ingredient i in ingr)
        {
            ret += i + " ";
        }
        return ret;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCard : MonoBehaviour
{
    public Text recipeText;
    public Text timer;
    RectTransform rt;
    void Start()
    {
        rt = GetComponent<RectTransform>();
        timer.text = "";
    }

    public void DrawRecipe(string recipeName, List<Recipe.Ingredient> ing)
    {
        recipeText.text = recipeName + "\n--------------------------\n";
        foreach(Recipe.Ingredient i in ing)
        {
            recipeText.text += i + "\n";
        }
    }

    public IEnumerator SlideIntoPosition(Vector2 pos)
    {
        //this shouldnt be an IEnum anymore???
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        yield return new WaitForSeconds(0.1f);
    }

    public void TimerDrawText(int t)
    {
        timer.text = t.ToString();
    }
}

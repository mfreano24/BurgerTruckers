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

    // Update is called once per frame
    void Update()
    {
        
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
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = pos;
        /*
        rt = GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(1116, 271);
        for(int i = 0; i < 100; i++)
        {
            rt.anchoredPosition = new Vector2((rt.anchoredPosition.x - pos.x) * 0.01f, 271);
            yield return new WaitForSeconds(0.01f);
        }
        */
        yield return new WaitForSeconds(0.1f);
    }

    public void TimerDrawText(int t)
    {
        timer.text = t.ToString();
    }
}

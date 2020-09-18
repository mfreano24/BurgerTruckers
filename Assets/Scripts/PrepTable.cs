using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class PrepTable : MonoBehaviour
{
    List<Ingredient_Full> storage = new List<Ingredient_Full>();
    public GameObject canvas;
    public Text prepList;
    void Start()
    {
        canvas.SetActive(false);
        prepList.text = "";
    }


    void Update()
    {

    }


    public Ingredient_Full Interact_Take()
    {
        EmptyTable();
        return null;

    }

    public bool Interact_Place(Ingredient_Full i)
    {
        Debug.Log("PLACED " + i.ingr);
        storage.Add(i);
        drawPrepList();
        return true;
    }

    public void PullUpList()
    {
        canvas.SetActive(true);
    }
    public void PutAwayList()
    {
        canvas.SetActive(false);
    }

    void drawPrepList()
    {
        prepList.text = "";
        foreach(Ingredient_Full i in storage)
        {
            prepList.text += i.ingr.ToString();
            if (i.cookValue > -1)
            {
                if (i.cookValue < 1000)
                {
                    //raw
                    prepList.text += " - Raw\n";
                }
                else if (i.cookValue > 2000)
                {
                    prepList.text += " - Burnt\n";
                }
                else
                {
                    prepList.text += " - Cooked\n";
                }
            }
            else
            {
                prepList.text += "\n";
            }
        }
    }

    public Recipe ValidRecipeCheck(Cooking c)
    {
        foreach (Recipe r in c.gameRecipes.ToList())
        {
            r.score = 0;
            List<Recipe.Ingredient> copy = r.ingr.ToList();
            foreach (Recipe.Ingredient ri in r.ingr.ToList())
            {
                foreach (Ingredient_Full i in storage.ToList())
                {
                    if (i.ingr == ri)
                    {
                        if(i.cookValue >= 0) //if its cookable
                        {
                            if(i.cookValue < 1000)
                            {
                                //raw
                                r.score += 500;
                                Debug.Log("Too Raw!");
                            }
                            else if (i.cookValue > 2000)
                            {
                                //burnt 
                                r.score += 400;
                                Debug.Log("Burnt!");
                            }
                            else if(i.cookValue > 1350 && i.cookValue < 1500)
                            {
                                r.score += 2000; //perfect!
                                Debug.Log("PERFECT!");
                            }
                            else
                            {
                                r.score += 1000; //almost!
                                Debug.Log("Almost!");
                            }
                        }
                        
                        copy.Remove(ri);
                        break;
                    }
                }
            }
            if (copy.Count == 0 && storage.Count == r.ingr.Count)
            {
                Debug.Log("Recipe Found: " + r.name);
                r.score += r.baseScore;
                return r;
            }
        }
        Debug.Log("No Recipes Found...");
        return null;
    }

    public void EmptyTable()
    {
        storage.Clear();
        drawPrepList();
    }
}

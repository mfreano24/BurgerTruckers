using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChopTable : MonoBehaviour
{
    Ingredient_Full storage = null;
    public Text OnTable;
    public GameObject InfoCanvas;

    void Update()
    {
        if(storage == null && InfoCanvas.activeInHierarchy)
        {
            //if the information canvas happens to be on, turn it off.
            InfoCanvas.SetActive(false);
        }
    }


    public Ingredient_Full Interact_Take()
    {
        //take something, return the ingredient taken so that the player can hold it in their script.
        if(storage == null)
        {
            Debug.Log("There's nothing on the table.");
            return null;
        }
        else
        {
            Ingredient_Full ret = storage;
            storage = null;
            return ret;
        }
    }

    public bool Interact_Place(Ingredient_Full i)
    {
        //pass in an ingredient (usually the held ingredient from the player)
        //return a bool whether or not it was successful.
        if(storage == null)
        {
            Debug.Log("Chop chop!");
            switch (i.ingr)
            {
                case Recipe.Ingredient.Mushroom: storage = new Ingredient_Full(Recipe.Ingredient.Chopped_Mushroom); break;
                case Recipe.Ingredient.Onion: storage = new Ingredient_Full(Recipe.Ingredient.Chopped_Onion); break;
                case Recipe.Ingredient.Tomato: storage = new Ingredient_Full(Recipe.Ingredient.Chopped_Tomato); break;
                default: storage = i; break;
            }
            InfoCanvas.SetActive(true);
            DrawUI();
            return true;
        }
        else
        {
            Debug.Log("There's something there already.");
            return false;
        }
    }

    void DrawUI()
    {
        OnTable.text = "ON TABLE:\n" + storage.ingr.ToString();
    }


}

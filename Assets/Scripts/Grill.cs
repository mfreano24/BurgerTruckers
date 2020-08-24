using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Grill : MonoBehaviour
{
    int cooked_level_L = 0, cooked_level_R = 0;
    Ingredient_Full L = null, R = null;
    public Text Left;
    public Text Right;
    public GameObject LPanel;
    public GameObject RPanel;
    public GameManager gm;
    AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        aud.volume = 0;
        LPanel.SetActive(false);
        RPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!gm.paused)
        {
            if (L != null || R != null)
            {
                aud.volume = 1;
            }
            else
            {
                aud.volume = 0;
            }
            if (L != null)
            {
                L.cookValue++;
                Left.color = textColorFunction(L.cookValue);
            }
            if (R != null)
            {
                R.cookValue++;
                Right.color = textColorFunction(R.cookValue);
            }
            DrawUI(1); DrawUI(2);

        }
    }

    public Ingredient_Full Interact_Take(int sideOfGrill)
    {

        if(sideOfGrill == 1)
        {
            if(L != null)
            {
                Ingredient_Full ret = L;
                L = null;
                LPanel.SetActive(false);
                return ret;
            }
            else
            {
                return null;
            }
        }
        else if(sideOfGrill == 2)
        {
            if (R != null)
            {
                Ingredient_Full ret = R;
                R = null;
                RPanel.SetActive(false);
                return ret;
            }
            else
            {
                return null;
            }

        }
        else
        {
            return null; //what
        }
        

    }

    public Color textColorFunction(float val)
    {
        if(val < 1000)
        {
            return new Color(1 - (val / 5000), 1, 1 - (val / 5000));
        }
        else if(val < 2000)
        {
            return Color.green;
        }
        else {
            return new Color(val/4000, val/4000, val/4000);
        }
    }

    public bool Interact_Place(Ingredient_Full ri, int sideOfGrill)
    {
        //TODO: How to differentiate which half of the grill we're standing on
        if (ri.ingr != Recipe.Ingredient.Burger && ri.ingr != Recipe.Ingredient.Chopped_Onion && ri.ingr != Recipe.Ingredient.Chopped_Mushroom)
        {
            Debug.Log("YOU CANT COOK THAT");
            return false;
        }
        else
        {
            if(sideOfGrill == 1)
            {
                if(L == null)
                {
                    L = ri;
                    LPanel.SetActive(true);
                    DrawUI(1);
                    
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if(R == null)
                {
                    R = ri;
                    RPanel.SetActive(true);
                    DrawUI(2);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    void DrawUI(int sideOfGrill)
    {
        if(sideOfGrill == 1)
        {
            if(L != null)
            {
                string s = "";
                if(L.ingr == Recipe.Ingredient.Chopped_Mushroom)
                {
                    s = "Mushroom";
                }
                else if (L.ingr == Recipe.Ingredient.Chopped_Onion)
                {
                    s = "Onion";
                }
                else
                {
                    s = "Burger";
                }
                Left.text = s + "\n" + L.cookValue;
            }
            
        }
        else
        {
            if(R != null)
            {
                string s = "";
                if (R.ingr == Recipe.Ingredient.Chopped_Mushroom)
                {
                    s = "Mushroom";
                }
                else if (R.ingr == Recipe.Ingredient.Chopped_Onion)
                {
                    s = "Onion";
                }
                else
                {
                    s = "Burger";
                }
                Right.text = s + "\n" + R.cookValue;
            }
            
        }
    }


}



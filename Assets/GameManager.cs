using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    Cooking ck;
    public List<Request> currentRequests;
    const int REQUEST_MAX = 2;
    public PrepTable table1;
    public PrepTable table2;

    public int totalScore;
    public int misses;

    bool onRegularTime = false;
    public int requestsDone = 0;
    bool newRequestAvailable = true;
    public GameObject recipeCardPrefab;
    RecipeCard inst1, inst2;
    public GameObject MainCanvas;
    public GameObject[] missIcons;
    public DebugInfoText info;

    public GameObject Lose;
    public GameObject Pause;
    public bool paused;

    public GameObject seatLight;

    public GameObject tutorialLight;
    AudioSource[] aud;
    void Start()
    {
        aud = GetComponents<AudioSource>();
        aud[4].volume = 0;
        aud[3].volume = 0.85f;
        //0 = Miss, 1 = New Order, 2 = Success
        Debug.Log("RQUESTS DONE = " + requestsDone);
        
        ck = new Cooking();
        currentRequests = new List<Request>();


        
        info.drawText();
        
        
        Request tutorialBurger = new Request(ck.gameRecipes[0], 999);
        AddRequestToList(tutorialBurger);


        StartCoroutine(Tutorial());
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            if (onRegularTime)
            {
                if (newRequestAvailable && currentRequests.Count < REQUEST_MAX)
                {
                    int randomRecipe = GetRandomRecipeIndex();
                    
                    Request newReq = new Request(ck.gameRecipes[randomRecipe], 120);
                    AddRequestToList(newReq);
                }
            }

            /*if (Input.GetKeyDown(KeyCode.K)) //debug :)
            {
                requestsDone++;
            }*/

            if (requestsDone > 1 && !onRegularTime)
            {
                Debug.Log("Starting on regular time!");
                string[] free = { "I think you can take it from here. Start making those orders, and we'll fly 'em out! Good Luck!", "" };
                
                onRegularTime = true;
                newRequestAvailable = true;
            }
            if (Input.GetButtonDown("Pause"))
            {
                PauseGame();
            }

        }
        

        


        
    }

    int GetRandomRecipeIndex()
    {
        int ret = Random.Range(2, ck.gameRecipes.Count - 1);
        if(currentRequests.Count == 1)
        {
            //ensure that no two requests will be the same

            if(currentRequests[0].rec == ck.gameRecipes[ret])
            {
                ret++;
            }
        }
        return ret;
    }

    float timer(float x)
    {
        if(x != 0)
        {
            return 1.0f / x;
        }
        else
        {
            return 1;
        }
        
    }

    void AddRequestToList(Request r)
    {
        if(currentRequests.Count < REQUEST_MAX)
        {
            newRequestAvailable = false;
            currentRequests.Add(r);
            aud[1].Play();
            GameObject inst = (Instantiate(recipeCardPrefab, MainCanvas.transform));
            RecipeCard current = inst.GetComponent<RecipeCard>();
            if(inst1 == null)
            {
                current.DrawRecipe(r.rec.name, r.rec.ingr);
                StartCoroutine(current.SlideIntoPosition(new Vector2(817, 271)));
                inst1 = current;
                if(onRegularTime)
                {
                    StartCoroutine(RequestTimer(r, 1));
                }
                
            }
            else if(inst2 == null)
            {
                current.DrawRecipe(r.rec.name, r.rec.ingr);
                StartCoroutine(current.SlideIntoPosition(new Vector2(527, 271)));
                inst2 = current;
                if (onRegularTime)
                {
                    StartCoroutine(RequestTimer(r, 2));
                }
                
            }

            if (onRegularTime)
            {
                if (currentRequests.Count == 2)
                {
                    newRequestAvailable = false;
                }
                else if (currentRequests.Count == 1)
                {
                    newRequestAvailable = false;
                    StartCoroutine(TimerBetweenRequests(timer(requestsDone) * r.timeLimit));
                }

            }
            
        }
        
    }

    void ClearRequest(Request r)
    {
        if (currentRequests.Contains(r))
        {
            int index = currentRequests.IndexOf(r);
            currentRequests.Remove(r);
            if(index == 0 && inst1 != null)
            {
                Destroy(inst1.gameObject);
                inst1 = null;
            }
            else if (index == 0 && inst2 != null)
            {
                Destroy(inst2.gameObject);
                inst2 = null;
            }

            else if(index == 1 && inst1 != null)
            {
                Destroy(inst1.gameObject);
                inst1 = null;
            }
            else
            {
                Destroy(inst2.gameObject);
                inst2 = null;
            }
            
            if (currentRequests.Count == 0 && onRegularTime)
            {
                newRequestAvailable = true;
            }
        }
        
    }
    IEnumerator TimerBetweenRequests(float time)
    {
        
        yield return new WaitForSeconds(time);
        newRequestAvailable = true;
    }
    IEnumerator RequestTimer(Request r, int requestNumber)
    {
        for(int i = r.timeLimit; i >= 0; i--)
        {
            if (!paused)
            {
                yield return new WaitForSeconds(1);
                if (!currentRequests.Contains(r))
                {
                    break;
                }

                if (requestNumber == 1)
                {
                    inst1.TimerDrawText(i); 
                }
                else
                {
                    inst2.TimerDrawText(i);
                }

            }
            else
            {
                i++;
                yield return new WaitForSeconds(1);
            }
            
        }
        if (currentRequests.Contains(r))
        {
            //time's up!
            ClearRequest(r);
            Debug.Log("Time's up!");
            aud[0].Play();
            misses++;
            switch (misses)
            {
                case 1: missIcons[2].SetActive(false); break;
                case 2: missIcons[1].SetActive(false); break;
                case 3: missIcons[0].SetActive(false); StartCoroutine(LoseGame()); break;
            }
            
        }
        else
        {
            //do nothing
            Debug.Log("Time ran out but you finished the recipe!");
        }
    }

    public void SubmitRecipe(int table)
    {
        Recipe result = new Recipe("",new List<string>());
        if(table == 1)
        {
            result = table1.ValidRecipeCheck(ck);
        }
        else if(table == 2)
        {
            result = table2.ValidRecipeCheck(ck);
        }
        foreach(Request r in currentRequests.ToList())
        {
            if(r.rec.name == result.name)
            {
                aud[2].Play();
                if(r.rec.name == "Tutorial Burger")
                {
                    Request tutTwo = new Request(ck.gameRecipes[1], 999);
                    AddRequestToList(tutTwo);
                    seatLight.SetActive(true);
                    aud[3].volume = 0;
                    aud[4].volume = 0.85f;

                }
                requestsDone++;
                ClearRequest(r);
                

                if (table == 1)
                {
                    table1.EmptyTable();
                }
                else if(table == 2)
                {
                    table2.EmptyTable();
                }
                totalScore += result.score;
                info.drawText();
                break;
            }
        }
    }


    public void PauseGame()
    {
        paused = true;
        Pause.SetActive(true);
    }

    public void Unpause()
    {
        if (Pause.activeInHierarchy){
            Pause.SetActive(false);
            paused = false;
        }
    }

    public IEnumerator LoseGame()
    {
        paused = true;
        Lose.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene("MainMenu");
    }


    IEnumerator Tutorial()
    {
        StartCoroutine(info.DialogueTrigger("Hi! Welcome to the Burger Trucker Burger Truck. Let's get to making burgers, yeah?"));
        yield return new WaitForSeconds(7.0f);
        StartCoroutine(info.DialogueTrigger("Up in the top right is your current order, the Tutorial Burger! Let's start by putting some buns onto this Prep Table..."));
        yield return new WaitUntil(() => paused != true);
    }

    

}


public class Request
{
    public Recipe rec;
    public int timeLimit;
    public Request(Recipe r, int timer)
    {
        rec = r;
        timeLimit = timer;
    }
}

public class Ingredient_Full
{
    public Recipe.Ingredient ingr;
    public int cookValue;
    
    public Ingredient_Full(Recipe.Ingredient ri)
    {
        ingr = ri;
        if(ri != Recipe.Ingredient.Burger && ri != Recipe.Ingredient.Chopped_Onion && ri != Recipe.Ingredient.Chopped_Mushroom)
        {
            cookValue = -1;
        }
        else
        {
            cookValue = 0;
        }
    }
}

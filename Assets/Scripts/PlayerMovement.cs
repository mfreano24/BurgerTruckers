using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //MOVING AROUND
    Vector3 forward, right, moveDirection;
    float xInput, yInput;
    CharacterController cc;
    public GameObject cam;
    public float playerSpeed;
    GameObject currentInteractable;
    string interact_name;
    public Ingredient_Full holding;
    public Recipe holding_finished;
    public DebugInfoText debugText;
    public GameManager gm;
    Rigidbody rb;
    public Rigidbody truckRB;
    public GameObject camLook;
    public TruckDriver truckScript;
    bool truckMode = false;
    public GameObject viewCam;
    public GameObject holdingCanvas;
    public Image HoldingUI;
    public Sprite[] Food_UI;
    public AudioSource aud;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        cam.transform.localPosition = new Vector3(-8.73f, 5.56f, 0.33f);
        holding = null;
        currentInteractable = null;
        rb = GetComponent<Rigidbody>();
        holdingCanvas.SetActive(false);
        truckRB.centerOfMass = new Vector3(0, -2, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!gm.paused) {
            if (!truckMode)
            {
                forward = cam.transform.forward;
                forward.y = 0;
                right = cam.transform.right;
                xInput = Input.GetAxis("Horizontal");
                yInput = Input.GetAxis("Vertical");

                moveDirection = xInput * right + yInput * forward;
                if (holding != null && !holdingCanvas.activeInHierarchy)
                {
                    Debug.Log("Holding UI!");
                    holdingCanvas.SetActive(true);
                    //pick which image to display
                    //0 = top bun   1 = bottom bun  2 = cheese  3 = lettuce   4 = burger    5 = onion,whole
                    //6 = onion, chopped    7 = tomato, whole   8 = tomato, chopped    9 = mushroom, whole 
                    //10 = mushroom, chopped
                    switch (holding.ingr)
                    {
                        case Recipe.Ingredient.Top_Bun: HoldingUI.sprite = Food_UI[0]; break;
                        case Recipe.Ingredient.Bottom_Bun: HoldingUI.sprite = Food_UI[1]; break;
                        case Recipe.Ingredient.Cheese: HoldingUI.sprite = Food_UI[2]; break;
                        case Recipe.Ingredient.Lettuce: HoldingUI.sprite = Food_UI[3]; break;
                        case Recipe.Ingredient.Burger: HoldingUI.sprite = Food_UI[4]; break;
                        case Recipe.Ingredient.Onion: HoldingUI.sprite = Food_UI[5]; break;
                        case Recipe.Ingredient.Chopped_Onion: HoldingUI.sprite = Food_UI[6]; break;
                        case Recipe.Ingredient.Tomato: HoldingUI.sprite = Food_UI[7]; break;
                        case Recipe.Ingredient.Chopped_Tomato: HoldingUI.sprite = Food_UI[8]; break;
                        case Recipe.Ingredient.Mushroom: HoldingUI.sprite = Food_UI[9]; break;
                        case Recipe.Ingredient.Chopped_Mushroom: HoldingUI.sprite = Food_UI[10]; break;
                    }
                }

                if (Input.GetButtonDown("Submit"))
                {
                    Interact();
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    submitRecipe();
                }


            }
            else if (truckMode && Input.GetButtonDown("Cancel"))
            {
                truckScript.truckMode = false;
                truckMode = false;
                CameraModeSwitch();
            }
        }
        else
        {
            if (Input.GetButtonDown("Submit"))
            {
                gm.Unpause();
            }
            else if (Input.GetButtonDown("Pause"))
            {
                SceneManager.LoadScene("MainMenu");
            }
        }
        
        
    }

    private void FixedUpdate()
    {
        rb.velocity = playerSpeed * moveDirection + truckRB.velocity;
        if (moveDirection.magnitude > 0.1f)
        {
            transform.LookAt(transform.position + moveDirection);
        }
    }

    private void LateUpdate()
    {
        if (!truckMode)
        {
            cam.transform.LookAt(camLook.transform);
        }
        else
        {
            cam.transform.LookAt(viewCam.transform);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable_Pickup") || other.CompareTag("Interactable_Utility"))
        {
            currentInteractable = other.gameObject;
            interact_name = other.gameObject.name;
            if(other.gameObject.name == "PREP_TABLE1" || other.gameObject.name == "PREP_TABLE2")
            {
                other.gameObject.GetComponent<PrepTable>().PullUpList();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable_Pickup") || other.CompareTag("Interactable_Utility"))
        {
            currentInteractable = null;
            interact_name = "";
            if (other.gameObject.name == "PREP_TABLE1" || other.gameObject.name == "PREP_TABLE2")
            {
                other.gameObject.GetComponent<PrepTable>().PutAwayList();
            }
        }
    }
    public void submitRecipe()
    {
        if(currentInteractable == null && holding != null)
        {
            holding = null;
            holdingCanvas.SetActive(false);
            Debug.Log("Tossed!");
        }
        else if(currentInteractable.name == "PREP_TABLE1")
        {
            Debug.Log("PREP TABLE 1 CHECK");
            gm.SubmitRecipe(1);
        }
        else if(currentInteractable.name == "PREP_TABLE2")
        {
            Debug.Log("PREP TABLE 2 CHECK");
            gm.SubmitRecipe(2);
        }
    }
    public void Interact()
    {
        //interact with anything you're nearby
        if (currentInteractable.CompareTag("Interactable_Pickup"))
        {
            if (holding == null)
            {
                switch (currentInteractable.name)
                {
                    case "TOP_BUNS": holding = new Ingredient_Full(Recipe.Ingredient.Top_Bun); break;
                    case "BOTTOM_BUNS": holding = new Ingredient_Full(Recipe.Ingredient.Bottom_Bun); break;
                    case "BURGERS": holding = new Ingredient_Full(Recipe.Ingredient.Burger); break;
                    case "CHEESE": holding = new Ingredient_Full(Recipe.Ingredient.Cheese); break;
                    case "ONION": holding = new Ingredient_Full(Recipe.Ingredient.Onion); break;
                    case "LETTUCE": holding = new Ingredient_Full(Recipe.Ingredient.Lettuce); break;
                    case "MUSHROOM": holding = new Ingredient_Full(Recipe.Ingredient.Mushroom); break;
                    case "TOMATO": holding = new Ingredient_Full(Recipe.Ingredient.Tomato); break;
                }
                Debug.Log("Now holding " + holding.ingr);
                aud.Play();
            }
            else
            {
                Debug.Log("You're already holding something!");
            }
        }
        else if (currentInteractable.CompareTag("Interactable_Utility")){
            if (holding != null)
            {
                if(currentInteractable.name == "GRILL_L")
                {
                    if (currentInteractable.transform.parent.gameObject.GetComponent<Grill>().Interact_Place(holding,1))
                    {
                        holding = null;
                        holdingCanvas.SetActive(false);
                        aud.Play();
                    }
                    else
                    {
                        //grill is full
                    }
                    
                }
                else if (currentInteractable.name == "GRILL_R")
                {
                    if (currentInteractable.transform.parent.gameObject.GetComponent<Grill>().Interact_Place(holding, 2))
                    {
                        holding = null;
                        holdingCanvas.SetActive(false);
                        aud.Play();
                    }
                    else
                    {
                        //grill is full
                    }

                }
                else if(currentInteractable.name == "CHOP_TABLE")
                {
                    if (currentInteractable.GetComponent<ChopTable>().Interact_Place(holding))
                    {
                        holding = null;
                        holdingCanvas.SetActive(false);
                        aud.Play();
                    }
                    else
                    {
                        //chop table is full
                    }
                }
                else if(currentInteractable.name == "PREP_TABLE1" || currentInteractable.name == "PREP_TABLE2")
                {
                    if (currentInteractable.GetComponent<PrepTable>().Interact_Place(holding))
                    {
                        holding = null;
                        holdingCanvas.SetActive(false);
                        aud.Play();
                    }
                }
                else if(currentInteractable.name == "DRIVER_SEAT" && gm.requestsDone > 0)
                {
                    truckMode = true;
                    truckScript.truckMode = true;
                    CameraModeSwitch();
                    aud.Play();
                }
            }
            else
            {
                if (currentInteractable.name == "GRILL_L")
                {
                    holding = currentInteractable.transform.parent.gameObject.GetComponent<Grill>().Interact_Take(1);
                    if (holding == null)
                    {
                        Debug.Log("Grill was empty");
                    }
                    else
                    {
                        Debug.Log("Now holding " + holding.ingr);
                        aud.Play();
                    }

                }
                else if (currentInteractable.name == "GRILL_R")
                {
                    holding = currentInteractable.transform.parent.gameObject.GetComponent<Grill>().Interact_Take(2);
                    if (holding == null)
                    {
                        Debug.Log("Grill was empty");
                    }
                    else
                    {
                        Debug.Log("Now holding " + holding.ingr);
                        aud.Play();
                    }

                }
                else if (currentInteractable.name == "CHOP_TABLE")
                {
                    holding = currentInteractable.GetComponent<ChopTable>().Interact_Take();
                    if (holding == null)
                    {
                        Debug.Log("Table was empty");
                    }
                    else
                    {
                        Debug.Log("Now holding " + holding.ingr);
                        aud.Play();
                    }
                }
                else if(currentInteractable.name == "PREP_TABLE1" || currentInteractable.name == "PREP_TABLE2")
                {
                    holding = currentInteractable.GetComponent<PrepTable>().Interact_Take();
                    if(holding == null)
                    {
                        Debug.Log("Table was empty");
                    }
                    else
                    {
                        Debug.Log("Now holding " + holding.ingr);
                        aud.Play();
                    }
                }
                else if (currentInteractable.name == "DRIVER_SEAT")
                {
                    truckMode = true;
                    truckScript.truckMode = true;
                    CameraModeSwitch();
                    aud.Play();
                }
            }
        }
    }

    void CameraModeSwitch()
    {
        //TODO: IEnumerate this please.
        if (truckMode)
        {
            //switch to truck position
            cam.transform.localPosition = new Vector3(-0.4f, 14.41f, 17.31f);
            cam.transform.rotation = Quaternion.Euler(20f, 90, 0);
        }
        else
        {
            //switch to food position
            cam.transform.localPosition = new Vector3(-8.73f, 5.56f, 0.33f);
            cam.transform.rotation = Quaternion.Euler(12.19f, 0, 0);
        }
    }
}

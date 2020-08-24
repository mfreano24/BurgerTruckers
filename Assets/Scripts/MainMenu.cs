using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    Vector2[] positions = new Vector2[2];
    public RectTransform icon;
    int currIndex = 0;
    bool buffer = false;

    public Text howToPlayText;
    public GameObject htpPanel;
    public GameObject layout;

    AudioSource aud;

    bool inHTP = false;
    int htpIndex = 0;
    string[] h;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        layout.SetActive(false);
        string[] htp = {"1- BASIC CONTROLS\nWASD/Left Stick -> Move\nA/Spacebar or Enter -> Interact\nB/Right Shift ->  Throw Item Away/Cancel",
        "2- Your job is to make burgers and send em out, fresh to order. There are several tools to use for this.\n\nPREP TABLE: This is where you \"compile\" your ingredients. "+
        "On here you should put every ingredient thats on the recipe card exactly, then press (B) or RightShift to send it out. Interacting with the table while it has food will wipe it clean.\n\n"+
        "CHOP TABLE: Some ingredients need to be chopped: MUSHROOMS, ONIONS, TOMATOES.\nInteract with the chop table to \"chop\" an ingredient, and iteract again to pick it back up.\n\n"+
        "GRILL: Some ingredients need to be cooked: MUSHROOMS, BURGERS, ONIONS.\nYou have 2 spaces on the grill: left and right. Interact with one to put a cookable ingredient down."+
        "After its cooked enough pick it back up and take it to prep.",
        "3- After your first order, the truck will start to go out of control and accelerate. It's your job to go to the STEERING WHEEL and keep it in control."+
        " Keep in mind that you still have to make burgers!\n"+
        "Controls: Left / Right - steer, B/RShift to exit truck mode\n\nYour first two burgers are untimed (save for the truck driving), so make sure to get a feel for everything!"};

        h = htp;





        positions[0] = new Vector2(182, -79);
        positions[1] = new Vector2(315, -285);
    }

    
    void Update()
    {
        if (!inHTP)
        {
            if (!buffer)
            {
                if (Input.GetAxis("Vertical") == -1)
                {
                    currIndex++;
                    aud.Play();
                    if (currIndex > 1)
                    {
                        currIndex = 0;
                    }
                    StartCoroutine(cooldown());
                }
                else if (Input.GetAxis("Vertical") == 1)
                {
                    currIndex--;
                    aud.Play();
                    if (currIndex < 0)
                    {
                        currIndex = 0;
                    }
                    StartCoroutine(cooldown());

                }

            }

            icon.anchoredPosition = positions[currIndex];

            if (Input.GetButtonDown("Submit"))
            {
                aud.Play();
                if (currIndex == 0)
                {
                    htpPanel.SetActive(true);
                    inHTP = true;
                    howToPlayText.text = h[0];
                }
                else
                {
                    Quit();
                }
            }

        }
        else
        {
            //how to play
            if (Input.GetButtonDown("Submit"))
            {
                aud.Play();
                htpIndex++;
                
                if(htpIndex == 2)
                {
                    layout.SetActive(true);

                }
                else if(htpIndex == 3)
                {
                    SceneManager.LoadScene("Main");
                }
                howToPlayText.text = h[htpIndex];

            }


        }
        
        

        
    }

    IEnumerator cooldown()
    {
        buffer = true;
        yield return new WaitForSeconds(0.75f);
        buffer = false;
    }


    void Quit()
    {
        Application.Quit();
    }


}

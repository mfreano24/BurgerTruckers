using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DebugInfoText : MonoBehaviour
{
    public Text t;
    public Text back;

    public Grill gr;
    public ChopTable ct;

    public Text Dialogue;

    public GameManager gm;
    int sc;
    void Start()
    {
        sc = gm.totalScore;
        //Dialogue.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        sc = gm.totalScore;
    }

    public IEnumerator DialogueTrigger(string s)
    {
        Dialogue.text = "";
        for (int i = 0; i < s.Length; i++)
        {
            Dialogue.text += s[i];
            yield return new WaitForSeconds(0.03f);
        }

    }


    public void drawText()
    {
        t.text = "Score: " + sc + "\tRequests Done: " + gm.requestsDone;
        back.text = t.text;
    }
}

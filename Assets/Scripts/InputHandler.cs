using UnityEngine;
using TMPro;

public class InputHandler : MonoBehaviour
{
    [SerializeField] TMP_Text  text;

    [SerializeField] Transform textbox;

    [SerializeField] Transform textboxPast;

    [SerializeField] bool uiUpdated = true;

    [SerializeField] bool inUse = false;

    [SerializeField] LevelHandler levelHandler;
    [SerializeField] UIHandler UIhandler;

    private static string[] words; 
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(levelHandler == null)
        {
            levelHandler = GetComponent<LevelHandler>();
            if(levelHandler == null)
            {
                Debug.LogError("Input handler failed to get LevelHandler!");
            }
        }



        if(UIhandler == null)
        {
            UIhandler = GetComponent<UIHandler>();

            if(UIhandler == null)
            {
                Debug.LogError("Input handler failed to get UIHandler!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform trans;

        //If UI tells inputhandler to turn on
        //Begin looking for the correct UI to edit
        //If find the edit UI, allow for editing!
        //Otherwise fail

        trans = UIHandler.GetTypingTextFromCurrentUI();
        //trans is the text's transform

        if(trans != null && inUse == true)
        {
            if(uiUpdated == true)
            {
                text = LookForText(trans);
                uiUpdated = false;
            }

            if(text != null)
            {
                Debug.Log("Start Typing!");
                foreach (char c in Input.inputString)
                {
                    if (c == '\b') // has backspace/delete been pressed?
                    {
                        if (text.text.Length != 0)
                        {
                            text.text = text.text.Substring(0, text.text.Length - 1);
                        }
                    }
                    else if ((c == '\n') || (c == '\r')) // enter/return
                    {
                        print("User entered their name: " + text.text);
                    }
                    else
                    {
                        text.text += c;
                    }
                }
            }
        }
        //Debug.Log("Name: " + trans.name);
    
    }

    /*
        Given a transform of a UI element, returns the typing box for the user to type in.
        Or null if not found.
    */
    private TMP_Text LookForText(Transform ui)
    {
        TMP_Text text = null;

        foreach(Transform element in ui)
        {
            Debug.Log(element.name);
            if(string.Equals(element.name,"Typing"))
            {
                text = element.GetComponent<TMP_Text>();
                Debug.Log("Found the typing element!");
                return text;
            }
        }

        Debug.Log("Did not find the typing element!");
        return text;
    }

    public void SetUpdateUI(bool val)
    {
        uiUpdated = val;
    }

    public bool GetUpdateUI()
    {
        return uiUpdated;
    }

    public void SetInUse(bool val)
    {
        inUse = val;
    }

    public bool GetInUse(bool val)
    {
        return inUse;
    }
}

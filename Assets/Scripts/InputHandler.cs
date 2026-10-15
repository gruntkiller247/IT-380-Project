using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;

public class InputHandler : MonoBehaviour
{
    private TMP_Text  text;

    private Transform textbox;

    private Transform textboxPast;

    [SerializeField] bool uiUpdated = true;

    [SerializeField] bool inUse = false;

    private LevelHandler levelHandler;
    private UIHandler uiHandler;
    [SerializeField] bool needUpdateWord = false;
    

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



        if(uiHandler == null)
        {
            uiHandler = GetComponent<UIHandler>();

            if(uiHandler == null)
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
                if(needUpdateWord == true)
                {
                    uiHandler.UpdateNextWord();
                    needUpdateWord = false;
                }
                
                //Debug.Log("Start Typing!");
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
                        //print("User entered their name: " + text.text);
                        text.text = "";
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
        //Debug.Log(ui.name);

        text = ui.GetComponent<TMP_Text>();

        if(text == null)
        {
            Debug.Log("Did not find the typing element!");
        }

        
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

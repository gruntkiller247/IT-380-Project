using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using System;
using UnityEditor.Search;

public class InputHandler : MonoBehaviour
{
    private TMP_Text  text;

    private string colorlessText;
    [SerializeField] bool uiUpdated = true;

    [SerializeField] bool inUse = false;

    //private LevelHandler levelHandler;
    private UIHandler uiHandler;
    [SerializeField] bool needUpdateWord = false;

    private string currentWord = null;

    private int currentWordIndex = 0;

    private int numGreen = 0;

    private bool wordRed = false;

    //This script speaks to the UIHandler and ask how the game should be handled
    //THIS SCRIPT SHOULD NOT BE SPEAKING TO THE LEVEL HANDLER!

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*
        if(levelHandler == null)
        {
            levelHandler = GetComponent<LevelHandler>();
            if(levelHandler == null)
            {
                Debug.LogError("Input handler failed to get LevelHandler!");
            }
        }
        */

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
        trans = UIHandler.GetTypingTextFromCurrentUI();

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
                    currentWord = uiHandler.GetWordToWrite();
                    needUpdateWord = false;
                    currentWordIndex = 0;
                    numGreen = 0;
                    colorlessText = "";
                }
                
                //text.text is each symbol written by the player
                
                foreach (char c in Input.inputString)
                {
                    if (c == '\b') // has backspace/delete been pressed?
                    {
                        /*
                        colorlessText = colorlessText.Substring(0,colorlessText.Length-1);
                        numGreen = 0;
                        text.text = "";
                        RemakeWord();
                        currentWordIndex = 0;
                        */
                        
                        //On deletion
                        //Remove the last letter to the colorlessText
                        //text.text = ""
                        //remake text.text based on colorlessText
                        

                        if (text.text.Length != 0)
                        {
                            text.text = text.text.Substring(0, text.text.Length - 1);
                        }

                        wordRed = false;

                    }
                    else if ((c == '\n') || (c == '\r')) // enter/return
                    {
                        //print("User entered their name: " + text.text);
                        text.text = "";
                    }
                    else
                    {
                        if(ValidateText(c) == true)
                        {
                            numGreen++;
                            ColorChar(c,'g');
                            colorlessText += colorlessText + c;
                            currentWordIndex++;
                        }
                        else
                        {
                            ColorChar(c,'r');
                            colorlessText += colorlessText + c;
                        }
                        
                        
                        
                    }
                }
            }
        }
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

    /*
        Checks if the inputted char is the same as the current char in the current word
        Returns true if yes, false in all other cases
    */
    private bool ValidateText(char word)
    {
        char[] ogWordArray; 

        if(currentWordIndex < 0 || currentWordIndex >= currentWord.Length)
        {
            Debug.LogError("InputHandler Validate Text given an out of bounds index!");
            return false;
        }

        if(word == '/' || word == '>' || word == '<' || word == '\\' || word == '\'' || word == '\"')
        {
            Debug.LogError("User gave an illegal character!");
            return false;
        }
        
        ogWordArray = currentWord.ToCharArray();
        

        if(word == ogWordArray[currentWordIndex])
        {
            return true;
        }
        return false;
    }

    /*
        Changes the color of the character to the color sent by validate letter
        return nothing but adds the colored phrase to the visible text
    */
    private void ColorChar(char let, char color)
    {
        if(wordRed == true)
        {
            text.text += "<color=red>" + let + "</color>"; 
            return;
        }

        if(color == 'g')
        {
            text.text += "<color=green>" + let + "</color>";
            return;
        }
        else
        {
            //Default to red
            text.text += "<color=red>" + let + "</color>";
            wordRed = true;
        }

              
    }

    /*
        Remakes the visible word using the colorlessWord as refrence
        Returns the colored word to be displayed
    */
    private void RemakeWord()
    {

        foreach(char c in colorlessText)
        {
            ValidateText(c);
            currentWordIndex++;
        }


    }
}

using UnityEngine;
using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using System;
using UnityEditor.Search;

public class InputHandler : MonoBehaviour
{
    private TMP_Text  text;                                     //text.text is each symbol written by the player displayed with color

    private string colorlessText;                               //colorlessText is each symbol without the color tags
    [SerializeField] bool uiUpdated = true;                     //bool indicating whether the UI has been updated and the input handler needs to reset the word
                                                                //To be called by game functionality

    [SerializeField] bool inUse = false;                        //bool indicating whether the InputHandler should be reading inputs
                                                                //To be called by game functionality - do not want to read inputes without a typing text box nor the game running!     
    private UIHandler uiHandler;                                //Refrence to the uiHandler. uiHandler is the broker between Input and Game
    [SerializeField] bool needUpdateWord = false;               //Bool indicating whether the current word is finished and the next word needs to be given

    private string currentWord = null;                          //String storing the current word to be typed

    private int currentWordIndex = 0;                           //Integer storing the current index on where the user wrote the correct characters in the word up to
                                                                //EX: it would be 2 if the word is Apple and the user wrote Aplll

    private int numGreen = 0;                                   //Number of correctly written chars
                                                                //Maye be depreciated in favor of just comparing the indexs and the number of words remaining!

    private bool wordRed = false;                               //Bool indicating whether the characters should always been drawn as red
                                                                //Used when the user has missed more than 1 character
    

    //This script speaks to the UIHandler and ask how the game should be handled
    //THIS SCRIPT SHOULD NOT BE SPEAKING TO THE LEVEL HANDLER!

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
                

                foreach (char c in Input.inputString)
                {
                    if (c == '\b') // has backspace/delete been pressed?
                    {                        
                        
                        if(colorlessText.Length != 0)
                        {
                            colorlessText = colorlessText.Substring(0, colorlessText.Length - 1);
                            RemakeWord();
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
                            ColorChar(c,'g');
                            numGreen++;
                            currentWordIndex++;
                        }
                        else
                        {
                            ColorChar(c,'r');
                        }
                        colorlessText +=  c;
                        
                        
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
        Returns false if the index is out of bounds, the user gave an illegal character, or 
        The char is not the same as the current char: a != p
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
        Changes the color of the character to the color sent by validate letter and adds the char to the display text
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
        text.text = colorlessWord but with color
    */
    private void RemakeWord()
    {
        char[] words = colorlessText.ToCharArray();
        text.text = "";
        currentWordIndex = 0;
        wordRed = false;
        
        foreach(char c in words)
        {
            if(ValidateText(c) == true)
            {
                ColorChar(c,'g');
                currentWordIndex++;
                numGreen++;
            }
            else
            {
                ColorChar(c,'r');
            }

        }



    }
}

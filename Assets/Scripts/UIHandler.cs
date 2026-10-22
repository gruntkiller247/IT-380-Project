using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Canvas menus;

    [SerializeField] static String nameOfTypingBox = "Typing";
    [SerializeField] static String nameOfWordBox = "Word";

    [SerializeField] static String nameOfResultsBox = "Score";
    private static Transform[] elements; 
    
    private LevelHandler levelHandler;
    private InputHandler inputHandler;
    void Start()
    {
        if(elements == null)
        {
            elements = menus.GetComponentsInChildren<Transform>(true);
        }

        if(levelHandler == null)
        {
            levelHandler = GetComponent<LevelHandler>();
            if(levelHandler == null)
            {
                Debug.LogError("Input handler failed to get LevelHandler!");
            }
        }

        if(inputHandler == null)
        {
            inputHandler = GetComponent<InputHandler>();
            if(inputHandler == null)
            {
                Debug.LogError("UIHandler failed to find the InputHandler!");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnToLevelSelect()
    {
        DeactivateThenActivate(GetTransformByName("Level Select"));
    }

    public void ToEasyLevel()
    {
        DeactivateThenActivate(GetTransformByName("Level Easy"));
    }

    public void ToMediumLevel()
    {
        DeactivateThenActivate(GetTransformByName("Level Medium"));
    }

    public void ToHardLevel()
    {
        DeactivateThenActivate(GetTransformByName("Level Hard"));
    }

    public void ToTestLevel()
    {
        DeactivateThenActivate(GetTransformByName("Level Test"));
        StartLevel("Level Test");
    }


    public void ToLevelSelect()
    {
        DeactivateThenActivate(GetTransformByName("Level Select"));   
    }

    public void ToTutorial()
    {
        Transform thing;
        thing = GetTransformByName("Tutorial Menu");

        if(thing == null)
        {
            //Debug.LogError("Failed to find the Tutorial Menu!");
            return;
        }

        //Debug.Log("Found component: " + thing.name);

        DeactivateThenActivate(thing);
        
    }

    /*

    */
    public static Transform GetResultsTextFromCurrentUI()
    {
        Transform currentActive = GetActiveUI();
        
        
        foreach(Transform element in currentActive)
        {
            
            if(string.Equals(element.name,nameOfResultsBox))
            {
                return element;
            }

        }

        return null;
    }

    
    /*
        Called by InputHandler when the game ends normally.
        Recieves number of mistakes made and the time when the input manager was started for grading
    */
    public void EndLevel(int numCharMistakes, float levelStartTime)
    {
        Transform element;
        string endText;
        TMP_Text text;
        //Debug.Log("Level is over!");

        levelHandler.SetInLevel(false);
        //Need to then open the UI to the results screen!
        DeactivateThenActivate(GetTransformByName("Results")); 

        element = GetActiveUI();

        //Debug.Log("Element name: " + element.name);

        if(element != null)
        {
            
            endText = "You made: " + numCharMistakes + " mistakes!\nYou took " +  ((Time.time - levelStartTime)).ToString() + " seconds!"; 
            
            element = GetNamedTextFromCurrentUI("Score");

            if(element == null)
            {
                Debug.LogError("Text element is NULL!");
                return;
            }

            text = element.GetComponent<TMP_Text>();
            
            if(text == null)
            {
                Debug.LogError("TMP_Text component came back NULL!");
                return;
            }

            text.text = endText;

        }
        else
        {
            Debug.LogError("Failed to find the active UI for results!");
        }
        levelHandler.StopLevelHandler();
        inputHandler.StopInputHandler();
    }



    /*
        Recieves a string that is the name of the screen to go to. IE "Home"
        Deactives the current level + stops the input manager. Does not change the UI menu!
        Returns nothing.
    */
    public void StopLevel(String levelName)
    {
        Transform level;
        level = GetTransformByName(levelName);

        if(level == null)
        {
            Debug.LogError("Stop Level failed to find a level! Nothing could be stopped!");
            return;
        }

        inputHandler.StopInputHandler();
        levelHandler.StopLevelHandler();
    }

    /*
        Recieves a string that is the name of a level. Iterates through the array for the matching level.
        Tells that level to start + starts the input manager.
        Returns nothing.
    */
    public void StartLevel(String levelName)
    {
        Transform level;
        level = GetTransformByName(levelName);

        if(level == null)
        {
            Debug.LogError("Start Level failed to find a level! Nothing was started!");
            return;
        }

        //Must call the stack in this order!
        inputHandler.StartInputHandler();
        levelHandler.StartLevelHandler();
    }

    /*
        Returns the next word or null
    */

    public string GetWordToWrite()
    {
        return levelHandler.GetWordToWrite();
    }

    /*
        Prompts the Level Handler for the next word then sets the text of the Word Text to that word
    */

    public void UpdateNextWord()
    {
        Transform wordBox = GetWordTextFromCurrentUI();
        String word = null;
        TMP_Text temp = null;

        if(wordBox == null)
        {
            Debug.LogError("There is no word text box!");
            return;
        }

        levelHandler.UpdateWord();
        word = levelHandler.GetAndUpdateWordToWrite();

        if(word == null)
        {
            //Debug.LogError("There is no word to write! Defaulting to NULL!");
            word = "NULL!";
        }

        temp = wordBox.GetComponent<TMP_Text>();

        if(temp == null)
        {
            Debug.LogError("Failed to get wordBox's TMP_Text component!");
            return;
        }

        temp.text = word;
        
    }

    /*

    */
    public static Transform GetNamedTextFromCurrentUI(string name)
    {
        Transform currentActive = GetActiveUI();

        foreach(Transform element in currentActive)
        {
            
            if(string.Equals(element.name,name))
            {
                return element;
            }
        }

        return null;
    }

    /*
        Returns the current active UI's text that displays the current word to be typed
        Returns a transform of the active text UI or null
    */

    public static Transform GetWordTextFromCurrentUI()
    {
        Transform currentActive = GetActiveUI();

        foreach(Transform element in currentActive)
        {
            
            if(string.Equals(element.name,nameOfWordBox))
            {
                return element;
            }
        }

        return null;
    }

    /*
        Returns the current active UI's text that can be written to by the player
        Returns a transform of the active text UI or null
    */
    public static Transform GetTypingTextFromCurrentUI()
    {
        Transform currentActive = GetActiveUI();
        
        
        foreach(Transform element in currentActive)
        {
            
            if(string.Equals(element.name,nameOfTypingBox))
            {
                return element;
            }


        }

        return null;
    }
    
    /*
        Returns the current active UI as a transform or null
    */
    public static Transform GetActiveUI()
    {
        foreach(Transform element in elements)
        {
            if(element.gameObject.activeInHierarchy == true)
            {
                if(!string.Equals("Background - Main",element.gameObject.name) && !string.Equals("Canvas",element.gameObject.name))
                {
                    return element;
                } 
            }
        }

        return null;
    }


    /*
        Deactivates all other UI then activates the transform UI + children    
    */
    public static void DeactivateThenActivate(Transform thing)
    {
        if(thing == null)
        {
            Debug.LogError("Deactuvate then Activate: Given a null UI!");
            return;
        }

        DeactivateAllOtherUI(thing);
        ActivateUI(thing);
    }

    /*
        Iterates through the element array and activates the transform + all children
    */
    public static void ActivateUI(Transform thing)
    {

        if(thing == null)
        {
            Debug.LogError("Given a null transform!");
            return;
        }

        Transform parent = thing;

        while (parent != null)
        {
            parent.gameObject.SetActive(true);
            parent = parent.parent;
        }

        foreach (Transform child in thing.GetComponentsInChildren<Transform>(true))
        {
            child.gameObject.SetActive(true);
        }
    }


    /*
        Deactivates a UI element by name
    */
    public static void DisableUIByName(String name)
    {
        if(name == null)
        {
            Debug.LogError("Given a null transform!");
            return;
        }

        if(string.Equals(name,"Background - Main"))
        {
            Debug.LogError("Cannot disable the Background - Main UI element!");
            return ;
        }
        
        foreach(Transform element in elements)
        {
          
            if(string.Equals(element.name,name))
            {
                 element.gameObject.SetActive(false);
                 return;
            }
            
        }
        Debug.LogError("Failed to find UI to disable!");

    }

    /*
        Iterates through the element array and sets active false everything but the given component and the Background - Main. 
        Does not enable any inactive UI!
        Searches by NAME of the transform!
    */
    public static void DeactivateAllOtherUI(Transform thing)
    {
        if(thing == null)
        {
            Debug.LogError("Given a null transform!");
            return;
        }

        foreach(Transform element in elements)
        {
           //Debug.Log("\n"+ element.name + "\n" + thing.name);
           if (element == thing || element.IsChildOf(thing.transform) || string.Equals(element.name,"Background - Main"))
           {
                continue;
           }
           
           element.gameObject.SetActive(false);
        }
            
    }

    /*
        Iterates through the element array looking for the named canvas.
        Returns the canvas or null
    */
    public Transform GetTransformByName(string name)
    {
        if(name == null)
        {
            Debug.LogError("GetCanvasByName given a null name!");
            return null;
        }

        foreach(Transform element in elements)
        {
            //Debug.Log(element.name);

            if(string.Equals(element.name,name))
                return element;
        }
            

        return null;
    }


}

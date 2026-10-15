using System;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    [SerializeField] Canvas menus;

    [SerializeField] static String nameOfTypingBox = "Typing";
    private static Transform[] elements; 
    
    void Start()
    {
        if(elements == null)
        {
            elements = menus.GetComponentsInChildren<Transform>(true);
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


    //Depreciated
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
            Debug.LogError("Given a null UI!");
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

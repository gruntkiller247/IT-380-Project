using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //[SerializeField] List<Menus> menus = new List<Menus>();

    [SerializeField] GameObject mainMenu;
    //[SerializeField] GameObject menu2;

    [SerializeField] GameObject tutorialMenu;

    [SerializeField] Canvas menus;
    private static Transform[] elements; 



    void Start()
    {
        if(elements == null)
            elements = menus.GetComponentsInChildren<Transform>(true);
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        //Debug.Log("Button Clicked");
        
        if(mainMenu.activeSelf)
        {
            //Debug.Log("Changing Menu!");
            mainMenu.SetActive(false);
            tutorialMenu.SetActive(true);
        }
        
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
        Deactivates all other UI then activates the transform UI + children    
    */
    private static void DeactivateThenActivate(Transform thing)
    {
        DeactivateAllOtherUI(thing);
        ActivateUI(thing);
    }

    /*
        Iterates through the element array and activates the transform + all children
    */
    private static void ActivateUI(Transform thing)
    {
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
        Iterates through the element array and sets active false everything but the given component. 
        Does not enable any inactive UI!
        Searches by NAME of the transform!
    */
    private static void DeactivateAllOtherUI(Transform thing)
    {
        foreach(Transform element in elements)
        {
           //Debug.Log("\n"+ element.name + "\n" + thing.name);
           if (element == thing || element.IsChildOf(thing.transform))
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
    private Transform GetTransformByName(string name)
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

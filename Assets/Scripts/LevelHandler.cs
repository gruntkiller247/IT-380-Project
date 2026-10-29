using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour
{
    [SerializeField]  bool inLevel = false;

    private string[] words = {"Apple"};//{"Apple", "Bacon", "Cups"};
    //[SerializeField] string currentWord = null;

    private static int index = 0;

    public static bool updateWord = false;

    private Object[] levelText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        Debug.Log("\n");
        for(int c=0;c<words.Length;c++)
        {
            Debug.Log(words[c]);
        }
        Debug.Log("\n");
        */

        //Debug.Log("Current index: " + index);

    }

    public void SetInLevel(bool inbool)
    {
        inLevel = inbool;
    }

    public bool GetInLevel()
    {
        return inLevel;
    }

    /*
        Returns the word to write or null if there is no active level or no more words
    */
    public string GetWordToWrite()
    {
        if(inLevel == true)
        {
            if(index-1 < words.Length)
                return words[index-1];
            else
            {
                //Debug.Log("Ran out of words to write! Returning Null!");
                return null;
            }
                
        }

        return null;
    }

    /*
        Returns the current word to write in the level's list.
        Or return null in a level is not currently running!
    */
    public string GetAndUpdateWordToWrite()
    {
        if(inLevel == true)
        {
            //updateWord = false;
            index++;
            return GetWordToWrite();
            
        }
        else
            return null;
    }

    public void UpdateWord()
    {
        //updateWord = true;

        if(inLevel == true)
        {
            //Debug.Log("Index: " + index);
            
        }
    }

    public void StartLevelHandler(string levelName)
    {
        int c = 0;
        string temp = "";
        List<string> wordList = new List<string>();

        if(levelName == null)
        {
            Debug.LogError("Illegel level name given! No level can start!");
            return;
        }

        levelText = Resources.LoadAll("Levels/" + levelName,typeof(TextAsset));

        if(levelText == null)
        {
            Debug.LogError("Failed to load the levelText file!");
            return;
        }
        else
        {
            //words = new string[levelText[0].ToString().Length];
            //Needs to be rewritten. Length is too long needs to be equal to num words
            
            for(c = 0;c < levelText[0].ToString().Length;c++)
            {
                //Debug.Log(levelText[0].ToString()[c]);
                if(levelText[0].ToString()[c].CompareTo(',') != 0)
                {
                    temp = temp + levelText[0].ToString()[c];
                }
                else
                {
                    //Hit a csv spot
                    //words[c] = temp;
                    wordList.Add(temp);
                    temp = "";
                }
            }

            words = wordList.ToArray();

        }

        

        inLevel = true;
    }

    public void StopLevelHandler()
    {
        inLevel = false;
        index = 0;
    }

}

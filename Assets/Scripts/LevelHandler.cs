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
        Debug.Log("\n");
        for(int c=0;c<words.Length;c++)
        {
            Debug.Log(words[c]);
        }
        Debug.Log("\n");

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
        string[] wordList;
        int c = 0;
        string temp = "";

        if(levelName == null)
        {
            Debug.LogError("Illegel level name given! No level can start!");
            return;
        }

        levelText = Resources.LoadAll(levelName,typeof(TextAsset));

        if(levelText == null)
        {
            Debug.LogError("Failed to load the levelText file!");
            return;
        }
        else
        {
            //Debug.Log("Found the levelTexT!");
            //wordList = new string[levelText.Length];

            for(c = 0; c < levelText.Length;c++)
            {
                //Debug.Log(levelText[c].ToString());

                //for(d = 0;d < levelText[c].len)
                //This gets the entire chunk of words + commas
                //I gotta substring this shit!
                //Using indexOf ',' and split based on that
                //wordList[c] = (string) levelText[c];
            }

            words = new string[levelText[0].ToString().Length];

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
                    Debug.Log(temp);
                    words[c] = temp;
                    Debug.Log(words[c] + "\n");
                    temp = "";
                }
                
            }

        }

        

        inLevel = true;
    }

    public void StopLevelHandler()
    {
        inLevel = false;
        index = 0;
    }

}

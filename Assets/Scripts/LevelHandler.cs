using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField]  bool inLevel = false;

    private string[] words = {"Apple", "Bacon", "Cups"};
    //[SerializeField] string currentWord = null;

    private static int index = 0;

    public static bool updateWord = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(inLevel == true)
        {
            //Currently doing a test level!
            
        }
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
        Returns the current word to write in the level's list.
        Or return null in a level is not currently running!
    */
    public string GetWordToWrite()
    {
        if(inLevel == true && updateWord == true)
        {
            updateWord = false;
            return words[index++];
        }
            
        else
            return null;
    }

    public void UpdateWord()
    {
        updateWord = true;
    }

}

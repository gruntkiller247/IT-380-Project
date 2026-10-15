using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    [SerializeField] bool inLevel = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(inLevel == true)
        {
            
        }
    }

    public void setInLevel(bool inbool)
    {
        inLevel = inbool;
    }

    public bool getInLevel()
    {
        return inLevel;
    }
}

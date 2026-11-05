using UnityEngine;

public class RecipeSelector : MonoBehaviour
 {
    [SerializeField] string levelPanelName = "Level Test";

    private string selectedRecipe = "";

    private UIHandler ui;
    private LevelHandler levelHandler;
    private InputHandler inputHandler;

    void Start() 
    {
        ui = FindFirstObjectByType<UIHandler>();

        if(ui == null) 
        {
            Debug.LogError("RecipeSelector could not find the UIHandler");
            return;
        }

        levelHandler = ui.GetComponent<LevelHandler>();
        inputHandler = ui.GetComponent<InputHandler>();
    }

    public void RecipeSelectionScreen() 
    {
        UIHandler.DeactivateThenActivate(ui.GetTransformByName("Recipe Select"));
    }

    //saves the recipe the player selected then moves to the diffiulty screen
    public void SelectRecipe(string recipeName)
     {
        if(string.IsNullOrEmpty(recipeName)) 
        {
            Debug.LogError("SelectRecipe did not receive a valid recipe name");
            return;
        }

        selectedRecipe = recipeName;
        UIHandler.DeactivateThenActivate(ui.GetTransformByName("Level Select"));
    }

    public void StartRecipe(string difficulty) 
    {
        if(selectedRecipe == "") 
        {
            Debug.LogError("No recipe selected");
            return;
        }

        StartLevelUsingFolder(selectedRecipe + "_" + difficulty);
    }

    //loads the level using the folder
    public void StartLevelUsingFolder(string folder) 
    {
        if(ui == null || levelHandler == null || inputHandler == null) 
        {
            Debug.LogError("RecipeSelector is missing level or input handler");
            return;
        }

        //makes sure a txt file for the level exists in the folder
        if(Resources.LoadAll("Levels/" + folder, typeof(TextAsset)).Length == 0) 
        {
            Debug.LogError("No level was found");
            return;
        }

        //looks for the panel that is displayed in the level
        Transform panel = ui.GetTransformByName(levelPanelName);

        if(panel == null) 
        {
            Debug.LogError("The RecipeSelector did not find the panel");
            return;
        }

        UIHandler.DeactivateThenActivate(panel);

        //starts inputHandler so user can type during the level
        inputHandler.StartInputHandler();
        //starts levelHandler so the level can be played by using the folder name
        levelHandler.StartLevelHandler(folder);
    }

    //fixes the Typing box error from before by stopping the level and making it go back to Level Select
    public void ExitLevel() 
    {
        inputHandler.StopInputHandler();
        levelHandler.StopLevelHandler();
        UIHandler.DeactivateThenActivate(ui.GetTransformByName("Level Select"));
    }

    //recipes for our game (might change recipes, but for now these work for examples)
    public void SelectEgg()      { SelectRecipe("SunnySideEggs"); }
    public void SelectBrownie()   { SelectRecipe("Brownie"); }

    //got to think of what kind of chicken recipe might just do breaded chicken
    public void SelectChicken()    { SelectRecipe("Chicken"); }

    //other recipe ideas that we could add
    //public void SelectPancakes()  { SelectRecipe("Pancakes"); }
    //public void SelectPasta()     { SelectRecipe("Pasta"); }
    //public void SelectCookies()   { SelectRecipe("Cookies"); }

    //these are the difficulty for the selected recipes
    public void StartEasy()      { StartRecipe("Easy"); }
    public void StartMedium()    { StartRecipe("Medium"); }
    public void StartHard()      { StartRecipe("Hard"); }
}

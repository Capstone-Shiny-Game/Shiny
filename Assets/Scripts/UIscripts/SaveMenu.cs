using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    private List<string> gameNames;
    string Savename;
    private bool savingIsEnabled;

    public void OnEnable()
    {
        gameNames = Save.GetSaveFileNames();

        if (savingIsEnabled)
        {
            //TODO set save menu active
        }
        else { 
            //TODO set load menu active
        }
    }

    /// <summary>
    /// set if the save menu should be enabled or the load menu
    /// </summary>
    /// <param name="enable"></param>
    public void SetSavingisEnabled(bool enable) {
        savingIsEnabled = enable;
    }


    public void LoadGame()
    {
        Savename = "hi"; //TODO get save name from user from list picker
        //TODO warn player that it will overrite current game
        Save.LoadDataJson(Savename); 
    }
    public void SaveGame()
    {
        Savename = "hi"; //TODO get save name from user
        if (gameNames.Contains(Savename))
        {
            //TODO: tell user name is already in use and ask to confirm with overwrite
            //TODO: add return statement
        }
        makeSaveFile();
    }

    public void overwriteSave()
    {
        Savename = ""; //TODO get save name from user
        makeSaveFile();
    }

    /// <summary>
    /// helper function for code re-use
    /// </summary>
    private void makeSaveFile()
    {
        Save.SaveDataJson(Savename);
    }
}

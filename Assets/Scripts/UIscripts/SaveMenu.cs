using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    private List<string> gameNames;
    string Savename;
    private bool savingIsEnabled;
    public ConfirmPopup confirmPopup;

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
        Savename = "save1"; //TODO get save name from user from list picker
        //TODO warn player that it will overrite current game
        Save.LoadDataJson(Savename); 
    }
    public void SaveGame()
    {
        Savename = "save1"; //TODO get save name from user
        if (gameNames.Contains(Savename))
        {
            confirmPopup.ShowPopUP("are you sure you want to overwrite this Save?\n\"" + Savename + "\"", confirmOverwriteSave);
            this.gameObject.SetActive(false);
            return;
        }
        makeSaveFile();
    }

    public void confirmOverwriteSave(bool value)
    {
        this.gameObject.SetActive(true);
        if (value)
        {
            makeSaveFile();
        }
    }

    /// <summary>
    /// helper function for code re-use
    /// </summary>
    private void makeSaveFile()
    {
        Save.SaveDataJson(Savename);
    }
}

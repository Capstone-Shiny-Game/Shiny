using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMenu : MonoBehaviour
{
    private List<string> gameNames;
    string Savename;

    public void OnEnable()
    {
        gameNames = Save.GetSaveFileNames();
    }

    public void LoadGame()
    {
        Savename = "hi"; //TODO get save name from user from list picker
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

    private void makeSaveFile()
    {
        Save.SaveDataJson(Savename);
    }
}

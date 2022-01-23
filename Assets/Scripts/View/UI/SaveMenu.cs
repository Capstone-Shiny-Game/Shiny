using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{


    [SerializeField] private RectTransform saveTemplate;
    [SerializeField] private Transform listContent;
    [SerializeField] private GameObject savingOnlyOptions;
    public Button backButton;
    public int unsavedProgressSafeToLoseSeconds = 45;
    System.DateTime lastSaveTime = System.DateTime.MinValue;
    private List<string> gameNames;
    private string saveName;
    private bool savingIsEnabled;
    public ConfirmPopup confirmPopup;
    public TMPro.TMP_InputField saveNameInput;
    private List<Save.SaveDescriptorData> gamesDescriptors;

    public void OnEnable()
    {
        gameNames = Save.GetSaveFileNames();
        if (savingIsEnabled)
        {
            savingOnlyOptions.SetActive(true);
            PopulateSaveGameList("Overwrite");
        }
        else
        {
            savingOnlyOptions.SetActive(false);
            PopulateSaveGameList("Load");
        }
    }

    public void NewSaveGame()
    {
        if (saveNameInput.text is null || saveNameInput.text.Trim() == "")
        {
            return;
        }
        saveName = saveNameInput.text;
        SaveGame();
    }

    /// <summary>
    /// set if the save menu should be enabled or the load menu
    /// </summary>
    /// <param name="enable"></param>
    public void SetSavingisEnabled(bool enable)
    {
        savingIsEnabled = enable;
    }

    /// <summary>
    /// helper function for code re-use
    /// </summary>
    private void makeSaveFile()
    {
        Save.SaveDataJson(saveName);
        lastSaveTime = System.DateTime.Now;
        if (savingIsEnabled)//repopulate the list with new save
        {
            PopulateSaveGameList("Overwrite");
        }
        else
        {
            PopulateSaveGameList("Load");
        }
    }

    private void PopulateSaveGameList(string loadOrOverwrite = "Load")
    {
        ClearSaveList();
        gameNames = Save.GetSaveFileNames();
        gamesDescriptors = Save.GetSaveFileDescriptors();
        //Debug.Log(gamesDescriptors.Count);
        foreach (Save.SaveDescriptorData gameDescriptor in gamesDescriptors)
        {
            RectTransform newTemplate = Instantiate(saveTemplate, listContent, false);
            fillSaveTemplate(newTemplate, gameDescriptor, loadOrOverwrite);
        }
    }

    /// <summary>
    /// Clears out all instantiated List Objects
    /// </summary>
    private void ClearSaveList()
    {
        // destroy old ui elements to avoid duplicates
        foreach (Transform child in listContent)
        {
            if (child == saveTemplate)
            {
                // don't destroy the template
                continue;
            }
            Destroy(child.gameObject);
        }
    }

    private void fillSaveTemplate(RectTransform templateRectTransform, Save.SaveDescriptorData gameDescriptor, string loadOrOverwrite = "Load")
    {
        if (templateRectTransform is null)
        {
            Debug.Log("error : templateRectTransform is null in SaveMenu.cs");
            return;
        }
        templateRectTransform.gameObject.SetActive(true);
        //Debug.Log(gameDescriptor.SaveName);
        // find and set saveScreenshot image
        if (!(gameDescriptor.saveScreenshot is null))
        {
            Image image = templateRectTransform.Find("SaveImage").GetComponent<Image>();
            image.sprite = gameDescriptor.saveScreenshot;
        }
        Button deleteButton = templateRectTransform.Find("DeleteButton").GetComponent<Button>();
        deleteButton.onClick.AddListener(delegate { DeleteSaveHandler(gameDescriptor); });
        // find and set template button text and onclick
        Button loadButton = templateRectTransform.Find("LoadButton").GetComponent<Button>();
        loadButton.GetComponentInChildren<TextMeshProUGUI>().text = loadOrOverwrite;
        //private delegate void OverwriteOrLoadHandler(string gameSaveName);
        loadButton.onClick.AddListener(delegate { OverwriteOrLoadSaveHander(gameDescriptor); });
        // find and set template text values
        templateRectTransform.Find("SaveDate").GetComponent<TextMeshProUGUI>().text = gameDescriptor.timestamp.ToString("MM/dd/y h:mm tt");
        templateRectTransform.Find("SaveName").GetComponent<TextMeshProUGUI>().text = gameDescriptor.saveName;
        if (gameDescriptor.currentQuestName is null || gameDescriptor.currentQuestName == "")
        {
            templateRectTransform.Find("CurrentQuestName").GetComponent<TextMeshProUGUI>().text = "Shiny";
        }
        else
        {
            templateRectTransform.Find("CurrentQuestName").GetComponent<TextMeshProUGUI>().text = gameDescriptor.currentQuestName;
        }
    }
    public void DeleteSaveHandler(Save.SaveDescriptorData gameDescriptor)
    {
        saveName = gameDescriptor.saveName;
        if (saveName is null || saveName == "")
        {
            Debug.Log("error: saveName not found in Savemenu.cs");
            return;
        }
        if (gameNames.Contains(saveName))
        {
            confirmPopup.ShowPopUP("are you sure you want to delete this Save?\n\"" + saveName + "\"", confirmDeleteSave, "Delete");
            this.gameObject.SetActive(false);
        }

    }

    private void confirmDeleteSave(bool value)
    {
        this.gameObject.SetActive(true);//set save menu active
        if (value)
        {
            Save.DeleteSave(saveName);
        }

        if (savingIsEnabled)//repopulate the list with new save
        {
            PopulateSaveGameList("Overwrite");
        }
        else
        {
            PopulateSaveGameList("Load");
        }
    }

    public void OverwriteOrLoadSaveHander(Save.SaveDescriptorData gameDescriptor)
    {
        saveName = gameDescriptor.saveName;
        if (saveName is null || saveName == "")
        {
            Debug.Log("error: saveName not found in Savemenu.cs");
            return;
        }
        if (savingIsEnabled)
        {
            SaveGame();
        }
        else
        {
            LoadGame(gameDescriptor);
        }
    }
    private void LoadGame(Save.SaveDescriptorData gameDescriptor)
    {
        if (gameNames.Contains(saveName))
        {
            if (System.Math.Abs((lastSaveTime - System.DateTime.Now).TotalSeconds) > unsavedProgressSafeToLoseSeconds)
            {//if it's only been a short time since last save, dont ask to confirm loading the save
                confirmPopup.ShowPopUP("when loading this game, You will lose all unsaved progress", confirmLoadGame);
                this.gameObject.SetActive(false);
                return;
            }
            else
            {
                lastSaveTime = System.DateTime.Now;
                Save.LoadDataJson(saveName);
                backButton.onClick.Invoke();
            }
        }
        else
        {
            Debug.Log("save name not found in SaveMenu.cs");
        }
    }

    private void SaveGame()
    {
        if (gameNames.Contains(saveName))
        {
            confirmPopup.ShowPopUP("are you sure you want to overwrite this Save?\n\"" + saveName + "\"", confirmOverwriteSave);
            this.gameObject.SetActive(false);
            return;
        }
        gameNames.Add(saveName);
        makeSaveFile();
    }

    private void confirmOverwriteSave(bool value)
    {
        this.gameObject.SetActive(true);//set save menu active
        if (value)
        {
            makeSaveFile();
        }
    }
    private void confirmLoadGame(bool value)
    {
        this.gameObject.SetActive(true);//set save menu active
        if (value)
        {
            Save.LoadDataJson(saveName);
            lastSaveTime = System.DateTime.Now;
            backButton.onClick.Invoke();
        }
    }
}

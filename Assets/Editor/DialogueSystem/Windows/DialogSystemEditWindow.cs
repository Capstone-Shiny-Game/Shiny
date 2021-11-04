using UnityEditor;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DialogSystemEditWindow : EditorWindow
{
    private readonly string defaultFilename = "DialogueFilename";
    private TextField fileNameTextField;
    private Button saveButton;
    private DSGraphView graphView;

    [MenuItem("Window/Dialog Graph")]
    public static void Open()
    {
        GetWindow<DialogSystemEditWindow>();
    }

    private void OnEnable()
    {
        AddGraphView();
        AddToolbar();

        AddStyles();
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("Dialogue System/DSVariables.uss");
    }

    private void AddGraphView()
    {
        graphView = new DSGraphView(this);

        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }

    private void AddToolbar()
    {
        Toolbar toolbar = new Toolbar();

        fileNameTextField = DSElementUtilty.CreateTextField(defaultFilename, "File Name:", callback =>
        {
            fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
        });

        saveButton = DSElementUtilty.CreateButton("Save", () => Save());

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);

        toolbar.AddStyleSheets("Dialogue System/DSToolbarStyles.uss");

        rootVisualElement.Add(toolbar);
    }

    private void Save()
    {
        if (string.IsNullOrEmpty(fileNameTextField.value))
        {
            EditorUtility.DisplayDialog("Invalid File Name", "Please enter a file name", "Continue");
            return;
        }
        DSIOUtility.Initialize(fileNameTextField.value, graphView);
        DSIOUtility.Save();
    }

    public void EnableSave()
    {
        saveButton.SetEnabled(true);
    }

    public void DisableSave()
    {
        saveButton.SetEnabled(false);
    }
}
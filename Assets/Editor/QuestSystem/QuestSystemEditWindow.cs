using System.IO;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestSystemEditWindow : EditorWindow
{
    private readonly string defaultFilename = "QuestFilename";
    private static TextField fileNameTextField;
    private Button saveButton;
    private QSGraphView graphView;

    [MenuItem("Window/Quest Graph")]
    public static void Open()
    {
        QuestSystemEditWindow window = GetWindow<QuestSystemEditWindow>();
        window.titleContent = new GUIContent("Quest Graph Editor");
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
        graphView = new QSGraphView(this);

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

        Button clearButton = DSElementUtilty.CreateButton("Clear", () => Clear());
        Button resetButton = DSElementUtilty.CreateButton("Reset", () => ResetGraph());
        Button loadButton = DSElementUtilty.CreateButton("Load", () => Load());

        toolbar.Add(fileNameTextField);
        toolbar.Add(saveButton);
        toolbar.Add(loadButton);
        toolbar.Add(clearButton);
        toolbar.Add(resetButton);

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
        // TODO (Ella) : save
        //DSIOUtility.Initialize(fileNameTextField.value, graphView);
        //DSIOUtility.Save();
    }

    private void Load()
    {
        string filePath = EditorUtility.OpenFilePanel("Quest Graphs", "Assets/Editor/QuestSystem/Graphs", "asset");

        if (string.IsNullOrEmpty(filePath))
        {
            return;
        }

        Clear();

        // TODO (Ella) : load
        //DSIOUtility.Initialize(Path.GetFileNameWithoutExtension(filePath), graphView);
        //DSIOUtility.Load();
    }

    private void Clear()
    {
        graphView.ClearGraph();
    }

    private void ResetGraph()
    {
        graphView.ClearGraph();
        UpdateFilename(defaultFilename);
    }

    public static void UpdateFilename(string newFilename)
    {
        fileNameTextField.value = newFilename;
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

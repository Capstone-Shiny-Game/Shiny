using UnityEditor;
using System.IO;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class DialogSystemEditWindow : EditorWindow
{
    [MenuItem("Window/Dialog Graph")]
    public static void Open()
    {
        GetWindow<DialogSystemEditWindow>();
    }

    private void OnEnable()
    {
        AddGraphView();

        AddStyles();
    }

    private void AddStyles()
    {
        rootVisualElement.AddStyleSheets("Dialogue System/DSVariables.uss");
    }

    private void AddGraphView()
    {
        DSGraphView graphView = new DSGraphView(this);

        graphView.StretchToParentSize();

        rootVisualElement.Add(graphView);
    }
}
using UnityEditor;
using UnityEngine.UIElements;

public static class DSStyleUtility
{
    public static VisualElement AddClasses(this VisualElement element, params string[] classes)
    {
        foreach (var c in classes)
        {
            element.AddToClassList(c);
        }
        return element;
    }


    public static VisualElement AddStyleSheets(this VisualElement element, params string[] styleSheetNames)
    {
        foreach (string styleSheetName in styleSheetNames)
        {
            StyleSheet styleSheet = (StyleSheet)EditorGUIUtility.Load(styleSheetName);

            element.styleSheets.Add(styleSheet);
        }

        return element;
    }
}

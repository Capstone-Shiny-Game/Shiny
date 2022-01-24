using System;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class QSGroup : Group
{
    public string ID { get; set; }
    private Color defaultBorderColor;
    private float defaultBorderWidth;
    public string oldTitle { get; set; }

    public QSGroup(Vector2 position)
    {
        ID = Guid.NewGuid().ToString();
        title = $"Quest Group #{ID.Substring(ID.Length - 4)}";
        oldTitle = title;

        SetPosition(new Rect(position, Vector2.zero));

        defaultBorderColor = contentContainer.style.borderBottomColor.value;
        defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
    }

    public void SetErrorStyle(Color color)
    {
        contentContainer.style.borderBottomColor = color;
        contentContainer.style.borderBottomWidth = 2f;
    }

    public void ResetStyle()
    {
        contentContainer.style.borderBottomColor = defaultBorderColor;
        contentContainer.style.borderBottomWidth = defaultBorderWidth;
    }
}

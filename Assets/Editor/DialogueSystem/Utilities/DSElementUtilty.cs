using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public static class DSElementUtilty
{
    public static Foldout CreateFoldout(string title, bool collapsed = false)
    {
        Foldout foldout = new Foldout()
        {
            text = title,
            value = !collapsed
        };

        return foldout;
    }

    public static Button CreateButton(string text, Action onClick = null)
    {
        Button button = new Button(onClick)
        {
            text = text
        };

        return button;
    }

    public static Port CreatePort(this DSNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
    {
        Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));
        port.portName = portName;

        return port;
    }

    public static Port CreatePort(this QSNode node, string name, Type type, Direction direction, Port.Capacity capacity)
    {
        Port port = node.InstantiatePort(Orientation.Horizontal, direction, capacity, type);
        port.portName = name;

        if (type == typeof(QSUnlock))
            port.portColor = new Color(253f / 255, 151f / 255, 31f / 255); // orange
        else if (type == typeof(QSLink))
            port.portColor = new Color(171f / 255, 157f / 255, 242f / 255); // purple
        else if (type == typeof(ItemSO))
            port.portColor = new Color(166f / 256, 226f / 256, 46f / 256); // green
        else if (type == typeof(QSNPC))
            port.portColor = new Color(102f / 255, 217f / 255, 239f / 255); // blue

        return port;
    }

    public static Toggle CreateCheckBox(bool value = false, string label = null, EventCallback<ChangeEvent<bool>> onValueChanged = null)
    {
        Toggle toggle = new Toggle()
        {
            value = value,
            label = label,
        };
        if (onValueChanged != null)
            toggle.RegisterValueChangedCallback(onValueChanged);
        return toggle;
    }

    public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
    {
        TextField textField = new TextField()
        {
            value = value,
            label = label
        };
        if (onValueChanged != null)
        {
            textField.RegisterValueChangedCallback(onValueChanged);
        }
        return textField;
    }

    public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
    {
        TextField textArea = CreateTextField(value, label, onValueChanged);

        textArea.multiline = true;

        return textArea;
    }
}

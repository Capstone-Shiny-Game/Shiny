using System.Collections;
using UnityEngine;
using System.ComponentModel;

public static class PatioUtility
{
    public enum Furniture
    {
        [Description("Bird Bath")]
        BIRDBATH
    }

    public enum Hat
    {
        [Description("Witch's Hat")]
        WITCH,
        [Description("Top Hat")]
        FANCY,
        [Description("Straw Hat")]
        STRAW,
        [Description("EAE Cap")]
        EAE
    }
    public static string GetPrettyName(System.Enum e)
    {
        var nm = e.ToString();
        var tp = e.GetType();
        var field = tp.GetField(nm);
        var attrib = System.Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

        if (attrib != null)
            return attrib.Description;
        else
            return nm;
    }
}

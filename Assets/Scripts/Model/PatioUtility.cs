using System.Collections;
using UnityEngine;
using System.ComponentModel;

public static class PatioUtility
{
    public enum Furniture
    {
        [Description("Bird Bath")]
        BIRDBATH,
        [Description("Umbrella")]
        UMBRELLA,
        [Description("Bench")]
        BENCH,
        [Description("Chair")]
        CHAIR

    }
    public enum Toys
    {
        [Description("Toy Blocks")]
        BLOCKS,
        [Description("Toy Bucket")]
        BUCKET,
        [Description("Bird Bath")]
        FOUNTAIN
    }
    public enum Hat
    {
        [Description("Witch's Hat")]
        WITCH,
        [Description("Top Hat")]
        FANCY,
        [Description("Straw Hat")]
        STRAW,
        [Description("Gamer Cap")]
        EAE,
        [Description("Cowboy Hat")]
        COWBOY,
        [Description("No Hat")]
        NONE
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

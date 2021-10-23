using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public Sprite item;
    public Sprite item1;
    public Sprite item2;
    public Sprite item3;
    public Sprite item4;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Debug.Log("hi");
        Instance = this;
    }

    public Transform pfItemPickUpWorld;

    public Sprite potionSprite;//item sprites listed here
    public Sprite foodSprite;
    public Sprite shinySprite;
}

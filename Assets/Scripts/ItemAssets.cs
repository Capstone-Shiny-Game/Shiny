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

    public Transform pfItemPickUpWorld;

    public Sprite potion;//item sprites listed here
    public Sprite food;
    public Sprite shiny;
    public Sprite item3;
    public Sprite item4;
}

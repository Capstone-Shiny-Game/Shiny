using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Savable 
{
    public static List<Savable> savables;

    public Savable() {
        savables.Add(this);
    }

    public abstract void GetSaveData();

    public abstract void LoadData();
}

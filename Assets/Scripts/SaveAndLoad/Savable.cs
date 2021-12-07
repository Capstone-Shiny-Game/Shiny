using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Savable 
{
    public static List<Savable> savables;

    public Savable() {
        savables.Add(this);
    }//requires testing

    public abstract void GetSaveData(Save.SaveData saveData);

    public abstract void LoadData(Save.SaveData saveData);
}

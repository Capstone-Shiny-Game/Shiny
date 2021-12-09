using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public SerializableDictionary<GameObject, Vector3> questItemsAndPositions;
    public DSDialogueContainerSO dialogueContainer;

    private BoxCollider interactionCollider;
    private List<GameObject> spawnedItems;


    // Start is called before the first frame update
    void Start()
    {
        interactionCollider = GetComponentInChildren<BoxCollider>();
        spawnedItems = new List<GameObject>();


        StartQuest();
    }

    void FixedUpdate()
    {
        if (interactionCollider.bounds.Contains(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position)) // oof
        {
            if (CheckQuestItems())
            {
                Debug.Log("Quest complete");
                //DO THING
            }
        }
    }

    private void StartQuest()
    {
        SpawnQuestItems();
    }

    private void SpawnQuestItems()
    {
        foreach (KeyValuePair<GameObject, Vector3> item in questItemsAndPositions)
        {
            spawnedItems.Add(ItemWorld.SpawnItemWorld(item.Key, item.Value).item.prefab);
        }
    }

    private bool CheckQuestItems()
    {
        foreach (GameObject item in spawnedItems)
        {
            if (!item.active || !interactionCollider.bounds.Contains(item.transform.position))
            {
                return false;
            }
        }
        return true;
    }
}

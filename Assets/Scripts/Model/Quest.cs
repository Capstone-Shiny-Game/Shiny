using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    public SerializableDictionary<GameObject, Vector3> questItemsAndPositions;
    public DSDialogueContainerSO dialogueContainer;

    public GameObject interactButton;
    private BoxCollider interactionCollider;
    private List<GameObject> spawnedItems;


    // Start is called before the first frame update
    void Awake()
    {
        interactionCollider = GetComponentInChildren<BoxCollider>();
        spawnedItems = new List<GameObject>();
        //interactButton = GetComponentInChildren<BillboardFX>();
    }

    void FixedUpdate()
    {
        if (spawnedItems.Count <= 0)
        {
            return;
        }

        if (interactionCollider.bounds.Contains(GameObject.FindGameObjectWithTag("Player").gameObject.transform.position)) // oof
        {
            if (CheckQuestItems())
            {
                foreach (GameObject item in spawnedItems)
                {
                    Destroy(item);
                }
                spawnedItems.Clear();
                interactButton.SetActive(true);
                GetComponent<NPCInteraction>().dialogueContainer = dialogueContainer;
                Destroy(this);
            }
        }
    }

    public void StartQuest()
    {

        interactButton.SetActive(false);
        SpawnQuestItems();
    }

    public void resetList(GameObject item, Vector3 position)
    {
        questItemsAndPositions.Clear();
        questItemsAndPositions.Add(item, position);
    }

    private void SpawnQuestItems()
    {
        foreach (KeyValuePair<GameObject, Vector3> item in questItemsAndPositions)
        {
            //spawnedItems.Add(ItemWorld.SpawnItemWorld(item.Key, item.Value).item.GetPrefab());//TODO Please change this to the new spawn item world
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

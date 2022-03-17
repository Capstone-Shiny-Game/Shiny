using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempQuestComplete : MonoBehaviour
{
    public ParticleSystem particles;

    void Start()
    {
        FetchQuest.OnQuestCompleteEvent += particles.Play;
    }
}

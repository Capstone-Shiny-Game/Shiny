using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioAnim : MonoBehaviour
{
    public void WingFlap()
    {
        AkSoundEngine.PostEvent("wingFlap", gameObject);
    }

    public void CrowStep()
    {
        AkSoundEngine.PostEvent("crowStep", gameObject);
    }

    public void HumanStep()
    {
        AkSoundEngine.PostEvent("footstepGrass", gameObject);
    }
}

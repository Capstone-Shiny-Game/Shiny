using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    // by adding this, it will show up in the inspector
    [SerializeField] Color MyColor = new Color();

    void Start()
    {
        MaterialPropertyBlock myMatBlock = new MaterialPropertyBlock();
        myMatBlock.SetColor("_Tint", MyColor);

        // Get our Renderer here...
        MeshRenderer myMeshRenderer = GetComponent<MeshRenderer>();
        myMeshRenderer.SetPropertyBlock(myMatBlock);
    }
}


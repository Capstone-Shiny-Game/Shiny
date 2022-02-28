using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
[ExecuteInEditMode]
public class RenderTerrainMap : MonoBehaviour
{
 
    public Camera camToDrawWith;
    [SerializeField]
    LayerMask layer;
    // objects to render
    [SerializeField]
    Renderer[] renderers;
    public int resolution = 512;
    [SerializeField]
    bool RealTimeDiffuse;
    RenderTexture tempTex;
 
    private Bounds bounds;
    // resolution of the map
 
    // Start is called before the first frame update
 
    void GetBounds()
    {
        bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }
    }
 
    void OnEnable()
    {
        tempTex = new RenderTexture(resolution, resolution, 24);
        GetBounds();
        SetUpCam();
        DrawDiffuseMap();
    }
 
 
    void Start()
    {
        GetBounds();
        SetUpCam();
        DrawDiffuseMap();
    }
 
    void OnRenderObject()
    {
        if (!RealTimeDiffuse)
        {
            return;
        }
        UpdateTex();
    }
 
    void UpdateTex()
    {
        camToDrawWith.enabled = true;
        camToDrawWith.targetTexture = tempTex;
        Shader.SetGlobalTexture("_TerrainDiffuse", tempTex);
    }
    public void DrawDiffuseMap()
    {
        DrawToMap("_TerrainDiffuse");
    }
 
    void DrawToMap(string target)
    {
        camToDrawWith.enabled = true;
        camToDrawWith.targetTexture = tempTex;
        camToDrawWith.depthTextureMode = DepthTextureMode.Depth;
        Shader.SetGlobalFloat("_OrthographicCamSize", camToDrawWith.orthographicSize);
        Shader.SetGlobalVector("_OrthographicCamPos", camToDrawWith.transform.position);
        camToDrawWith.Render();
        Shader.SetGlobalTexture(target, tempTex);
        camToDrawWith.enabled = false;
    }
 
    void SetUpCam()
    {
        if (camToDrawWith == null)
        {
            camToDrawWith = GetComponentInChildren<Camera>();
        }
        float size = bounds.size.magnitude;
        camToDrawWith.cullingMask = layer;
        camToDrawWith.orthographicSize = size / 3;
        camToDrawWith.transform.parent = null;
        camToDrawWith.transform.position = bounds.center + new Vector3(0, bounds.extents.y + 5f, 0);
        camToDrawWith.transform.parent = gameObject.transform;
    }
 
}

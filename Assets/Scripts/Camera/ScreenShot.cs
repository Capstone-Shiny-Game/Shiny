using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour, SaveDescriptor
{
    public int resWidth, resHeight;
    public Image outputImage;
    bool isCapturing = false;
    Texture2D currentCapture;

    IEnumerator CaptureRoutine()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            isCapturing = true;
            currentCapture = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
            currentCapture.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0, false);
            currentCapture.Apply();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Screen capture failed in ScreenShot.cs");
            Debug.LogError(e.ToString());
            isCapturing = false;
        }
    }

    void LateUpdate()
    {
        if (isCapturing && currentCapture != null)
        {
            Sprite sprite = Sprite.Create(currentCapture, new Rect(0, 0, Screen.width, Screen.height), new Vector2(0, 0));
            if (outputImage)
                outputImage.sprite = sprite;
            isCapturing = false;
        }
    }

    public void ScreenshotToImage()
    {
        StartCoroutine(CaptureRoutine());
    }

    public void AddSelfToSaveDescriptorsList()
    {
        Save.AddSelfToSaveDescriptorsList(this);
    }

    public void GetSaveDescriptorData(ref Save.SaveDescriptorData saveData)
    {
        outputImage = null;
        ScreenshotToImage();
        int i = 0;
        while (outputImage is null && i < 10000) {
            i++;
        }
        Debug.Log("screenshot.cs GetSaveDescriptorData looped " + i + " times");
        if (outputImage)
        {
            saveData.saveScreenshot = outputImage.sprite;
        }
    }
}

using System;
using System.Collections;
using System.IO;
using UnityEngine;
using TMPro;

public class Photo : MonoBehaviour
{
    private WebCamTexture webCamTexture;
    private string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "sei_babbo.png");

    void Start()
    {
        webCamTexture = new WebCamTexture();
        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
            renderer = gameObject.AddComponent<SpriteRenderer>();
        renderer.material.mainTexture = webCamTexture;
        StartCoroutine(WaitAndShootPhoto());
    }

    public Texture2D CaptureImage()
    {
        Texture2D capturedImage = new Texture2D(webCamTexture.width, webCamTexture.height);
        capturedImage.SetPixels(webCamTexture.GetPixels());
        capturedImage.Apply();
        webCamTexture.Stop();
        return capturedImage;
    }

    public void SaveImage(Texture2D image, string filePath)
    {
        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(filePath, bytes);
    }

    public void ShootPhoto()
    {
        Debug.Log("Shooting photo...");
        Texture2D capturedImage = CaptureImage();
        SaveImage(capturedImage, filePath);
        WallpaperSetter.SetWallpaper(filePath);
    }

    IEnumerator WaitAndShootPhoto()
    {
        yield return new WaitForSeconds(8);
        webCamTexture.Play();
        yield return new WaitForSeconds(2);
        ShootPhoto();
    }
}

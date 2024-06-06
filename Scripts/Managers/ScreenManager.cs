using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ResItem
{
    public int width, height;
}

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager _instance;

    public List<ResItem> resolutions = new List<ResItem>
    {
        new ResItem { width = 640, height = 360 },
        new ResItem { width = 1024, height = 576 },
        new ResItem { width = 1280, height = 720 },
        new ResItem { width = 1600, height = 900 },
        new ResItem { width = 1920, height = 1080 },
        new ResItem { width = 2560, height = 1440 },
        new ResItem { width = 3840, height = 2160 }
    };

    public static ScreenManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ScreenManager>();
                if (_instance == null)
                    Debug.LogError("No ScreenManager found in the scene.");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SetResolution(int resIndex, bool fullscreen, bool vsync)
    {
        Screen.fullScreen = fullscreen;
        QualitySettings.vSyncCount = vsync ? 1 : 0;
        ResItem res = resolutions[resIndex];
        Screen.SetResolution(res.width, res.height, fullscreen);
    }
}

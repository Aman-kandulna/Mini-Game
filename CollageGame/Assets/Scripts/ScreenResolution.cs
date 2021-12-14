    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenResolution : MonoBehaviour
{
    List<int> widths = new List<int>() { 1280, 1366, 1600, 1920 };
    List<int> heights = new List<int>() { 720, 768, 900, 1080 };
    public void SetRes(int index)
    {
        bool fullscreen = Screen.fullScreen;
        int width = widths[index];
        int height = heights[index];
        Screen.SetResolution(width, height, fullscreen);

    }
    public void SetFullscreen(bool _fullscreen)
    {
        Screen.fullScreen = _fullscreen;
    }
}

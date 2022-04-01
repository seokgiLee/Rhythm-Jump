using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSManager : MonoBehaviour
{
    public int size;
    public float r, g, b;
    float deltaTime = 0;

    void Awake()
    {
        size = size == 0 ? 50 : size;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;
        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / size;
        style.normal.textColor = new Color(r, g, b, 1.0f);

        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0}ms({1:0.}fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
}

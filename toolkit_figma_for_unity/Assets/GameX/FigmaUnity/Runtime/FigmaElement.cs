using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

[System.Serializable]
public class FigmaElement : MonoBehaviour
{
    public string id;
    public string name;
    public string type; // frame, rectangle, text, etc.
    public float x;
    public float y;
    public float width;
    public float height;
    public Color color;
    public string text;
    public int fontSize;
    public string fontFamily;
    public List<FigmaElement> children;
    public CornerRadius cornerRadius;
    public Border border;
    public Effects effects;
}

[System.Serializable]
public class CornerRadius
{
    public float topLeft;
    public float topRight;
    public float bottomLeft;
    public float bottomRight;
}

[System.Serializable]
public class Border
{
    public float width;
    public Color color;
}

[System.Serializable]
public class Effects
{
    public Shadow shadow;
}

[System.Serializable]
public class Shadow
{
    public float x;
    public float y;
    public float blur;
    public Color color;
}


[System.Serializable]
public class PixsoResponseWrapper
{
    public List<FigmaElement> nodes;
}
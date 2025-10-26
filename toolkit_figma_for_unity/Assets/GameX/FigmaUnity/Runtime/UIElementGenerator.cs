// UIElementGenerator.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;
using System;

public class UIElementGenerator
{
    private Transform parentCanvas;
    private Dictionary<string, GameObject> generatedObjects = new Dictionary<string, GameObject>();

/// <summary>
/// UI元素生成器的构造函数
/// </summary>
/// <param name="canvas">传入的Canvas对象，用于获取其transform属性</param>
    public UIElementGenerator(Canvas canvas)
    {
    // 将传入的Canvas对象的transform属性赋值给父画布变量
        parentCanvas = canvas.transform;
    }


    /// <summary>
    /// 根据Figma设计数据生成UI界面
    /// </summary>
    /// <param name="elements">从Figma导出的UI元素列表</param>
    public void GenerateUIFromFigmaData(List<FigmaElement> elements)
    {
        // 遍历所有UI元素
        foreach (var element in elements)
        {
            // 为每个元素创建对应的UI组件，并添加到父画布中
            CreateUIElement(element, parentCanvas);
        }

        // 重新排序层级
        ReorderHierarchy();
    }
    
    public GameObject GenerateUIFromFigmaData(FigmaElement element)
    {
        // 为每个元素创建对应的UI组件，并添加到父画布中
        GameObject uiObject = CreateUIElement(element, parentCanvas);
        // 重新排序层级
        ReorderHierarchy();
        // 返回生成的UI对象
        return uiObject;
    }
    
    private GameObject CreateUIElement(FigmaElement element, Transform parent)
    {
        GameObject uiObject = new GameObject(element.name);
        uiObject.transform.SetParent(parent, false);
        
        // 添加RectTransform
        RectTransform rectTransform = uiObject.AddComponent<RectTransform>();
        SetupRectTransform(rectTransform, element);
        
        // 根据类型创建对应的UI组件
        switch (element.type.ToLower())
        {
            case "frame":
            case "rectangle":
                CreateImageComponent(uiObject, element);
                break;
                
            case "text":
                CreateTextComponent(uiObject, element);
                break;
                
            case "instance":
            case "component":
                CreateButtonComponent(uiObject, element);
                break;
        }
        
        // 递归创建子元素
        if (element.children != null && element.children.Count > 0)
        {
            foreach (var child in element.children)
            {
                CreateUIElement(child, uiObject.transform);
            }
        }
        
        generatedObjects[element.id] = uiObject;
        return uiObject;
    }
    
    private void SetupRectTransform(RectTransform rectTransform, FigmaElement element)
    {
        // 设置位置和大小
        rectTransform.anchoredPosition = new Vector2(element.x, -element.y); // Pixso Y轴向下，Unity向上
        rectTransform.sizeDelta = new Vector2(element.width, element.height);
        
        // 设置锚点为左上角（匹配Pixso的坐标系）
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(0, 1);
        rectTransform.pivot = new Vector2(0, 1);
    }
    
    private void CreateImageComponent(GameObject obj, FigmaElement element)
    {
        Image image = obj.AddComponent<Image>();
        
        // 处理颜色
        if (element.color != null)
        {
            image.color = element.color;
        }
        
        // 处理圆角
        if (element.cornerRadius != null && 
            (element.cornerRadius.topLeft > 0 || element.cornerRadius.topRight > 0 ||
             element.cornerRadius.bottomLeft > 0 || element.cornerRadius.bottomRight > 0))
        {
            // 使用MaskableGraphic和Shader实现圆角，或者使用九宫格
            SetupRoundedCorners(image, element.cornerRadius);
        }
        
        // 处理边框
        if (element.border != null && element.border.width > 0)
        {
            CreateBorder(obj, element.border);
        }
        
        // 处理阴影
        if (element.effects?.shadow != null)
        {
            CreateShadowEffect(obj, element.effects.shadow);
        }
    }


    private void CreateTextComponent(GameObject obj, FigmaElement element)
    {
        Text text = obj.AddComponent<Text>();
        text.text = element.text ?? element.name;
        
        if (element.color != null)
        {
            text.color = element.color;
        }
        
        if (element.fontSize > 0)
        {
            text.fontSize = Mathf.RoundToInt(element.fontSize);
        }
        
        // 字体映射
        if (!string.IsNullOrEmpty(element.fontFamily))
        {
            text.font = GetMappedFont(element.fontFamily);
        }
        
        // 文本对齐（根据Pixso数据设置）
        text.alignment = TextAnchor.MiddleCenter; // 根据实际情况调整
    }
    
    private void CreateButtonComponent(GameObject obj, FigmaElement element)
    {
        // 先创建图像背景
        CreateImageComponent(obj, element);
        
        // 添加Button组件
        Button button = obj.AddComponent<Button>();
        
        // 查找并设置文本子对象
        Text childText = obj.GetComponentInChildren<Text>();
        if (childText != null)
        {
            // 设置按钮的颜色过渡
            ColorBlock colors = button.colors;
            colors.normalColor = element.color;
            colors.highlightedColor = element.color * 0.8f;
            colors.pressedColor = element.color * 0.6f;
            button.colors = colors;
        }
    }
    
    private Font GetMappedFont(string fontFamily)
    {
        Dictionary<string, Font> fontMapping = new Dictionary<string, Font>();

        if (fontMapping.ContainsKey(fontFamily))
        {
            return fontMapping[fontFamily];
        }
        
        // 默认字体
        return Resources.GetBuiltinResource<Font>("Arial.ttf");
    }

    private void ReorderHierarchy()
    {
        // 根据Pixso中的层级顺序重新排序Unity中的GameObject
        // 这需要维护Pixso元素的层级信息
    }
    

    private void CreateShadowEffect(GameObject obj, Shadow shadow)
    {
        throw new NotImplementedException();
    }

    private void SetupRoundedCorners(Image image, CornerRadius cornerRadius)
    {
        throw new NotImplementedException();
    }

    private void CreateBorder(GameObject obj, Border border)
    {
        throw new NotImplementedException();
    }
}

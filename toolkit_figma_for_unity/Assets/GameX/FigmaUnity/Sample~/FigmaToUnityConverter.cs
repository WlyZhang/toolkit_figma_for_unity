using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;


public class FigmaToUnityConverter : MonoBehaviour
{
    [Header("Figma 设置")]
    public TextAsset figmaConfig;

    [Header("Unity 设置")]
    public Canvas targetCanvas;
    public bool autoGenerateOnStart = false;

    private Dictionary<string, Font> fontMapping = new Dictionary<string, Font>();

    void Start()
    {
        if (autoGenerateOnStart && figmaConfig != null)
        {
            StartConversion();
        }
    }

    public async void StartConversion()
    {
        // 1. 从Figma平台获取 UI/UE 原型设计Json数据
        var figmaData = await FetchFigmaData<FigmaElement>(figmaConfig.text);
        // 2. 解析并生成UI
        GenerateUnityUI(figmaData);
    }

    /// <summary>
    /// 异步解析 Figma 数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="figmaJson"></param>
    /// <returns></returns>
    private async Task<T> FetchFigmaData<T>(string figmaJson)
    {
        while (figmaConfig == null)
        {
            Debug.LogError("Figma 配置文件未设置！");
            await Task.Yield();
        }

        return JsonConvert.DeserializeObject<T>(figmaJson);
    }

    /// <summary>
    /// 生成 Unity UI
    /// </summary>
    /// <param name="figmaData"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void GenerateUnityUI(FigmaElement figmaData)
    {
        //TODO: 实现 UI 生成逻辑
        Debug.Log("开始生成 Unity UI 元素...");
        UIElementGenerator uiGenerator = new UIElementGenerator(targetCanvas);
        uiGenerator.GenerateUIFromFigmaData(new List<FigmaElement> { figmaData });
    }
}
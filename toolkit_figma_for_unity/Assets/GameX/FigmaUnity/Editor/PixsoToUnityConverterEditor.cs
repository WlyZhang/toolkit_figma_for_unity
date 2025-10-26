#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UIElementGenerator))]
public class PixsoToUnityConverterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        UIElementGenerator converter = (UIElementGenerator)target();
        
        GUILayout.Space(10);
        
        if (GUILayout.Button("从Figma资源生成UI"))
        {
            converter.StartConversion();
        }
        
        if (GUILayout.Button("清除生成的UI"))
        {
            ClearGeneratedUI(converter);
        }
        
        GUILayout.Space(5);
        
        EditorGUILayout.HelpBox(
            "1. 在Pixso中获取分享链接\n" +
            "2. 粘贴到上面的输入框\n" +
            "3. 点击生成按钮", 
            MessageType.Info);
    }
    
    private void ClearGeneratedUI(PixsoToUnityConverter converter)
    {
        // 清除之前生成的UI元素
        Transform canvas = converter.targetCanvas.transform;
        for (int i = canvas.childCount - 1; i >= 0; i--)
        {
            if (canvas.GetChild(i).name.StartsWith("Pixso_"))
            {
                DestroyImmediate(canvas.GetChild(i).gameObject);
            }
        }
    }
}
#endif

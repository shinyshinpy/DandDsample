using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// Editor用拡張
/// </summary>
public class Extension : MonoBehaviour
{
    /// <summary>
    /// タグ検索用のバッファ
    /// </summary>
    /// <returns></returns>
    private static List<string> _tagList = new();

    /// <summary>
    /// タグを設定します。存在しない場合はタグを登録して設定します。<br/>
    /// タグ登録はEditor専用。タグ設定がめんどくさかったから作った
    /// </summary>
    /// <param name="target"></param>
    /// <param name="tagName"></param>
    public static void SetTag(GameObject target, string tagName)
    {
        if(_tagList.Contains(tagName) == false)
        {
            AddTag(tagName);
        }
        target.tag = tagName;
    }

    /// <summary>
    /// タグを登録します
    /// </summary>
    /// <param name="tagname"></param>
    private static void AddTag(string tagname)
    {
        #if UNITY_EDITOR
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            for (int i = 0; i < tags.arraySize; ++i)
            {
                if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                {
                    return;
                }
            }

            int index = tags.arraySize;
            tags.InsertArrayElementAtIndex(index);
            tags.GetArrayElementAtIndex(index).stringValue = tagname;
            _tagList.Add(tagname);
            so.ApplyModifiedProperties();
            so.Update();
        }
        #endif
    }

    private void Awake()
    {
        #if UNITY_EDITOR
        UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
        if ((asset != null) && (asset.Length > 0))
        {
            SerializedObject so = new SerializedObject(asset[0]);
            SerializedProperty tags = so.FindProperty("tags");

            foreach (var index in Enumerable.Range(0, tags.arraySize))
            {
                _tagList.Add(tags.GetArrayElementAtIndex(index).stringValue);
            }
        }
        #endif
    }
}
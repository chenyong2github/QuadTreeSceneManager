using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneNodeEditor : Editor
{
    [MenuItem("Tools/SceneNode/Save data", false, 0)]
    public static void SaveData()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        CheckChildrenNameUnique();

        SceneNodeData snd = go.GetComponent<SceneNodeData>();
        if (snd == null)
        {
            snd = go.AddComponent<SceneNodeData>();
        }

        snd.Save(go.transform);
        EditorUtility.SetDirty(go);
    }

   [MenuItem("Tools/SceneNode/Batch generate prefab", false, 0)]
    public static void BatchGenPrefab()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        CheckChildrenNameUnique();

        if (!Directory.Exists("Assets/Resources/st_jhs_01/" + go.name))
        {
            Directory.CreateDirectory("Assets/Resources/st_jhs_01/" + go.name);
        }

        Transform tParent = go.transform;
        foreach (Transform t in tParent)
        {
            Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/Resources/st_jhs_01/" + tParent.name +"/"+ t.name + ".prefab");
            tempPrefab = PrefabUtility.ReplacePrefab(t.gameObject, tempPrefab);
        }
    }

    public static void CheckChildrenNameUnique()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        Dictionary<string, int> nameNums = new Dictionary<string, int>();

        Transform tParent = go.transform;


        while (true)
        {
            bool duplicate = false;
            nameNums.Clear();
            foreach (Transform t in tParent)
            {
                if (nameNums.ContainsKey(t.name))
                {
                    duplicate = true;
                    t.name += "_dupl";
                }

                nameNums[t.name] = 1;

                if (duplicate)
                    break;
            }

            if (!duplicate)
                break;
        }
    }
}

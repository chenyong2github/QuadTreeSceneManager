using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneNodeEditor : Editor
{
    [MenuItem("Tools/SceneNode/ArrangeObj", false, 0)]
    public static void ArrangeObj()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        CheckChildrenNameUnique();

        GameObject root = new GameObject();
        root.name = go.name + "_QuadTree";

        GameObject tree = new GameObject();
        tree.transform.parent = root.transform;
        tree.name = "QuadTreeInfo";
        SceneNodeData snd = tree.AddComponent<SceneNodeData>();
        snd.depth = 2;
        snd.Save(go.transform);

        GameObject objs = new GameObject();
        objs.transform.parent = root.transform;
        objs.name = "Objs";
        ArrangeOneObj(snd.sceneTree, objs.transform);
    }

    private static void ArrangeOneObj(QuadTree<SceneNode> treeInfo, Transform root)
    {
        if (!string.IsNullOrEmpty(treeInfo.prefabName))
        {
            GameObject newGo = new GameObject();
            newGo.name = treeInfo.prefabName;
            newGo.transform.parent = root;

            for (int i = 0; i < treeInfo.storedObjects.Count; i++)
            {
                Transform stored = Selection.activeTransform.FindChild(treeInfo.storedObjects[i].name);
                if (stored!= null)
                {
                    stored.parent = newGo.transform;
                }
                else
                {
                    Debug.LogErrorFormat("{0} can't find !!", treeInfo.storedObjects[i].name);
                }
            }
        }

        if (treeInfo.cells != null)
        {
            for (int i = 0; i < 4; i++)
            {
                ArrangeOneObj(treeInfo.cells[i], root);
            }
        }
    }

    [MenuItem("Tools/SceneNode/Batch generate prefab", false, 0)]
    public static void BatchGenPrefab()
    {
        GameObject go = Selection.activeGameObject;
        if (go == null) return;

        string szPath = "Assets/Resources/QuadTree";
        CheckPath(szPath);

        //树信息
        Transform tree = go.transform.FindChild("QuadTreeInfo");
        Object treePrefab = PrefabUtility.CreateEmptyPrefab(szPath + "/"+ go.name + ".prefab");
        treePrefab = PrefabUtility.ReplacePrefab(tree.gameObject, treePrefab);

        //物件信息
        szPath = szPath + "/" + go.name;
        CheckPath(szPath);

        Transform objs = go.transform.FindChild("Objs");
        foreach (Transform t in objs)
        {
            PrefabLightmapData pld = t.gameObject.AddComponent<PrefabLightmapData>();
            pld.SaveLightmap();
            
            Object tempPrefab = PrefabUtility.CreateEmptyPrefab(szPath + "/" + t.name + ".prefab");
            tempPrefab = PrefabUtility.ReplacePrefab(t.gameObject, tempPrefab);
        }
    }

    static void CheckPath(string szPath)
    {
        if (!Directory.Exists(szPath))
        {
            Directory.CreateDirectory(szPath);
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

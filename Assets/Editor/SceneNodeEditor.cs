using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class SceneNodeEditor : EditorWindow
{
    static int mapSize = 256;
    static int mapDensity = 1;
    static int mapMinWidth = 5;
    static int nodeDepth = 2;
    static LayerMask layerMask;

    [MenuItem("Tools/SceneNodeEditor")]
    static void Init()
    {
        SceneNodeEditor window = EditorWindow.GetWindow<SceneNodeEditor>("SceneNodeEditor");
        window.minSize = new Vector2(40, window.minSize.y);
        window.Show();
    }

    void OnEnable()
    {
        layerMask.value = EditorPrefs.GetInt("SceneNodeEditor_layerMask", 0);
    }

    void OnLostFocus()
    {
        EditorPrefs.SetInt("SceneNodeEditor_layerMask", layerMask.value);
    }

    void OnGUI()
    {
        mapSize = EditorGUILayout.IntField("Map Size", mapSize);
        mapDensity = EditorGUILayout.IntField("Map Density", mapDensity);
        mapMinWidth = EditorGUILayout.IntField("Map MinWidth", mapMinWidth);
        EditorGUILayout.LabelField("-----Others-----");
        nodeDepth = EditorGUILayout.IntField("Node Depth", nodeDepth);
        layerMask = LayerMaskField("Ignore Layer", layerMask);

        if (GUILayout.Button("Arrange"))
            ArrangeObj();

        if (GUILayout.Button("Batch"))
            BatchGenPrefab();

        if (GUILayout.Button("SaveLightmapData"))
            SaveLightmapData(Selection.activeGameObject);

        if (GUILayout.Button("UseLightmapData"))
            UseLightmapData(Selection.activeGameObject);
    }

    static LayerMask LayerMaskField(string label, LayerMask layerMask)
    {
        List<string> layers = new List<string>();
        List<int> layerNumbers = new List<int>();

        for (int i = 0; i < 32; i++)
        {
            string layerName = LayerMask.LayerToName(i);
            if (layerName != "")
            {
                layers.Add(layerName);
                layerNumbers.Add(i);
            }
        }
        int maskWithoutEmpty = 0;
        for (int i = 0; i < layerNumbers.Count; i++)
        {
            if (((1 << layerNumbers[i]) & layerMask.value) > 0)
                maskWithoutEmpty |= (1 << i);
        }
        maskWithoutEmpty = EditorGUILayout.MaskField(label, maskWithoutEmpty, layers.ToArray());
        int mask = 0;
        for (int i = 0; i < layerNumbers.Count; i++)
        {
            if ((maskWithoutEmpty & (1 << i)) > 0)
                mask |= (1 << layerNumbers[i]);
        }
        layerMask.value = mask;
        return layerMask;
    }

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
        snd.mapSize = mapSize;
        snd.mapDensity = mapDensity;
        snd.mapMinWidth = mapMinWidth;
        snd.nodeDepth = nodeDepth;
        snd.layerMask = layerMask.value;
        snd.Save(go.transform);

        GameObject objs = new GameObject();
        objs.transform.parent = root.transform;
        objs.name = "Objs";
        ArrangeOneObj(snd.sceneTree, objs.transform);

        Selection.activeGameObject = root;
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

    static void SaveLightmapData(GameObject obj)
    {
        PrefabLightmapData pld = obj.GetComponent<PrefabLightmapData>();
        if (pld == null)
        {
            pld = obj.AddComponent<PrefabLightmapData>();
        }

        pld.SaveLightmap();
    }

    static void UseLightmapData(GameObject obj)
    {
        PrefabLightmapData pld = obj.GetComponent<PrefabLightmapData>();
        if (pld != null)
        {
            pld.LoadLightmap();
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

        CheckChildrenNameUnique(nameNums, go.transform, 0);
    }

    static void CheckChildrenNameUnique(Dictionary<string, int> nameNums, Transform tParent, int deapth)
    {
        deapth++;

        foreach (Transform t in tParent.transform)
        {
            if (deapth >= nodeDepth)
            {
                while (nameNums.ContainsKey(t.name))
                {
                    t.name += "_dupl";
                }

                nameNums[t.name] = 1;
            }
            else
            {
                CheckChildrenNameUnique(nameNums, t, deapth);
            }
        }
    }
}

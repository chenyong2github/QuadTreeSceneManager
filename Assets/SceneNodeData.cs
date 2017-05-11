using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Scene场景节点数据
[System.Serializable]
public class SceneNode : System.Object, IQuadTreeObject
{
    public string prefabName;
    public string name;
    public Vector3 position;

    public Vector2 GetPosition()
    {
        //Ignore the Y position, Quad-trees operate on a 2D plane.
        return new Vector2(position.x, position.z);
    }
}

public class SceneNodeData : MonoBehaviour
{
    [SerializeField]
    QuadTree<SceneNode> m_sceneTree;
    public QuadTree<SceneNode> sceneTree
    {
        get
        {
            return m_sceneTree;
        }
        set
        {
            if (value == null)
            {
                Debug.LogError("!!!!!!!!!!!!!!!!!!!");
            }

            m_sceneTree = value;
        }
    }

    //节点信息
    public List<SceneNode> miniRectNodes = new List<SceneNode>();
    public List<SceneNode> nodes = new List<SceneNode>();

    public string treeName;

    //可选参数
    public int mapSize = 256;
    public int mapDensity = 1;
    public int mapMinWidth = 5;

    //子节点深度
    public int depth = 1;

    public int objCount = 0;

    bool CheckTarget(Transform tParent, Transform tTarget)
    {
        if (tTarget.gameObject.layer == 8) // Road
            return false;

        int nDepth = 1;

        while (tTarget.parent!= null && tTarget.parent != tParent)
        {
            nDepth++;
            tTarget = tTarget.parent;
        }

        return nDepth <= depth;
    }

    string GetFullName(Transform tParent, Transform tTarget)
    {
        string name = "";

        Transform temp = tTarget.parent;
        while (temp != null && temp != tParent)
        {
            name = temp.name + @"/" + name;
            temp = temp.parent;
        }

        return name+ tTarget.name;
    }

    public void Save(Transform tParent)
    {
        treeName = tParent.name + "_QuadTree";

        Rect mapRect = new Rect(-mapSize / 2, -mapSize / 2, mapSize, mapSize);
        sceneTree = new QuadTree<SceneNode>(mapDensity, mapMinWidth, mapRect);
        
        MeshRenderer[] mrs = tParent.GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] smrs = tParent.GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (MeshRenderer mr in mrs)
        {
            if (!CheckTarget(tParent, mr.transform))
                continue;

            SceneNode sn = new SceneNode();
            sn.name = GetFullName(tParent, mr.transform);
            sn.position = new Vector3(mr.transform.position.x, 0, mr.transform.position.z);
            sceneTree.Insert(sn);
            ++objCount;
        }

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if (!CheckTarget(tParent, smr.transform))
                continue;

            SceneNode sn = new SceneNode();
            sn.name = GetFullName(tParent, smr.transform);
            sn.position = new Vector3(smr.transform.position.x, 0, smr.transform.position.z);
            sceneTree.Insert(sn);
            ++objCount;
        }

        sceneTree.SetPrefabName(tParent.name);

        SaveOneNode(sceneTree);
    }

    void SaveOneNode(QuadTree<SceneNode> treeInfo)
    {
        SceneNode sn = new SceneNode();
        if (treeInfo.storedObjects.Count > 0)
        {
            sn.prefabName = treeInfo.prefabName;

            sn.position = Vector3.zero;
            // calculate storedObject's average position
            for(int i=0; i<treeInfo.storedObjects.Count; i++)
            {
                sn.position += treeInfo.storedObjects[i].position;
            }
            sn.position /= treeInfo.storedObjects.Count;

            miniRectNodes.Add(sn);
        }
        
        for (int i = 0; i < treeInfo.storedObjects.Count; i++)
        {
            treeInfo.storedObjects[i].prefabName = treeInfo.prefabName;
            nodes.Add(treeInfo.storedObjects[i]);
        }

        if (treeInfo.cells != null)
        {
            for (int i = 0; i < 4; i++)
            {
                SaveOneNode(treeInfo.cells[i]);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (sceneTree != null)
        {
            sceneTree.DrawDebug();
        }
    }


    [ContextMenu("Do SaveTreeAsset")]
    void SaveTreeAsset()
    {
#if UNITY_EDITOR
        //UnityEditor.AssetDatabase.CreateAsset(m_sceneTree, "Assets/XXXX.asset");
#endif
    }

    [ContextMenu("Do LoadTreeAsset")]
    void LoadTreeAsset()
    {
#if UNITY_EDITOR
        //QuadTree<SceneNode> t = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/XXXX.asset", typeof(QuadTree<SceneNode>)) as QuadTree<SceneNode>;
        //Debug.LogError(t);
#endif
    }
}

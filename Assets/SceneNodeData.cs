using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNodeData : MonoBehaviour {

    [System.Serializable]
    public struct SceneNode
    {
        public string name;
        public string parent;        
        public Vector3 position;
    }

    public List<SceneNode> sceneNodes;

    void Awake()
    {
        Load();
    }

    public void Save(Transform tParent)
    {
        if (sceneNodes == null)
            sceneNodes = new List<SceneNode>();
        sceneNodes.Clear();

        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (MeshRenderer mr in mrs)
        {
            if (mr.transform.parent != tParent)
                continue;

            if (mr.gameObject.layer == 8) // Road
                continue;

            SceneNode sn = new SceneNode();
            sn.parent = tParent.name;
            sn.name = mr.gameObject.name;
            sn.position = new Vector3(mr.transform.position.x, 0, mr.transform.position.z);
            sceneNodes.Add(sn);
        }

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if (smr.transform.parent != tParent)
                continue;

            if (smr.gameObject.layer == 8) // Road
                continue;

            SceneNode sn = new SceneNode();
            sn.parent = tParent.name;
            sn.name = smr.gameObject.name;
            sn.position = new Vector3(smr.transform.position.x, 0, smr.transform.position.z);
            sceneNodes.Add(sn);
        }
    }

    public void Load()
    {

    }

    private string Vecter4ToString(Vector4 vector)
    {
        return "(" + vector.x + "," + vector.y + "," + vector.z + "," + vector.w + ")";
    }
}

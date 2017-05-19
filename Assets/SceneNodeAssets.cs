using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNodeAssets : MonoBehaviour {

    public bool loaded;

    public List<Mesh> meshs = new List<Mesh>();
    public List<Material> materials = new List<Material>();
    public List<Texture> textures = new List<Texture>();    

    void Awake()
    {
        MeshFilter[] mfs = GetComponentsInChildren<MeshFilter>();
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (MeshFilter mf in mfs)
        {
            if (mf.sharedMesh == null)
                Debug.LogErrorFormat("{0} MeshFilter is Missing !!!", mf.name);
            else
                meshs.Add(mf.sharedMesh);
        }

        foreach (MeshRenderer mr in mrs)
        {
            if (mr.sharedMaterials == null)
                Debug.LogErrorFormat("{0} MeshRenderer is Missing !!!", mr.name);
            else
                materials.AddRange(mr.sharedMaterials);
        }

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if (smr.sharedMaterials == null)
                Debug.LogErrorFormat("{0} SkinnedMeshRenderer is Missing !!!", smr.name);
            else
                materials.AddRange(smr.sharedMaterials);
        }

        foreach (Material mat in materials)
        {
            if (mat.mainTexture == null)
                Debug.LogErrorFormat("{0} mainTexture is Missing !!!", mat.name);
            else
                textures.Add(mat.mainTexture);
        }
    }

    void OnDestroy()
    {
         
    }
}

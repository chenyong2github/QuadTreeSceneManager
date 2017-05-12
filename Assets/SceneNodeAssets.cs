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
            meshs.Add(mf.sharedMesh);
        }

        foreach (MeshRenderer mr in mrs)
        {
            materials.AddRange(mr.sharedMaterials);
        }

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            materials.AddRange(smr.sharedMaterials);
        }

        foreach (Material mat in materials)
        {
            if (mat.mainTexture == null)
                continue;

            textures.Add(mat.mainTexture);
        }
    }

    void OnDestroy()
    {
         
    }
}

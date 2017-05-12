using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneNodeAssets : MonoBehaviour {

    public SceneNode sceneNode;

    public bool m_bLoaded;
    public List<Material> m_materials;
    public List<Texture> m_textures;
    public List<Mesh> m_meshs;


    void Awake()
    {

    }

    void OnDestroy()
    {
         
    }

    public SceneNodeAssets(SceneNode sn)
    {
        m_bLoaded = false;

        sceneNode = sn;

        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        SkinnedMeshRenderer[] smrs = GetComponentsInChildren<SkinnedMeshRenderer>();

        m_materials = new List<Material>();
        foreach (MeshRenderer mr in mrs)
        {
            m_materials.AddRange(mr.sharedMaterials);
        }
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            m_materials.AddRange(smr.sharedMaterials);
        }

        m_textures = new List<Texture>();
        foreach (Material mat in m_materials)
        {
            if (mat.mainTexture == null)
                continue;

            m_textures.Add(mat.mainTexture);
        }
    }
}

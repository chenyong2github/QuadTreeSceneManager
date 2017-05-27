using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssetsManager : MonoBehaviour {

    private static SceneAssetsManager _instance = null;
    public static SceneAssetsManager Instance
    {
        get
        {
            return _instance;
        }
    }

    [HideInInspector]
    public int unloadDistance;

    Dictionary<SceneNode, SceneNodeAssets> loadedSceneNodes = new Dictionary<SceneNode, SceneNodeAssets>();
    Dictionary<SceneNode, SceneNodeAssets> needRemoveSceneNodes = new Dictionary<SceneNode, SceneNodeAssets>();

    public Dictionary<Mesh, int> meshRefs = new Dictionary<Mesh, int>();
    public Dictionary<Texture, int> textureRefs = new Dictionary<Texture, int>();
    public Dictionary<Material, int> materialRefs = new Dictionary<Material, int>();

    private bool _loaded = false;
    public bool loaded { get { return _loaded; } }

    void Awake()
    {
        _instance = this;
        _loaded = false;

        Application.backgroundLoadingPriority = ThreadPriority.Normal;
    }

    void Update()
    {
        if (loaded)
            return;

        CountStaticGameObjectAssetsRefs();
    }

    SceneNodeAssets LoadSceneNode(SceneNode sn, string parentName, Transform parent)
    {
        UnityEngine.Object obj = Resources.Load("QuadTree/" + parentName + "/" + sn.prefabName);
        if (obj == null)
        {
            Debug.LogError(sn.prefabName + "isn't exist!");
            return null;
        }

        GameObject go = GameObject.Instantiate(obj) as GameObject;
        go.transform.parent = parent;

        SceneNodeAssets sna = go.AddComponent<SceneNodeAssets>();

        IncreaseAssetsRefs(sna);

        sna.loaded = true;

        return sna;
    }

    void UnloadSceneNode(SceneNodeAssets sna)
    {
        if (!sna.loaded)
            return;

        Destroy(sna.gameObject);

        DecreaseAssetsRefs(sna);

        sna.loaded = false;
    }

    public void OnUpdate(List<SceneNode> visibleSceneNodes, string parentName, Transform parent, Vector3 center)
    {
        // load
        foreach (SceneNode sn in visibleSceneNodes)
        {
            if (!loadedSceneNodes.ContainsKey(sn))
            {
                SceneNodeAssets sna = LoadSceneNode(sn, parentName, parent);

                loadedSceneNodes[sn] = sna;
            }
        }

        // unload
        needRemoveSceneNodes.Clear();
        foreach (KeyValuePair<SceneNode, SceneNodeAssets> kv in loadedSceneNodes)
        {
            Vector2 v = new Vector2(center.x - kv.Key.position.x, center.z - kv.Key.position.z);
            if (v.sqrMagnitude > unloadDistance * unloadDistance)
            {
                needRemoveSceneNodes.Add(kv.Key, kv.Value);
            }
        }

        foreach (KeyValuePair<SceneNode, SceneNodeAssets> kv in needRemoveSceneNodes)
        {
            loadedSceneNodes.Remove(kv.Key);

            UnloadSceneNode(kv.Value);
        }
    }

    void CountStaticGameObjectAssetsRefs()
    {
        UnityEngine.SceneManagement.Scene nowScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        if (!nowScene.isLoaded)
        {
            return;
        }

        GameObject[] gos = nowScene.GetRootGameObjects();

        foreach (GameObject go in gos)
        {
            CountOneObjRefs(go);
        }

        _loaded = true;
    }

    void CountOneObjRefs(GameObject go)
    {
        if (go == null)
            return;

        MeshFilter[] mfs = go.GetComponentsInChildren<MeshFilter>(true);
        MeshRenderer[] mrs = go.GetComponentsInChildren<MeshRenderer>(true);
        SkinnedMeshRenderer[] smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>(true);

        List<Mesh> meshs = new List<Mesh>();
        foreach (MeshFilter mf in mfs)
        {
            meshs.Add(mf.sharedMesh);
        }

        foreach (Mesh mesh in meshs)
        {
            if (meshRefs.ContainsKey(mesh))
            {
                meshRefs[mesh]++;
            }
            else
            {
                meshRefs[mesh] = 1;
            }
        }

        List<Material> mats = new List<Material>();
        foreach (MeshRenderer mr in mrs)
        {
            mats.AddRange(mr.sharedMaterials);
        }
        foreach (SkinnedMeshRenderer smr in smrs)
        {
            mats.AddRange(smr.sharedMaterials);
        }

        foreach (Material mat in mats)
        {
            if (materialRefs.ContainsKey(mat))
            {
                materialRefs[mat]++;
            }
            else
            {
                materialRefs[mat] = 1;
            }

            if (mat.mainTexture == null)
                continue;

            if (textureRefs.ContainsKey(mat.mainTexture))
            {
                textureRefs[mat.mainTexture]++;
            }
            else
            {
                textureRefs[mat.mainTexture] = 1;
            }
        }
    }

    void IncreaseAssetsRefs(SceneNodeAssets sna)
    {
        foreach (Mesh mesh in sna.meshs)
        {
            if (meshRefs.ContainsKey(mesh))
            {
                meshRefs[mesh]++;
            }
            else
            {
                meshRefs[mesh] = 1;
            }
        }

        foreach (Material mat in sna.materials)
        {
            if (materialRefs.ContainsKey(mat))
            {
                materialRefs[mat]++;
            }
            else
            {
                materialRefs[mat] = 1;
            }
        }

        foreach (Texture tex in sna.textures)
        {
            if (textureRefs.ContainsKey(tex))
            {
                textureRefs[tex]++;
            }
            else
            {
                textureRefs[tex] = 1;
            }
        }
    }

    void DecreaseAssetsRefs(SceneNodeAssets sna)
    {
        //check unload mesh
        foreach (Mesh mesh in sna.meshs)
        {
            if (meshRefs.ContainsKey(mesh))
            {
                meshRefs[mesh]--;

                if (meshRefs[mesh] <= 0)// need unload
                {
                    meshRefs.Remove(mesh);

                    Resources.UnloadAsset(mesh);
                }
            }
        }

        foreach (Material mat in sna.materials)
        {
            if (materialRefs.ContainsKey(mat))
            {
                materialRefs[mat]--;

                if (materialRefs[mat] <= 0)
                {
                    materialRefs.Remove(mat);

                    Resources.UnloadAsset(mat);
                }
            }            
        }

        // check unload texture
        foreach (Texture tex in sna.textures)
        {
            if (textureRefs.ContainsKey(tex))
            {
                textureRefs[tex]--;

                if (textureRefs[tex] <= 0)// need unload
                {
                    textureRefs.Remove(tex);
                                
                    Resources.UnloadAsset(tex);                 
                }
            }
        }
    }
}

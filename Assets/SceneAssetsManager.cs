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

    public GameObject player;
    public int loadDistance = 20;
    public int unloadDistance = 30;

    Dictionary<SceneNode, SceneNodeAssets> loadedSceneNodes = new Dictionary<SceneNode, SceneNodeAssets>();
    Dictionary<SceneNode, SceneNodeAssets> needRemoveSceneNodes = new Dictionary<SceneNode, SceneNodeAssets>();

    public Dictionary<Mesh, int> meshRefs = new Dictionary<Mesh, int>();
    public Dictionary<Texture, int> textureRefs = new Dictionary<Texture, int>();

    void Awake()
    {
        _instance = this;

        Application.backgroundLoadingPriority = ThreadPriority.Normal;

        CountStaticGameObjectAssets();
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

    public Rect GetPlayerVisibleArea(int mapSize)
    {
        if (player != null)
        {
            Vector2 position = new Vector2(player.transform.position.x, player.transform.position.z);
            return new Rect(position.x - loadDistance, position.y - loadDistance, loadDistance * 2, loadDistance * 2);
        }

        return new Rect(Random.Range(-mapSize / 2, mapSize / 2), Random.Range(-mapSize / 2, mapSize / 2), 100 * mapSize / 1024f, 100 * mapSize / 1024f);
    }

    public void OnUpdate(List<SceneNode> visibleSceneNodes, string parentName, Transform parent)
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
        if (player != null)
        {
            foreach (KeyValuePair<SceneNode, SceneNodeAssets> kv in loadedSceneNodes)
            {
                Vector2 v = new Vector2(player.transform.position.x - kv.Key.position.x, player.transform.position.z - kv.Key.position.z);
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
    }

    void CountStaticGameObjectAssets()
    {
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in gos)
        {
            if (go == null)
                return;

            MeshFilter[] mfs = GetComponentsInChildren<MeshFilter>();
            MeshRenderer[] mrs = go.GetComponentsInChildren<MeshRenderer>();
            SkinnedMeshRenderer[] smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>();

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

        // check unload texture
        foreach (Texture tex in sna.textures)
        {
            if (textureRefs.ContainsKey(tex))
            {
                textureRefs[tex]--;

                if (textureRefs[tex] <= 0)// need unload
                {
                    textureRefs.Remove(tex);

                    foreach (Material mat in sna.materials)
                    {
                        if (mat.mainTexture && mat.mainTexture == tex)
                        {
                            Resources.UnloadAsset(mat);
                        }
                    }

                    Resources.UnloadAsset(tex);
                }
            }
        }
    }
}

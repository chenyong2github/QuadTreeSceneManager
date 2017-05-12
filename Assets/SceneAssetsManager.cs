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

    public Dictionary<Texture, int> textureRefs = new Dictionary<Texture, int>();

    void Awake()
    {
        _instance = this;

        Application.backgroundLoadingPriority = ThreadPriority.Normal;
        GameObject[] gos = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject go in gos)
        {
            //IncreaseTextureRefs(go);
        }
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
        sna.sceneNode = sn;

        //IncreaseTextureRefs(sn);

        sna.m_bLoaded = true;

        return sna;
    }

    void UnloadSceneNode(SceneNodeAssets sna)
    {
        if (!sna.m_bLoaded)
            return;

        Destroy(sna.gameObject);
        //        DecreaseTextureRefs(sn);

        sna.m_bLoaded = false;
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssetsManager : MonoBehaviour {

    public class SceneNodeAssets : SceneNode {
        public GameObject m_go;
        public List<Material> m_materials;
        public List<Texture> m_textures;
        public List<Mesh> m_meshs;
    }

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

    HashSet<QuadTreeSceneManager.SceneNode> loadedSceneNodes = new HashSet<QuadTreeSceneManager.SceneNode>();

    public Dictionary<Texture, int> textureRefs = new Dictionary<Texture, int>();

    void Awake()
    {
        _instance = this;
    }

    void LoadSceneNode(QuadTreeSceneManager.SceneNode sn, string parentName, Transform parent)
    {
        if (sn.m_bLoaded)
            return;

        UnityEngine.Object obj = Resources.Load("QuadTree/" + parentName + "/" + sn.m_prefabName);
        if (obj == null)
        {
            Debug.LogError(sn.m_prefabName + "isn't exist!");
            return;
        }

        GameObject go = GameObject.Instantiate(obj) as GameObject;
        go.transform.parent = parent;
        sn.m_go = go;

        //IncreaseTextureRefs(sn);

        sn.m_bLoaded = true;
    }

    void UnloadSceneNode(QuadTreeSceneManager.SceneNode sn)
    {
        if (!sn.m_bLoaded)
            return;

        Destroy(sn.m_go);
//        DecreaseTextureRefs(sn);

        sn.m_bLoaded = false;
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

    List<QuadTreeSceneManager.SceneNode> needRemoveSceneNodes = new List<QuadTreeSceneManager.SceneNode>();
    public void OnUpdate(List<QuadTreeSceneManager.SceneNode> visibleSceneNodes, string parentName, Transform parent)
    {
        // load
        foreach (QuadTreeSceneManager.SceneNode sn in visibleSceneNodes)
        {
            if (!loadedSceneNodes.Contains(sn))
            {
                loadedSceneNodes.Add(sn);

                LoadSceneNode(sn, parentName, parent);
            }
        }


        // unload
        needRemoveSceneNodes.Clear();
        if (player != null)
        {
            foreach (QuadTreeSceneManager.SceneNode sn in loadedSceneNodes)
            {
                Vector2 v = new Vector2(player.transform.position.x - sn.m_vPosition.x, player.transform.position.z - sn.m_vPosition.z);
                if (v.sqrMagnitude > unloadDistance * unloadDistance)
                {
                    needRemoveSceneNodes.Add(sn);
                }
            }

            foreach (QuadTreeSceneManager.SceneNode sn in needRemoveSceneNodes)
            {
                loadedSceneNodes.Remove(sn);

                UnloadSceneNode(sn);
            }
        }
    }
}

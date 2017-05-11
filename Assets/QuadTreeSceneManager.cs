using UnityEngine;
using System.Collections.Generic;

public class QuadTreeSceneManager : MonoBehaviour
{
    public class SceneNode : IQuadTreeObject
    {
        public string m_name;
        public string m_prefabName;        
        public Vector3 m_vPosition;
        public bool m_bLoaded;

        public SceneNode(string prefabName, string name, Vector3 position, bool loaded = false)
        {
            m_prefabName = prefabName;
            m_name = name;
            m_vPosition = position; m_vPosition.y = 0;
            m_bLoaded = loaded;
        }

        public Vector2 GetPosition()
        {
            //Ignore the Y position, Quad-trees operate on a 2D plane.
            return new Vector2(m_vPosition.x, m_vPosition.z);
        }
    }

    public GameObject player;
    public SceneNodeData snd;
    public int loadDistance = 20;
    public int unloadDistance = 30;

    public int mapSize = 256;
    public int mapDensity = 5;
    public int mapMinWidth = 10;

    QuadTree<SceneNode> quadTree;

    List<SceneNode> sceneNodes;

    Rect visibleArea;
    List<SceneNode> visibleSceneNodes;

    HashSet<SceneNode> loadedSceneNodes = new HashSet<SceneNode>();

    GameObject objParent;

    Dictionary<string, int> loadCount = new Dictionary<string, int>();
    Dictionary<string, GameObject> loadObj = new Dictionary<string, GameObject>();

    void OnEnable()
    {
        sceneNodes = RetrieveSceneNodes();

        quadTree = new QuadTree<SceneNode>(mapDensity, mapMinWidth, new Rect(-mapSize/2, -mapSize/2, mapSize, mapSize));
        foreach (SceneNode to in sceneNodes)
        {
            quadTree.Insert(to);
        }

        objParent = new GameObject();
        objParent.name = "QuadTree";
    }

    void Awake()
    {
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
    }

    void Update()
    {
        UpdateSceneNodes();
    }

    void OnDrawGizmos()
    {
        if (sceneNodes != null)
        {
            Gizmos.color = Color.white;
            foreach (SceneNode to in sceneNodes)
            {
                Gizmos.DrawSphere(to.m_vPosition, 1*mapSize/1024f);
            }
        }

        if (visibleArea != null)
        {
            Gizmos.color = Color.red;

            Gizmos.DrawLine(new Vector3(visibleArea.x, 0, visibleArea.y), new Vector3(visibleArea.x, 0, visibleArea.y + visibleArea.height));
            Gizmos.DrawLine(new Vector3(visibleArea.x, 0, visibleArea.y), new Vector3(visibleArea.x + visibleArea.width, 0, visibleArea.y));
            Gizmos.DrawLine(new Vector3(visibleArea.x + visibleArea.width, 0, visibleArea.y), new Vector3(visibleArea.x + visibleArea.width, 0, visibleArea.y + visibleArea.height));
            Gizmos.DrawLine(new Vector3(visibleArea.x, 0, visibleArea.y + visibleArea.height), new Vector3(visibleArea.x + visibleArea.width, 0, visibleArea.y + visibleArea.height));
        }

        if (visibleSceneNodes != null)
        {
            Gizmos.color = Color.red;
            foreach (SceneNode to in visibleSceneNodes)
            {
                Gizmos.DrawSphere(to.m_vPosition, 1 * mapSize / 1024f);
            }
        }

        if (quadTree != null)
        {
            Gizmos.color = Color.black;
            quadTree.DrawDebug();
        }
    }

    List<SceneNode> RetrieveSceneNodes()
    {
        if (sceneNodes == null)
        {
            sceneNodes = new List<SceneNode>(100);
        }

        foreach (var item in snd.nodes)
        {
            SceneNode newObject = new SceneNode(item.prefabName, item.name, item.position);
            sceneNodes.Add(newObject);
        }

        return sceneNodes;

        /*
        if (sceneNodes == null)
        {
            sceneNodes = new List<SceneNode>(100);
        }

        subParent.Clear();

        foreach (Transform t in transform)
        {
            SceneNodeData snd = t.GetComponent<SceneNodeData>();
            if (snd)
            {
                subParent[t.name] = t.gameObject;

                foreach(SceneNodeData.SceneNode sn in snd.sceneNodes)
                {
                    SceneNode newObject = new SceneNode(sn.parent, sn.name, sn.position);
                    sceneNodes.Add(newObject);
                }
            }
        }

        return sceneNodes;
        */

        /*
        MeshRenderer[] mrs = GameObject.FindObjectsOfType<MeshRenderer>();
        SkinnedMeshRenderer[] smrs = GameObject.FindObjectsOfType<SkinnedMeshRenderer>();

        List<SceneNode> testObjects = new List<SceneNode>(mrs.Length);
        foreach (MeshRenderer mr in mrs)
        {
            if (mr.gameObject.layer == 8) // Road
                continue;

            SceneNode newObject = new SceneNode(mr.gameObject, new Vector3(mr.transform.position.x, 0, mr.transform.position.z));
            testObjects.Add(newObject);
        }

        foreach (SkinnedMeshRenderer smr in smrs)
        {
            if (smr.gameObject.layer == 8) // Road
                continue;

            SceneNode newObject = new SceneNode(smr.gameObject, new Vector3(smr.transform.position.x, 0, smr.transform.position.z));
            testObjects.Add(newObject);
        }

        return testObjects;
        */
    }

    Rect GetPlayerVisibleArea()
    {
        if (player != null)
        {
            Vector2 position = new Vector2(player.transform.position.x, player.transform.position.z);
            return new Rect(position.x - loadDistance, position.y - loadDistance, loadDistance*2, loadDistance*2);
        }

        return new Rect(Random.Range(-mapSize / 2, mapSize / 2), Random.Range(-mapSize / 2, mapSize / 2), 100 * mapSize / 1024f, 100 * mapSize / 1024f);
    }

    List<SceneNode> returnedSceneNodes   = new List<SceneNode>();
    List<SceneNode> needRemoveSceneNodes = new List<SceneNode>();
    void UpdateSceneNodes()
    {
        visibleArea = GetPlayerVisibleArea();

        returnedSceneNodes.Clear();
        visibleSceneNodes = quadTree.RetrieveObjectsInArea(visibleArea, returnedSceneNodes);

        // load
        foreach (SceneNode sn in visibleSceneNodes)
        {
            if (!loadedSceneNodes.Contains(sn))
            {
                loadedSceneNodes.Add(sn);

                LoadSceneNode(sn);
            }
        }

        // unload
        needRemoveSceneNodes.Clear();
        if (player != null)
        {
            foreach (SceneNode sn in loadedSceneNodes)
            {
                Vector2 v = new Vector2(player.transform.position.x - sn.m_vPosition.x, player.transform.position.z - sn.m_vPosition.z);
                if (v.sqrMagnitude > unloadDistance * unloadDistance)
                {
                    needRemoveSceneNodes.Add(sn);                    
                }
            }

            foreach(SceneNode sn in needRemoveSceneNodes)
            {
                loadedSceneNodes.Remove(sn);

                UnloadSceneNode(sn);
            }
        }
    }

    void LoadSceneNode(SceneNode sn)
    {
        if (loadCount.ContainsKey(sn.m_prefabName))
        {
            loadCount[sn.m_prefabName]++;
        }
        else
        {
            UnityEngine.Object obj = Resources.Load("QuadTree/" + snd.treeName + "/" + sn.m_prefabName);
            if (obj == null)
            {
                Debug.LogError(sn.m_prefabName + "isn't exist!");
                return;
            }

            GameObject go = GameObject.Instantiate(obj) as GameObject;
            go.transform.parent = objParent.transform;

            loadCount.Add(sn.m_prefabName, 1);
            loadObj.Add(sn.m_prefabName, go);
        }

        sn.m_bLoaded = true;
    }

    void UnloadSceneNode(SceneNode sn)
    {
        if (!loadCount.ContainsKey(sn.m_prefabName))
        {
            return;
        }

        loadCount[sn.m_prefabName]--;
        if (loadCount[sn.m_prefabName] > 0)
        {
            return;
        }

        Destroy(loadObj[sn.m_prefabName]);

        loadCount.Remove(sn.m_prefabName);
        loadObj.Remove(sn.m_prefabName);
    }

    
}
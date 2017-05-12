using UnityEngine;
using System.Collections.Generic;

public class QuadTreeSceneManager : MonoBehaviour
{
    private static QuadTreeSceneManager _instance = null;
    public static QuadTreeSceneManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public SceneNodeData snd;

    public int mapSize = 256;
    public int mapDensity = 5;
    public float mapMinWidth = 10;

    QuadTree<SceneNode> quadTree;

    List<SceneNode> sceneNodes; //use mini rect as scene node
    List<SceneNode> objectNodes; //objects in mini rect, just for draw gizmo

    Rect visibleArea;
    List<SceneNode> visibleSceneNodes;

    GameObject objParent;

    void OnEnable()
    {
        sceneNodes = RetrieveSceneNodes();
        objectNodes = RetrieveObjectsNodes(); //for debug

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
        _instance = this;
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
                Gizmos.DrawSphere(to.position, 1 * mapSize / 1024f);
            }
        }

        if (objectNodes != null)
        {
            Gizmos.color = Color.black;
            foreach (SceneNode to in objectNodes)
            {
                Gizmos.DrawSphere(to.position, 0.5f * mapSize / 1024f);
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
                Gizmos.DrawSphere(to.position, 1 * mapSize / 1024f);
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

        foreach (var item in snd.miniRectNodes)
        {
            SceneNode newObject = new SceneNode(item.prefabName, item.name, item.position);
            sceneNodes.Add(newObject);
        }

        return sceneNodes;
    }

    List<SceneNode> RetrieveObjectsNodes()
    {
        if (objectNodes == null)
        {
            objectNodes = new List<SceneNode>(300);
        }

        foreach (var item in snd.nodes)
        {
            SceneNode newObject = new SceneNode(item.prefabName, item.name, item.position);
            objectNodes.Add(newObject);
        }

        return objectNodes;
    }

    List<SceneNode> returnedSceneNodes = new List<SceneNode>();    
    void UpdateSceneNodes()
    {
        visibleArea = SceneAssetsManager.Instance.GetPlayerVisibleArea(mapSize);

        returnedSceneNodes.Clear();
        visibleSceneNodes = quadTree.RetrieveObjectsInArea(visibleArea, returnedSceneNodes);

        //load and unload
        SceneAssetsManager.Instance.OnUpdate(visibleSceneNodes, snd.treeName, objParent.transform);
    }
}
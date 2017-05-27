using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDynLoadManager : MonoBehaviour {
    public GameObject player;
    public float checkDistance = 5;
    public int loadDistance = 20;
    public int unloadDistance = 30;

    Vector3? lastPositon;
    QuadTreeSceneManager quadTreeMgr;

    void OnEnable()
    {
        quadTreeMgr = GetComponent<QuadTreeSceneManager>();
        lastPositon = null;
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogError("SceneDynLoadManager player is null !!!");
            return;
        }

        if (SceneAssetsManager.Instance == null || !SceneAssetsManager.Instance.loaded)
            return;

        if (lastPositon == null)
        {
            UpdateSceneNodes();
        }
        else
        {
            Vector3 dis = lastPositon.Value - player.transform.position;
            if (dis.sqrMagnitude >= checkDistance * checkDistance)
            {
                UpdateSceneNodes();

            }
        }
    }

    void UpdateSceneNodes()
    {
        SceneAssetsManager.Instance.unloadDistance = unloadDistance;
        lastPositon = player.transform.position;
        Rect rect = GetPlayerVisibleArea();
        quadTreeMgr.UpdateSceneNodes(lastPositon.Value, rect);
    }

    public Rect GetPlayerVisibleArea()
    {
        Vector2 position = new Vector2(player.transform.position.x, player.transform.position.z);
        return new Rect(position.x - loadDistance, position.y - loadDistance, loadDistance * 2, loadDistance * 2);
    }
}

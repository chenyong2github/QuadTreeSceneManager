using UnityEngine;
using System.Collections.Generic;

public class PrefabLightmapData : MonoBehaviour
{
    [System.Serializable]
    public struct RendererInfo
    {
        public Renderer renderer;
        public int lightmapIndex;
        public Vector4 lightmapOffsetScale;
    }

    public List<RendererInfo> m_RendererInfo;

    void Awake()
    //void Start()
    {
        LoadLightmap();
    }

    public void SaveLightmap()
    {
        if (m_RendererInfo == null)
            m_RendererInfo = new List<RendererInfo>();
        m_RendererInfo.Clear();
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer r in renderers)
        {
            if (r.lightmapIndex != -1)
            {
                RendererInfo info = new RendererInfo();
                info.renderer = r;
                info.lightmapOffsetScale = r.lightmapScaleOffset;
                info.lightmapIndex = r.lightmapIndex;                
                m_RendererInfo.Add(info);
            }
        }
    }
        
    public void LoadLightmap()
    {
        if (m_RendererInfo == null)
            return;        
        for (int i = 0; i < m_RendererInfo.Count; ++i)        
        {
            var item = m_RendererInfo[i];
            if (item.renderer == null)
                continue;
            item.renderer.lightmapIndex = item.lightmapIndex;
            item.renderer.lightmapScaleOffset = item.lightmapOffsetScale;            
        }
    }

    private string Vecter4ToString(Vector4 vector)
    {
        return "(" + vector.x + "," + vector.y +"," + vector.z +"," + vector.w +")";
    }
}
using System.Collections.Generic;
using UnityEngine;

/*
Quadtree by Just a Pixel (Danny Goodayle) - http://www.justapixel.co.uk
Copyright (c) 2015
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/

//四元数用于平面分割,不能旋转格子
//Any object that you insert into the tree must implement this interface
public interface IQuadTreeObject
{
    Vector2 GetPosition();
}

public class QuadTree<T> where T : IQuadTreeObject
{
    private float m_minWidth;         //格子最小宽度(优先级大于最大数目)
    private int m_maxObjectCount;     //格子内最大容纳的物体数目
    private List<T> m_storedObjects;  //格子内的物体
    private Rect m_bounds;            //格子范围
    private QuadTree<T>[] cells;      //子节点 固定4个

    public QuadTree(int maxSize, float minWidth, Rect bounds)
    {
        m_bounds = bounds;
        m_minWidth = minWidth;
        m_maxObjectCount = maxSize;
        cells = new QuadTree<T>[4];
        m_storedObjects = new List<T>(maxSize);
    }

    public void Insert(T objectToInsert)
    {
        //该格子有子节点 往子节点里面塞
        if (cells[0] != null)
        {
            int iCell = GetCellToInsertObject(objectToInsert.GetPosition());
            if (iCell > -1)
            {
                cells[iCell].Insert(objectToInsert);
            }
            return;
        }

        //没有子节点 自己存储
        m_storedObjects.Add(objectToInsert);
        //大于最大存储数目拆为四个子节点
        if (m_storedObjects.Count > m_maxObjectCount && (m_minWidth > 0 && m_bounds.width > m_minWidth))
        {
            //Split the quad into 4 sections
            if (cells[0] == null)
            {
                float subWidth = (m_bounds.width / 2f);
                float subHeight = (m_bounds.height / 2f);
                float x = m_bounds.x;
                float y = m_bounds.y;
                cells[0] = new QuadTree<T>(m_maxObjectCount, m_minWidth, new Rect(x + subWidth, y, subWidth, subHeight));
                cells[1] = new QuadTree<T>(m_maxObjectCount, m_minWidth, new Rect(x, y, subWidth, subHeight));
                cells[2] = new QuadTree<T>(m_maxObjectCount, m_minWidth, new Rect(x, y + subHeight, subWidth, subHeight));
                cells[3] = new QuadTree<T>(m_maxObjectCount, m_minWidth, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
            }

            //Reallocate this quads objects into its children
            for (int i = m_storedObjects.Count - 1; i >= 0; --i)
            {
                T storedObj = m_storedObjects[i];
                int iCell = GetCellToInsertObject(storedObj.GetPosition());
                if (iCell > -1)
                {
                    cells[iCell].Insert(storedObj);
                }
                m_storedObjects.RemoveAt(i);
            }
        }
    }

    public void Remove(T objectToRemove)
    {
        if (ContainsLocation(objectToRemove.GetPosition()))
        {
            m_storedObjects.Remove(objectToRemove);
            if (cells[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    cells[i].Remove(objectToRemove);
                }
            }
        }
    }

    public List<T> RetrieveObjectsInArea(Rect area, List<T> returnedObjects = null)
    {
        if (rectOverlap(m_bounds, area))
        {
            if (returnedObjects == null)
            {
                returnedObjects = new List<T>();
            }

            for (int i = 0; i < m_storedObjects.Count; i++)
            {
                if (area.Contains(m_storedObjects[i].GetPosition()))
                {
                    returnedObjects.Add(m_storedObjects[i]);
                }
            }

            if (cells[0] != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    cells[i].RetrieveObjectsInArea(area, returnedObjects);
                }
            }
        }

        return returnedObjects;
    }

    // Clear quadtree
    public void Clear()
    {
        m_storedObjects.Clear();

        for (int i = 0; i < cells.Length; i++)
        {
            if (cells[i] != null)
            {
                cells[i].Clear();
                cells[i] = null;
            }
        }
    }

    public bool ContainsLocation(Vector2 location)
    {
        return m_bounds.Contains(location);
    }

    //该位置 属于哪个字节点
    private int GetCellToInsertObject(Vector2 location)
    {
        for (int i = 0; i < 4; i++)
        {
            if (cells[i].ContainsLocation(location))
            {
                return i;
            }
        }
        return -1;
    }

    bool valueInRange(float value, float min, float max)
    {
        return (value >= min) && (value <= max);
    }

    //两个矩形是否重合
    bool rectOverlap(Rect A, Rect B)
    {
        bool xOverlap = valueInRange(A.x, B.x, B.x + B.width) ||
                        valueInRange(B.x, A.x, A.x + A.width);

        bool yOverlap = valueInRange(A.y, B.y, B.y + B.height) ||
                        valueInRange(B.y, A.y, A.y + A.height);

        return xOverlap && yOverlap;
    }

    public void DrawDebug()
    {
        Gizmos.DrawLine(new Vector3(m_bounds.x, 0, m_bounds.y), new Vector3(m_bounds.x, 0, m_bounds.y + m_bounds.height));
        Gizmos.DrawLine(new Vector3(m_bounds.x, 0, m_bounds.y), new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y));
        Gizmos.DrawLine(new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y), new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y + m_bounds.height));
        Gizmos.DrawLine(new Vector3(m_bounds.x, 0, m_bounds.y + m_bounds.height), new Vector3(m_bounds.x + m_bounds.width, 0, m_bounds.y + m_bounds.height));
        if (cells[0] != null)
        {
            for (int i = 0; i < cells.Length; i++)
            {
                if (cells[i] != null)
                {
                    cells[i].DrawDebug();
                }
            }
        }
    }
}
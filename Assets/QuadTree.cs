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
//Any object that you insert into the tree must implement this interface
public interface IQuadTreeObject
{
    Vector2 GetPosition();
}
public class QuadTree<T> where T : IQuadTreeObject
{
    private int m_maxObjectCount;
    private List<T> m_storedObjects;
    private Rect m_bounds;
    private QuadTree<T>[] cells;

    public QuadTree(int maxSize, Rect bounds)
    {
        m_bounds = bounds;
        m_maxObjectCount = maxSize;
        cells = new QuadTree<T>[4];
        m_storedObjects = new List<T>(maxSize);
    }
    public void Insert(T objectToInsert)
    {

        if (cells[0] != null)
        {
            int iCell = GetCellToInsertObject(objectToInsert.GetPosition());
            if (iCell > -1)
            {
                cells[iCell].Insert(objectToInsert);
            }
            return;
        }
        m_storedObjects.Add(objectToInsert);
        //Objects exceed the maximum count
        if (m_storedObjects.Count > m_maxObjectCount)
        {
            //Split the quad into 4 sections
            if (cells[0] == null)
            {
                float subWidth = (m_bounds.width / 2f);
                float subHeight = (m_bounds.height / 2f);
                float x = m_bounds.x;
                float y = m_bounds.y;
                cells[0] = new QuadTree<T>(m_maxObjectCount, new Rect(x + subWidth, y, subWidth, subHeight));
                cells[1] = new QuadTree<T>(m_maxObjectCount, new Rect(x, y, subWidth, subHeight));
                cells[2] = new QuadTree<T>(m_maxObjectCount, new Rect(x, y + subHeight, subWidth, subHeight));
                cells[3] = new QuadTree<T>(m_maxObjectCount, new Rect(x + subWidth, y + subHeight, subWidth, subHeight));
            }
            //Reallocate this quads objects into its children
            int i = m_storedObjects.Count - 1;
            while (i >= 0)
            {
                T storedObj = m_storedObjects[i];
                int iCell = GetCellToInsertObject(storedObj.GetPosition());
                if (iCell > -1)
                {
                    cells[iCell].Insert(storedObj);
                }
                m_storedObjects.RemoveAt(i);
                i--;
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
    public List<T> RetrieveObjectsInArea(Rect area)
    {
        if (rectOverlap(m_bounds, area))
        {
            List<T> returnedObjects = new List<T>();
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
                    List<T> cellObjects = cells[i].RetrieveObjectsInArea(area);
                    if (cellObjects != null)
                    {
                        returnedObjects.AddRange(cellObjects);
                    }
                }
            }
            return returnedObjects;
        }
        return null;
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
    { return (value >= min) && (value <= max); }

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
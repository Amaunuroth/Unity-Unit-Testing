using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DelauneyTriangles
{
    List<Vector3> points = new List<Vector3>();
    bool[] land;

    public DelauneyTriangles(int count, Rect bounds, float minZ, float maxZ)
    {
        Dictionary<float, HashSet<float>> distinct = new Dictionary<float, HashSet<float>>();
        land = new bool[count];
        for (int i = 0; i < count; i++)
        {
            land[i] = Random.Range(0, 2) > 0;
            float x = Random.Range(bounds.xMin, bounds.xMax);
            float y = Random.Range(bounds.yMin, bounds.yMax);
            while (distinct.ContainsKey(x) && distinct[x].Contains(y))
            {
                x = Random.Range(bounds.xMin, bounds.xMax);
                y = Random.Range(bounds.yMin, bounds.yMax);
            }
            if (!distinct.ContainsKey(x))
                distinct[x] = new HashSet<float>();
            distinct[x].Add(y);
            points.Add(new Vector3(x, y, Random.Range(minZ, maxZ)));
        }
        points.Sort(Comparer);
        CircularLinkNode start = ConvexHull(0, points.Count - 1);
        MakeLine(start);
    }

    private int Comparer(Vector3 x, Vector3 y)
    {
        Vector3 a = x;
        Vector3 b = y;
        if (a.x < b.x || (a.x == b.x && a.y <= b.y))
            return -1;
        // since we have tested for x & y equality we should never hit the == case
        return 1;
    }

    private void Triangulate()
    {
    }

    private CircularLinkNode ConvexHull(int start, int end)
    {
        if (end - start > 2)
        {
            int mid = (start + end) / 2;
            CircularLinkNode a = ConvexHull(start, mid);
            CircularLinkNode b = ConvexHull(mid + 1, end);
            // emmet - need to NOT merge these YET!
            MergePair right = FindRightTangent(a, b);
            MergePair left = FindLeftTangent(a, b);
            right.Merge();
            left.Merge();
            return a;
        }
        else
        {
            CircularLinkNode node = null, first = null;
            for (int i = start; i <= end; i++)
            {
                CircularLinkNode next = new CircularLinkNode();
                next.index = i;
                next.prev = node;
                if (node == null)
                    first = next;
                else
                    node.next = next;
                node = next;
            }
            first.prev = node;
            node.next = first;
            return first;
        }
    }

    protected void MakeLine(CircularLinkNode node)
    {
        List<Vector3> pts = new List<Vector3>();
        CircularLinkNode n = node;
        while (n != node)
        {
            pts.Add(points[n.index]);
            n = n.next;
        }
        GameObject go = new GameObject();
        go.name = "DT - ConvexHull " + points.Count;
        LineRenderer renderer = go.AddComponent<LineRenderer>();
        renderer.SetPositions(pts.ToArray());
    }

    public Mesh MakeMesh()
    {
        GameObject go = new GameObject();
        go.name = "DT - Mesh " + points.Count;
        Mesh result = new Mesh();
        result.SetVertices(points);
        result.SetTriangles(new int[] { 0, 1, 2 }, 0);
        List<Color> colors = new List<Color>();
        for (int i = 0; i < land.Length; i++)
        {
            if (land[i])
                colors.Add(Color.green);
            else
                colors.Add(Color.blue);
        }

        MeshFilter filter = go.AddComponent<MeshFilter>();
        filter.mesh = result;
        MeshRenderer render = go.AddComponent<MeshRenderer>();
        return result;
    }

    protected MergePair FindRightTangent(CircularLinkNode startA, CircularLinkNode startB)
    {
        CircularLinkNode a = startA, b = startB;

        do
        {
            float slope = FlatSlope(points[a.index], points[b.index]);
            float aSlope = FlatSlope(points[a.index], points[a.next.index]);
            float bSlope = FlatSlope(points[b.index], points[b.next.index]);

            if (aSlope >= slope)
            {
                a = a.next;
            }
            else if (bSlope > slope)
            {
                b = b.next;
            }
            else
            {
                return new MergePair(a, b);
            }
        }
        while (a != startA && b != startB);
        throw new System.Exception("No right tangent found!");
    }

    protected MergePair FindLeftTangent(CircularLinkNode startA, CircularLinkNode startB)
    {
        CircularLinkNode a = startA.prev, b = startB.prev;

        do
        {
            float slope = FlatSlope(points[a.index], points[b.index]);
            float aSlope = FlatSlope(points[a.prev.index], points[a.index]);
            float bSlope = FlatSlope(points[b.prev.index], points[b.index]);

            if (aSlope >= slope)
            {
                a = a.prev;
            }
            else if (bSlope > slope)
            {
                b = b.prev;
            }
            else
            {
                return new MergePair(a, b);
            }
        }
        while (a != startA.prev && b != startB.prev);
        throw new System.Exception("No left tangent found!");
    }

    public float FlatSlope(Vector3 a, Vector3 b)
    {
        if ((a.y - b.y) == 0)
            return float.PositiveInfinity;
        return (a.x -b.x) / (a.y - b.y);
    }

    protected class CircularLinkNode
    {
        public int index;
        public CircularLinkNode prev;
        public CircularLinkNode next;
    }

    protected class MergePair
    {
        public CircularLinkNode a;
        public CircularLinkNode b;

        public MergePair(CircularLinkNode a, CircularLinkNode b)
        {
            this.a = a;
            this.b = b;
        }

        public void Merge()
        {
            a.next = b;
            b.prev = a;
        }
    }
}
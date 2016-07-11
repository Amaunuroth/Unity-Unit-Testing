using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public class IslandMap
{
    private int[,] map;

    private int width;
    private int height;

    public IslandMap(int height, int width)
    {
        this.width = width;
        this.height = height;
        map = new int[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                map[x,y] = UnityEngine.Random.Range(0, 2);
            }
        }
    }

    public int IslandCount()
    {
        int count = 0;
        bool[,] visited = new bool[width, height];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!visited[x, y] && map[x, y] == 1)
                {
                    count++;
                    Queue<Point> points = new Queue<Point>();
                    points.Enqueue(new Point(x, y));
                    while (points.Count > 0)
                    {
                        Point p = points.Dequeue();
                        if (!visited[p.x, p.y] && map[p.x, p.y] == 1)
                        {
                            visited[p.x, p.y] = true;
                            if (p.x < width - 1)
                                points.Enqueue(new Point(p.x + 1, p.y));
                            if (p.x > 0)
                                points.Enqueue(new Point(p.x - 1, p.y));

                            if (p.y < height - 1)
                                points.Enqueue(new Point(p.x, p.y + 1));
                            if (p.y > 0)
                                points.Enqueue(new Point(p.x, p.y - 1));
                        }
                    }
                }
            }
        }

        return count;
    }

    public void PrintMap()
    {
        for (int x = 0; x < width; x++)
        {
            StringBuilder sb = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                sb.Append(map[x, y]);
                sb.Append(",");
            }
            Console.WriteLine(sb.ToString());
        }
    }

    internal class Point
    {
        internal int x;
        internal int y;

        internal Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }
}

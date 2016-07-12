using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SideTree
{
    private TreeNode root;

    public SideTree(int count)
    {
        root = new TreeNode();
        Queue<TreeNode> queue = new Queue<TreeNode>();
        queue.Enqueue(root);
        int created = 1;
        while (created < count)
        {
            TreeNode node = queue.Dequeue();
            int kids = UnityEngine.Random.Range(1, 6);
            for (int i = 0; i < kids && created < count; i++)
            {
                TreeNode child = new TreeNode();
                node.AddChild(child);
                queue.Enqueue(child);
                created++;
            }
        }
    }

    public List<string> RightSide()
    {
        List<string> results = new List<string>();

        Queue<TreeNode> top = new Queue<TreeNode>();
        Queue<TreeNode> children = new Queue<TreeNode>();

        top.Enqueue(root);

        while (top.Count > 0)
        {
            TreeNode node = top.Dequeue();
            foreach (TreeNode kid in node.Children)
                children.Enqueue(kid);
            
            if (top.Count == 0)
            {
                results.Add(node.Name);
                Queue<TreeNode> temp = top;
                top = children;
                children = temp;
            }
        }
        return results;
    }

    public List<string> LeftSide()
    {
        List<string> results = new List<string>();

        Queue<TreeNode> top = new Queue<TreeNode>();
        Queue<TreeNode> children = new Queue<TreeNode>();

        top.Enqueue(root);
        bool flag = true;

        while (top.Count > 0)
        {
            TreeNode node = top.Dequeue();
            if (children.Count == 0 && flag)
            {
                results.Add(node.Name);
                flag = false;
            }

            foreach (TreeNode kid in node.Children)
                children.Enqueue(kid);

            if (top.Count == 0)
            {
                Queue<TreeNode> temp = top;
                top = children;
                children = temp;
                flag = true;
            }
        }
        return results;
    }

    public int Height()
    {
        int count = 0;

        Queue<TreeNode> top = new Queue<TreeNode>();
        Queue<TreeNode> children = new Queue<TreeNode>();

        top.Enqueue(root);

        while (top.Count > 0)
        {
            TreeNode node = top.Dequeue();
            foreach (TreeNode kid in node.Children)
                children.Enqueue(kid);

            if (top.Count == 0)
            {
                count++;
                Queue<TreeNode> temp = top;
                top = children;
                children = temp;
            }
        }
        return count;
    }

    public class TreeNode
    {
        string name;
        List<TreeNode> children = new List<TreeNode>();

        public string Name { get { return name; } }
        public List<TreeNode> Children { get { return children; } }

        public TreeNode()
        {
            name = Guid.NewGuid().ToString();
        }

        public void AddChild(TreeNode node)
        {
            children.Add(node);
        }
    }
}

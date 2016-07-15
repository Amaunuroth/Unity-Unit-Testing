using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

public interface IDontCare
{
    string Test { get; set; }

    int[][] GenerateStaggeredArray();

    void PrintArray(int[][] array);
}

public abstract class AExample : IDontCare
{
    public abstract string Test { get; set; }

    public abstract int[][] GenerateStaggeredArray();

    public void PrintArray(int[][] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            string[] inputs = Array.ConvertAll<int, string>(array[i], Convert.ToString);
            Console.Out.WriteLine(string.Join(",", inputs));
            UnityEngine.Debug.Log(string.Join(",", inputs));
        }
    }
}

public class Classified : AExample
{
    private Random rand = new Random();

    public override string Test { get; set; }

    public override int[][] GenerateStaggeredArray()
    {
        int[][] retval = new int[rand.Next(1,10)][];
        for (int i = 0; i < retval.Length; i++)
        {
            retval[i] = new int[rand.Next(1,10)];
            for (int j = 0; j < retval[i].Length; j++)
            {
                retval[i][j] = rand.Next(1,10);
            }
        }
        return retval;
    }

    public void TestExtensionMethod()
    {
        List<double> test = new List<double>();
        Random rand = new Random ();
        int len = rand.Next(1, 100);
        for (int i = 0; i < len; i++)
        {
            test.Add(rand.NextDouble());
        }
        test.ListPrinter();
    }
}

public static class TestExtensions
{
    public static void ListPrinter(this IList derp)
    {
        StringBuilder sb = new StringBuilder();
        foreach(IComparable test in derp)
        {
            sb.Append(test);
            sb.Append(",");
        }
        sb.Remove(sb.Length - 1, 1);
        Console.Out.WriteLine(sb.ToString());

        UnityEngine.Debug.Log(sb.ToString()); 
    }
}

[TestFixture]
public class ClassyTests
{
    [Test]
    public void TestClassyMatrix()
    {
        Classified classy = new Classified();
        int[][] matrix = classy.GenerateStaggeredArray();
        classy.PrintArray(matrix);
    }

    [Test]
    public void TestClassyExtension()
    {
        Classified classy = new Classified();
        classy.TestExtensionMethod();
    }

    [Test]
    public void TestNullInvoke()
    {
        // aemmet - while this compiles in Mono it does NOT work with the overloaded .Equals method in Unity :-(
        /*
        String dingus = "le Derp";
        Console.WriteLine(dingus?.ToString());
        UnityEngine.Debug.Log(dingus?.ToString());

        String nope = null;
        Console.WriteLine("emmet" + nope?.ToString());
        UnityEngine.Debug.Log("emmet" + nope?.ToString());
        */
    }

    [Test, Combinatorial]
    public void TestComboPatterns(
        [Values("hello", "jello", "cello")] string prefix,
        [Values(1,2,3,4)] int value
    )
    {
        string output = prefix + " " + value;
        Console.WriteLine(output);
        UnityEngine.Debug.Log(output);
    }


    [Test, Sequential]
    public void TestSequentialPatterns(
        [Values("hello", "jello", "cello", "yello?")] string prefix,
        [Values(1,2,3,4)] int value
    )
    {
        string output = prefix + " " + value;
        Console.WriteLine(output);
        UnityEngine.Debug.Log(output);
    }



    [Test]
    public void TestBitMasking(
        [Values(285)] int value
    )
    {
        int start = value;
        value |= (1 << 7);
        Assert.AreEqual(start, value - (1 << 7));
        value &= ~(1 << 7);
        Assert.AreEqual(start, value);
    }

    [Test]
    public void TestEncoder(
        [Values("dddddddddx3hab", "3x3ddddddab", "111111111x3ab", "bdx63a", "1111112x334ab", "aabbcc", "ab", "")]
        string value)
    {
        string encoded = EncodeDecode.Encode(value);
        string decoded = EncodeDecode.Decode(encoded);
        Assert.AreEqual(value, decoded);
    }

    [Test]
    public void TestTree([Values(1, 10, 100, 1000)] int count)
    {
        SideTree st = new SideTree(count);
        int height = st.Height();
        StringBuilder sb = new StringBuilder();
        List<string> names = st.RightSide();
        foreach (string s in names)
        {
            sb.Append(s + ", ");
        }
        UnityEngine.Debug.Log("right side: " + sb.ToString());
        Console.WriteLine("right side: " + sb.ToString());

        Assert.AreEqual(height, names.Count);

        sb = new StringBuilder();
        names = st.LeftSide();
        foreach (string s in names)
        {
            sb.Append(s + ", ");
        }
        UnityEngine.Debug.Log("left side: " + sb.ToString());
        Console.WriteLine("left side: " + sb.ToString());

        Assert.AreEqual(height, names.Count);
    }

    [Test]
    public void TestDelauneyThings([Values(10, 100, 1000)] int count)
    {
        UnityEngine.Rect rect = new UnityEngine.Rect(0f, 0f, 1f * count, 1f * count);
        DelauneyTriangles dt = new DelauneyTriangles(count, rect, -.5f, 2f);
        dt.MakeMesh();
    }
}
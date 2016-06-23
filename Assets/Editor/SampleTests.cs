using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// Sample tests modified from the Unity Unit Tests Asset Store package.
/// NOTE: in the command line usage the results file must be specified using a Full Path or the Test library will fail!
/// Also note that Unity cannot be running at the same time when you launch in batch mode.
/// OSX version:
/// /Applications/Unity/Unity.app/Contents/MacOS/Unity -batchmode -projectPath <path-to-project>/Unity-Unit-Testing -runEditorTests -editorTestsResultFile <path-to-project>/results.xml -logFile log.xml
/// </summary>

[TestFixture]
[Category("Sample Tests")]
internal class SampleTests
{
    [Test]
    [Ignore("Ignored test")]
    public void IgnoredTest()
    {
        throw new Exception("Ignored this test");
    }

    // aemmet - this is NOT recommended - your test case will still fail, and you'll have to start again
    // alternately you're covering up an infrequent infinite loop. why?
    [Test]
    [Ignore("Ignored test")]
    [MaxTime(100)]
    [Category("Failing Tests")]
    public void SlowTest()
    {
        Thread.Sleep(200);
    }
    
    // aemmet - use this in internal cases where you want to early out the failure
    [Test]
    [Ignore("Ignored test")]
    [Category("Failing Tests")]
    public void FailingTest()
    {
        for (int i = 0; i < 4; i++)
            if (i == 3)
                Assert.Fail();
        Assert.Pass();
    }

    // aemmet - an inconclusive test Fails in Unity! This means that a Jenkins job 
    // or other framework that looks at the Status Code of the execute command will fail!
    [Test]
    [Ignore("Ignored test")]
    [Category("Failing Tests")]
    public void InconclusiveTest()
    {
        Assert.Inconclusive();
    }

    // aemmet - similarly you can early out a pass if future Assert scenarios would fail
    [Test]
    public void PassingTest()
    {
        int i = 1;
        if (i > 0)
            Assert.Pass();
        Assert.Fail();
    }

    [Test]
    public void ParameterizedTest([Values(1, 2, 3)] int a)
    {
        Assert.AreEqual(a.GetType(),typeof(int));
    }

    [Test]
    public void RangeTest([NUnit.Framework.Range(1, 10, 3)] int x)
    {
        Assert.IsTrue(x % 3 == 1);
    }

    [Test]
    [Culture("pl-PL")]
    public void CultureSpecificTest()
    {
        Assert.Pass();
    }

    [Test]
    [ExpectedException(typeof(ArgumentException), ExpectedMessage = "expected message")]
    public void ExpectedExceptionTest()
    {
        throw new ArgumentException("expected message");
    }

    public enum Days : int { Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }

    [Theory]
    public void DateChecking(Days day)
    {
        int next = ((int)day + 7) % 7;
        Assert.AreEqual(day, (Days)next);
    }

    [Test, Sequential]
    public void DateValueTest(
        [Values(Days.Sunday, Days.Monday, Days.Tuesday, Days.Wednesday, Days.Thursday, Days.Friday, Days.Saturday)] Days day,
        [Values(0,1,2,3,4,5,6)] int value
    )
    {
        Assert.AreEqual(day, (Days)value);
        Assert.AreEqual(value, (int)day);
    }

    [Datapoint]
    static IList intList = new List<int>() { 1, 2, 3, 4 };

    [Datapoint]
    static IList stringList = new List<string>() { "hello", "jello", "cello" };

    [Theory]
    public void TestListShuffle(IList input)
    {
        object[] elements = new object[input.Count];
        input.CopyTo(elements, 0);
        for (int i = 0; i < input.Count; ++i)
        {
            input.Shuffle();
        }
        for (int i = 0; i < elements.Length; ++i)
        {
            Assert.Contains(elements[i], input);
        }
    }

    [Datapoint]
    Array intArray = new int[] { -1, -2, -3, -4, -5 };

    [Datapoint]
    Array stringArray = new string[] { "jam", "cam", "yam", "dam", "ram", "ham" };

    [Theory]
    public void TestTheoryArrayShuffle(Array input)
    {
        object[] elements = new object[input.Length];
        input.CopyTo(elements, 0);
        for (int i = 0; i < input.Length; ++i)
        {
            // aemmet - note that this doesn't require <T> for the Shuffle.
            input.Shuffle();
        }
        for (int i = 0; i < elements.Length; ++i)
        {
            Assert.Contains(elements[i], input);
        }
    }

    [Test]
    // aemmet - can't use a string[] in inputs, because NUnit separates the object[] elements, which then mismatch the method sig!
    public void TestStringArrayShuffle()
    {
        string[] input = new string[] { "jam", "cam", "yam", "dam", "ram", "ham" };
        TestArrayShuffle<string>(input);
    }

    [Test]
    [TestCase(new int[] { -1, -2, -3, -4, -5 })] // aemmet - primitive type arrays can be used, since NUnit uses object[]
    public void TestIntArrayShuffle(int[] input)
    {
        TestArrayShuffle<int>(input);
    }

    public void TestArrayShuffle<T>(T[] input)
    {
        T[] elements = new T[input.Length];
        input.CopyTo(elements, 0);
        for (int i = 0; i < input.Length; ++i)
        {
            // aemmet - note that this doesn't require <T> for the Shuffle.
            input.Shuffle();
        }
        for (int i = 0; i < elements.Length; ++i)
        {
            Assert.Contains(elements[i], input);
        }
    }

    [Test]
    // aemmet - while this will not work outside of Unity, we can create game objects in our Scene in Unity Test Runs
    // this will Dirty the current scene though!
    public void GameObjectTest()
    {
        GameObject gameObject = new GameObject();
        gameObject.AddComponent<CryptoRandom>();

        string name = "Test";
        gameObject.name = name;

        Assert.AreEqual(name, gameObject.name);
    }
}

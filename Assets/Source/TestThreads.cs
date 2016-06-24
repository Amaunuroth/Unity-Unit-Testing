using UnityEngine;
using System.Threading;
using System.Collections.Generic;

public class TestThread
{
    public int Result { get; private set; }

    public void StartThread()
    {
        Debug.Log("emmet - thread started");
        Thread thread = new Thread(RunUnityRandom);
        thread.Start();
    }

    public void RunUnityRandom()
    {
        Debug.Log("emmet - threading?");
    }
}

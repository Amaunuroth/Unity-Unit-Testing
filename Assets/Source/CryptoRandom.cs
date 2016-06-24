using UnityEngine;
using System;
using System.Collections;
using System.Security.Cryptography;

public class CryptoRandom : MonoBehaviour
{
    private RNGCryptoServiceProvider rng;
    public int Roll = 0;
    public float Time = 1f;
    public TextMesh Text;
    byte[] data;
    TestThread thread;

    void Start ()
    {
        data = new byte[sizeof(Int32)];
        rng = new RNGCryptoServiceProvider();
//        thread = new TestThread();
//        thread.StartThread();
        StartCoroutine(MakeRoll());
    }

    IEnumerator MakeRoll()
    {
        while (true)
        {
            rng.GetBytes(data);
//            if (thread.Result > 0)
//                Roll = thread.Result;
//            else
                Roll = BitConverter.ToInt32(data, 0);
            Text.text = Roll.ToString();
            yield return new WaitForSeconds(Time);
        }
    }
}

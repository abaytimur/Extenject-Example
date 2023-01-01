using System;
using UnityEngine;
using Zenject;

public class Test : IInitializable, ITickable, IDisposable
{
    public void Initialize()
    {
        Debug.Log("Test.Initialize() called");
    }

    public void Tick()
    {
        Debug.Log("Test.Tick() called");
    }

    public void Dispose()
    {
        Debug.Log("Test.Dispose() called");
    }
}
using System;
using UnityEngine;

public interface IStateJob
{
    public void StartJob(Action onJobComplete);
}

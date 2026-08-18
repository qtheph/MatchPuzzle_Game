using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoroutineRunner
{
    Coroutine StartCoroutine(IEnumerator routine);
    void StopCoroutine(Coroutine coroutine);
}

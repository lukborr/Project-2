using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DestroyGo());
    }


    private IEnumerator DestroyGo()
    {
        yield return new WaitForSeconds(0.45f);
        Destroy(gameObject);
    }
}

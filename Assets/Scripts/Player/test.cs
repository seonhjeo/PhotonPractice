using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    // Start is called before the first frame update
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Hello");
    }
}

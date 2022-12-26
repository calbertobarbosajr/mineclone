using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpEX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        print(Mathf.Lerp(1, 4, 0.5f));
       
        print(Mathf.InverseLerp(1, 4, 0.5f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

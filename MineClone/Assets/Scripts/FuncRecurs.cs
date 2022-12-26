using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncRecurs : MonoBehaviour
{

    //5! = 5 * 4 * 3 * 2 * 1 = 120

    // Start is called before the first frame update
    void Start()
    {
        int resul = Fat(5);
        print(resul);
    }

    int Fat(int numero)
    {
        int resultado;

        if(numero <= 1)
        {
            resultado = 1;
        }
        else
        {
            resultado = numero * Fat(numero - 1);
        }

        return resultado;
    }
}

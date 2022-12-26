using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruido : MonoBehaviour
{
    static int maxAlrtura = 150;
    static float smooth = 0.01f;
    static int cicles = 4;
    static float persists = 0.5f;
 


    static float MoveB(float x,float z, int cicle, float persist)
    {
        float frequencia = 1;
        float amplitude = 1;
        float total = 0;
        float maxVal = 0;
        float offset = 32000f;

        for(int i = 0; i < cicle; i++)
        {
            total += Mathf.PerlinNoise((x + offset) * frequencia, (z+offset) * frequencia) * amplitude;
            maxVal += amplitude;
            amplitude *= persist;
            frequencia *= 2;
        }

        return total / maxVal;
    }

    public static float Caverna(float x, float y, float z, float smooth, int cicles)
    {
        float xy = MoveB(x * smooth*10, y * smooth, cicles, 0.5f);
        float yz = MoveB(y * smooth*10, z * smooth, cicles, 0.5f);
        float xz = MoveB(x * smooth*10, z * smooth, cicles, 0.5f);

        float zy = MoveB(z * smooth, y * smooth, cicles, 0.5f);
        float zx = MoveB(z * smooth, x * smooth, cicles, 0.5f);
        float yx = MoveB(y * smooth, x * smooth, cicles, 0.5f);

        return (xy + yz + xz + zy + zx + yx) / 6;
    }


    public static int GeraAltura(float x, float z)
    {
        float altura = Mapa(0, maxAlrtura, 0, 1, MoveB(x * smooth, z * smooth, cicles, persists));
        return (int)altura;
    }

    public static int GeraAlturaRocha(float x, float z)
    {
        float altura = Mapa(0, maxAlrtura - 5, 0, 1, MoveB(x * smooth * 2, z * smooth * 2, cicles + 1, persists));
        return (int)altura;
    }

    static float Mapa(float vMin,float vMax,float oriMin, float oriMax,float valor)
    {
        return Mathf.Lerp(vMin, vMax, Mathf.InverseLerp(oriMin, oriMax, valor));
    }



}

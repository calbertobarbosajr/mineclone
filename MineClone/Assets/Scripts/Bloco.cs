using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bloco : MonoBehaviour
{

    public bool solido;
    public Mundo_Chunk dono;

    public enum LadosCubo { baixo, cima, esquerda, direita, frente, tras };
    public enum tipoTextura{ grama,terra,rocha,ar}

    public tipoTextura tipoText;
    public Vector3 posicao;
    public GameObject obj;  
    public Material cuboMat;

    public MeshFilter meshFilter;    

    [SerializeField]
    private MeshCollider chunkCol;

    Vector3[] vertices = new Vector3[4];
    Vector3[] normals = new Vector3[4];
    int[] triangulos = new int[6];
    private Vector2[] uv = new Vector2[4];

    Vector2 uv00;
    Vector2 uv10;
    Vector2 uv01;
    Vector2 uv11;

    Vector3 p0 = new Vector3(-0.5f, -0.5f, 0.5f);
    Vector3 p1 = new Vector3(0.5f, -0.5f, 0.5f);
    Vector3 p2 = new Vector3(0.5f, -0.5f, -0.5f);
    Vector3 p3 = new Vector3(-0.5f, -0.5f, -0.5f);
    Vector3 p4 = new Vector3(-0.5f, 0.5f, 0.5f);
    Vector3 p5 = new Vector3(0.5f, 0.5f, 0.5f);
    Vector3 p6 = new Vector3(0.5f, 0.5f, -0.5f);
    Vector3 p7 = new Vector3(-0.5f, 0.5f, -0.5f);


    Vector2[,] blockUVs = { 
		/*Grama Cima*/		{new Vector2( 0.125f, 0.375f ), new Vector2( 0.1875f, 0.375f),
                                new Vector2( 0.125f, 0.4375f ),new Vector2( 0.1875f, 0.4375f )},
		/*Grama Lado*/		{new Vector2( 0.1875f, 0.9375f ), new Vector2( 0.25f, 0.9375f),
                                new Vector2( 0.1875f, 1.0f ),new Vector2( 0.25f, 1.0f )},
		/*Terra*/			{new Vector2( 0.125f, 0.9375f ), new Vector2( 0.1875f, 0.9375f),
                                new Vector2( 0.125f, 1.0f ),new Vector2( 0.1875f, 1.0f )},
		/*Pedra*/			{new Vector2( 0, 0.875f ), new Vector2( 0.0625f, 0.875f),
                                new Vector2( 0, 0.9375f ),new Vector2( 0.0625f, 0.9375f )},
		
                        };

    public Bloco(tipoTextura t,Vector3 pos,GameObject p,Mundo_Chunk o)
    {
        tipoText = t;
        dono = o;
        posicao = pos;
        obj = p;        
        if(tipoText == tipoTextura.ar)
        {
            solido = false;
        }
        else
        {
            solido = true;
        }
        

    }
    //

    public void TipoTex(tipoTextura t)
    {
        tipoText = t;
        if(tipoText == tipoTextura.ar)
        {
            solido = false;
        }
        else
        {
            solido = true;
        }
    }

    //novo

    public void ConstBloco(tipoTextura t)
    {
        TipoTex(t);
        dono.ReDesenhaBloco();        
    }
    public void DestBloco()
    {
        tipoText = tipoTextura.ar;
        solido = false;
        dono.ReDesenhaBloco();       
    }


    //

    int ConvertPos(int a)
    {
        if(a == -1)
        {
            a = Mundo.chunkTam - 1;
        }
        else if(a == Mundo.chunkTam)
        {
            a = 0;
        }

        return a;
    }

    public bool TemVizinhoSolido(int x, int y, int z)
    {
        Bloco[,,] chunks;

        if(x < 0 || x >= Mundo.chunkTam ||
            y < 0 || y >= Mundo.chunkTam ||
            z < 0 || z >= Mundo.chunkTam)
        {
            Vector3 posVizinho = this.obj.transform.position + new Vector3(
                (x - (int)posicao.x) * Mundo.chunkTam,
                (y - (int)posicao.y) * Mundo.chunkTam,
                (z - (int)posicao.z) * Mundo.chunkTam);

            string nomeK = Mundo.ChunkNome(posVizinho);

            x = ConvertPos(x);
            y = ConvertPos(y);
            z = ConvertPos(z);

            Mundo_Chunk ch;
            if(Mundo.chunks.TryGetValue(nomeK, out ch))
            {
                chunks = ch.chunkData;
            }
            else
            {
                return false;
            }

        }
        else
        {
            chunks = dono.chunkData;
        }


        try
        {
            return chunks[x, y, z].solido;
        }
        catch (System.IndexOutOfRangeException) { }
        
            
            return false;
        

        
    }





    public void CriaCubo(LadosCubo side)
    {
        Mesh mesh = new Mesh();
        mesh.name = "ScriptedMesh" + side.ToString();


        if (tipoText == tipoTextura.grama && side == LadosCubo.cima)
        {
            uv00 = blockUVs[0, 0];
            uv10 = blockUVs[0, 1];
            uv01 = blockUVs[0, 2];
            uv11 = blockUVs[0, 3];
        }
        else if (tipoText == tipoTextura.grama && side == LadosCubo.baixo)
        {
            uv00 = blockUVs[2, 0];
            uv10 = blockUVs[2, 1];
            uv01 = blockUVs[2, 2];
            uv11 = blockUVs[2, 3];
        }
        else
        {
            uv00 = blockUVs[(int)tipoText + 1, 0];
            uv10 = blockUVs[(int)tipoText + 1, 1];
            uv01 = blockUVs[(int)tipoText + 1, 2];
            uv11 = blockUVs[(int)tipoText + 1, 3];
        }

        


        switch (side)
        {
            case LadosCubo.baixo:
                vertices = new Vector3[] { p0, p1, p2, p3 };
                normals = new Vector3[] {Vector3.down, Vector3.down,
                                            Vector3.down, Vector3.down};
                uv = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangulos = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case LadosCubo.cima:
                vertices = new Vector3[] { p7, p6, p5, p4 };
                normals = new Vector3[] {Vector3.up, Vector3.up,
                                            Vector3.up, Vector3.up};
                uv = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangulos = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case LadosCubo.esquerda:
                vertices = new Vector3[] { p7, p4, p0, p3 };
                normals = new Vector3[] {Vector3.left, Vector3.left,
                                            Vector3.left, Vector3.left};
                uv = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangulos = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case LadosCubo.direita:
                vertices = new Vector3[] { p5, p6, p2, p1 };
                normals = new Vector3[] {Vector3.right, Vector3.right,
                                            Vector3.right, Vector3.right};
                uv = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangulos = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case LadosCubo.frente:
                vertices = new Vector3[] { p4, p5, p1, p0 };
                normals = new Vector3[] {Vector3.forward, Vector3.forward,
                                            Vector3.forward, Vector3.forward};
                uv = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangulos = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
            case LadosCubo.tras:
                vertices = new Vector3[] { p6, p7, p3, p2 };
                normals = new Vector3[] {Vector3.back, Vector3.back,
                                            Vector3.back, Vector3.back};
                uv = new Vector2[] { uv11, uv01, uv00, uv10 };
                triangulos = new int[] { 3, 1, 0, 3, 2, 1 };
                break;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uv;
        mesh.triangles = triangulos;

        mesh.RecalculateBounds();

        GameObject quad = new GameObject("Quad");
        quad.transform.position = posicao;
        quad.transform.parent = this.obj.transform;

        MeshFilter meshFilter = (MeshFilter)quad.AddComponent(typeof(MeshFilter));
        meshFilter.mesh = mesh;
        
    }

    




    public void Draw()
    {
        if (tipoText == tipoTextura.ar) return;

        if (!TemVizinhoSolido((int)posicao.x, (int)posicao.y, (int)posicao.z + 1))
            CriaCubo(LadosCubo.frente);
        if (!TemVizinhoSolido((int)posicao.x, (int)posicao.y, (int)posicao.z - 1))
            CriaCubo(LadosCubo.tras);
        if (!TemVizinhoSolido((int)posicao.x, (int)posicao.y + 1, (int)posicao.z))
            CriaCubo(LadosCubo.cima);
        if (!TemVizinhoSolido((int)posicao.x, (int)posicao.y - 1, (int)posicao.z))
            CriaCubo(LadosCubo.baixo);
        if (!TemVizinhoSolido((int)posicao.x - 1, (int)posicao.y, (int)posicao.z))
            CriaCubo(LadosCubo.esquerda);
        if (!TemVizinhoSolido((int)posicao.x + 1, (int)posicao.y, (int)posicao.z))
            CriaCubo(LadosCubo.direita);
    }


}



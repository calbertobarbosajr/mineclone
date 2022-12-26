using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mundo_Chunk : MonoBehaviour
{
    public Material material;
    public Bloco[,,] chunkData;    
    
    private MeshFilter[] meshFilter;

    public GameObject chunk;

    public enum ChunkEstado
    {
        Desenho,
        Feito,
        Guarda
    };

    public ChunkEstado estado;


    void BuildChunk()
    {

        chunkData = new Bloco[Mundo.chunkTam,Mundo.chunkTam,Mundo.chunkTam];

        //Criar           

        for (int z = 0; z < Mundo.chunkTam; z++)
        {
            for (int y = 0; y < Mundo.chunkTam; y++)
            {
                for (int x = 0; x < Mundo.chunkTam; x++)
                {
                    Vector3 pos = new Vector3(x,y,z);

                    if(Ruido.Caverna(chunk.transform.position.x + x, chunk.transform.position.y + y, chunk.transform.position.z + z,0.4f,3) < 0.4f)
                    {
                        chunkData[x, y, z] = new Bloco(Bloco.tipoTextura.ar, pos, chunk.gameObject, this);
                    }

                    else if(chunk.transform.position.y + y <= Ruido.GeraAlturaRocha(chunk.transform.position.x + x, chunk.transform.position.z + z))
                    {
                        chunkData[x,y,z] = new Bloco(Bloco.tipoTextura.rocha,pos,chunk.gameObject,this);
                    }
                    else if (chunk.transform.position.y + y == Ruido.GeraAltura(chunk.transform.position.x + x, chunk.transform.position.z + z))
                    {
                        chunkData[x, y, z] = new Bloco(Bloco.tipoTextura.grama, pos, chunk.gameObject, this);
                    }
                    else if (chunk.transform.position.y + y < Ruido.GeraAltura(chunk.transform.position.x + x, chunk.transform.position.z + z))
                    {
                        chunkData[x, y, z] = new Bloco(Bloco.tipoTextura.terra, pos, chunk.gameObject, this);
                    }
                    else
                    {
                        chunkData[x,y,z] = new Bloco(Bloco.tipoTextura.ar,pos,chunk.gameObject,this);

                    }

                    estado = ChunkEstado.Desenho;
                }
            }
        }
    }

    public void DrawChunk()
    {
        //Desenha Blocos

        for (int z = 0; z < Mundo.chunkTam; z++)
        {
            for (int y = 0; y < Mundo.chunkTam; y++)
            {
                for (int x = 0; x < Mundo.chunkTam; x++)
                {                    
                    chunkData[x,y,z].Draw();                   
                }
            }
            
        }
        CombineTudo();
        MeshCollider collider = chunk.gameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
        collider.sharedMesh = chunk.transform.GetComponent<MeshFilter>().mesh;
        estado = ChunkEstado.Feito;
    }

    public Mundo_Chunk(Vector3 posicao, Material c)
    {
        chunk = new GameObject(Mundo.ChunkNome(posicao));
        chunk.transform.position = posicao;
        material = c;
        BuildChunk();
    }

    // Start is called before the first frame update
    void Start()
    {
        print(chunkData[0,0,0]);
    }


    void CombineTudo()
    {
        meshFilter = chunk.GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilter.Length];
        int m = 0;
        while (m < meshFilter.Length)
        {
            combine[m].mesh = meshFilter[m].sharedMesh;
            combine[m].transform = meshFilter[m].transform.localToWorldMatrix;
            m++;
        }

        if (chunk.gameObject.GetComponent<MeshFilter>() == null)
        {

        
            MeshFilter mf = (MeshFilter)chunk.gameObject.AddComponent(typeof(MeshFilter));
            mf.mesh = new Mesh();
        

            mf.mesh.CombineMeshes(combine);
        }

        if(chunk.gameObject.GetComponent<MeshRenderer>() == null)
        {
            MeshRenderer renderer = (MeshRenderer)chunk.gameObject.AddComponent(typeof(MeshRenderer));
            renderer.material = material;
        }


        foreach (Transform item in chunk.transform)
        {
            Destroy(item.gameObject);
        }
    }

    public void ReDesenhaBloco()
    {
        GameObject.DestroyImmediate(chunk.GetComponent<MeshFilter>());
        GameObject.DestroyImmediate(chunk.GetComponent<MeshRenderer>());
        GameObject.DestroyImmediate(chunk.GetComponent<Collider>());
        DrawChunk();
    }

}

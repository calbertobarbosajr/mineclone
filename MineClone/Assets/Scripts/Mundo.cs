using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Realtime.Messaging.Internal;
using UnityEngine.SceneManagement;
using System;

public class Mundo : MonoBehaviour
{

    public Material texturaMat;
    public static int colunaAlt = 4;
    public static int chunkTam = 4;
    public static int mundoTam = 1;
    // public static Dictionary<string,Mundo_Chunk> chunks;
    public static ConcurrentDictionary<string, Mundo_Chunk> chunks;
    public static int raio;
    public GameObject jogador;
    private bool primeiraConst;
    private bool construindo;

    public Vector3 ultimaPosConst,movimentoJ;
    public CoroutineQueue queue;
    public uint maxCoro = 500;

    public static List<string> removeCena = new List<string>();


    IEnumerator DescarregaCenaAdd()
    {
        yield return new WaitForSeconds(5f);
        SceneManager.UnloadSceneAsync(0);
    }

    private void Awake()
    {
        raio = 3;
        primeiraConst = true;
        construindo = false;

        StartCoroutine(DescarregaCenaAdd());
    }


    public static string ChunkNome(Vector3 v)
    {
        return (int)v.x + "-" + (int)v.y + "-" + (int)v.z;
    }

   /* IEnumerator ChunkColunaBuild()
    {
        for (int i = 0; i < colunaAlt; i++)
        {
            Vector3 chunkPos = new Vector3(this.transform.position.x, i * chunkTam,this.transform.position.z);
            Mundo_Chunk c = new Mundo_Chunk(chunkPos,texturaMat);
            c.chunk.transform.parent = this.transform;
            chunks.Add(c.chunk.name,c);
        }

        foreach (KeyValuePair<string,Mundo_Chunk> c in chunks)
        {
            c.Value.DrawChunk();
            yield return null;
        }
    }*/

    /*IEnumerator MundoBuild()
    {
        construindo = true;
        int posX = (int)Mathf.Floor(jogador.transform.position.x / chunkTam);
        int posZ = (int)Mathf.Floor(jogador.transform.position.z / chunkTam);

        for (int z = -raio; z <= raio; z++)
            for (int x = -raio; x <= raio; x++)
                for (int y = 0; y < colunaAlt; y++)
                
                {
                    Vector3 chunkPos = new Vector3((x + posX) * chunkTam, y * chunkTam,(z + posZ) * chunkTam);

                    Mundo_Chunk c;
                    string chunkString = ChunkNome(chunkPos);

                    if(chunks.TryGetValue(chunkString,out c))
                    {
                        c.estado = Mundo_Chunk.ChunkEstado.Guarda;
                        break;
                    }
                    else
                    {
                        c = new Mundo_Chunk(chunkPos, texturaMat);
                        c.chunk.transform.parent = this.transform;
                        chunks.Add(c.chunk.name, c);
                    }

                    yield return null;

                }



        if(primeiraConst)
        {
            primeiraConst = false;
        }

        construindo = false;
    }*/

    void MundoBuild(int x, int y, int z)
    {
        Vector3 chunkPos = new Vector3(x * chunkTam, y * chunkTam, z * chunkTam);

        Mundo_Chunk c;
        string chunkString = ChunkNome(chunkPos);

        if(!chunks.TryGetValue(chunkString,out c))
        {
            c = new Mundo_Chunk(chunkPos, texturaMat);
            c.chunk.transform.parent = this.transform;
            chunks.TryAdd(c.chunk.name, c);
        }
    }

    IEnumerator MundoRepetidoBuild(int x, int y, int z, int raio)
    {
        raio--;
        if(raio <= 0)
        {
            yield break;
        }
       
 
         DirMundoRepetidoPositivo(true,x,y,z,0,0,1,raio);
       
         DirMundoRepetidoPositivo(false, x, y, z, 0, 0,1, raio);
        
         DirMundoRepetidoPositivo(true, x, y, z, 1, 0, 0, raio);
        
         DirMundoRepetidoPositivo(false, x, y, z, 1, 0, 0, raio);
        
         DirMundoRepetidoPositivo(true, x, y, z, 0, 1, 0, raio);
        
         DirMundoRepetidoPositivo(false, x, y, z, 0, 1, 0, raio);
        yield return null;

    }

    void DirMundoRepetidoPositivo(bool positivo,int x, int y , int z, int xi, int yi, int zi, int raio)
    {
        if(positivo)
        {
            MundoBuild(x + xi, y + yi, z + zi);
            queue.Run(MundoRepetidoBuild(x + xi, y + yi, z + zi, raio));
        }
        else
        {
            MundoBuild(x - xi, y - yi, z - zi);
            queue.Run(MundoRepetidoBuild(x - xi, y - yi, z - zi, raio));
        }
    }

    IEnumerator DesenhaCh()
    {
        foreach(KeyValuePair<string, Mundo_Chunk> c in chunks)
        {
            if(c.Value.estado == Mundo_Chunk.ChunkEstado.Desenho)
            {
                c.Value.DrawChunk();
            }
            if (c.Value.chunk && Vector3.Distance(jogador.transform.position,c.Value.chunk.transform.position) > raio * chunkTam)
            {
                removeCena.Add(c.Key);
            }
            yield return null;
        }
    }

    IEnumerator RemoveChunk()
    {
        for (int i = 0; i < removeCena.Count; i++)
        {
            string s = removeCena[i];
            Mundo_Chunk c;
            if(chunks.TryGetValue(s,out c))
            {
                Destroy(c.chunk);
                chunks.TryRemove(s,out c);
                 yield return null;
            }
        }
    }




    public void ConstPertoJ()
    {
        StopCoroutine("MundoRepetidoBuild");
        queue.Run(MundoRepetidoBuild((int)(jogador.transform.position.x / chunkTam),
                                    (int)(jogador.transform.position.y / chunkTam),
                                    (int)(jogador.transform.position.z / chunkTam), raio));
    }



    // Start is called before the first frame update
    void Start()
    {

        ultimaPosConst = jogador.transform.position;

        chunks = new ConcurrentDictionary<string, Mundo_Chunk>();
        this.transform.position = Vector3.zero;
        this.transform.rotation = Quaternion.identity;

        queue = new CoroutineQueue(maxCoro, StartCoroutine);

        // MundoBuild((int)(jogador.transform.position.x / chunkTam), (int)(jogador.transform.position.y / chunkTam), (int)(jogador.transform.position.z / chunkTam));
        
        queue.Run(MundoRepetidoBuild((int)(jogador.transform.position.x / chunkTam),
                            (int)(jogador.transform.position.y / chunkTam),
                            (int)(jogador.transform.position.z / chunkTam), 5));



        queue.Run(DesenhaCh());

        



        primeiraConst = false;
    }

    // Update is called once per frame
    void Update()
    {
        movimentoJ = ultimaPosConst - jogador.transform.position;

        if(movimentoJ.magnitude >= chunkTam)
        {
            ultimaPosConst = jogador.transform.position;
            ConstPertoJ();           
            
            queue.Run(DesenhaCh());
            queue.Run(RemoveChunk());
            
        }

        

    }


    //novo

    public static Bloco AjustaBlocoID(Vector3 pos)
    {
        int cx, cy, cz;

        if(pos.x < 0)
        {
            cx = (int)((Mathf.Round(pos.x - chunkTam)+1) / (float)chunkTam) * chunkTam;
        }
        else
        {
            cx = (int)(Mathf.Round(pos.x) / (float)chunkTam) * chunkTam;
        }

        if (pos.y < 0)
        {
            cy = (int)((Mathf.Round(pos.y - chunkTam)+1) / (float)chunkTam) * chunkTam;
        }
        else
        {
            cy = (int)(Mathf.Round(pos.y) / (float)chunkTam) * chunkTam;
        }

        if (pos.z < 0)
        {
            cz = (int)((Mathf.Round(pos.z - chunkTam)+1) / (float)chunkTam) * chunkTam;
        }
        else
        {
            cz = (int)(Mathf.Round(pos.z) / (float)chunkTam) * chunkTam;
        }

        int blx = (int)Mathf.Abs((float)Math.Round(pos.x) - cx);
        int bly = (int)Mathf.Abs((float)Math.Round(pos.y) - cy);
        int blz = (int)Mathf.Abs((float)Math.Round(pos.z) - cz);


        string cn = ChunkNome(new Vector3(cx, cy, cz));
        Mundo_Chunk c;
        if (chunks.TryGetValue(cn, out c))
        {
            return c.chunkData[blx, bly, blz];
        }
        else
            return null;
    }




    //

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletaBloco : MonoBehaviour
{

 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 10))
            {


                Mundo_Chunk hitBl;
                if (!Mundo.chunks.TryGetValue(hit.collider.gameObject.name, out hitBl)) return;

                Vector3 hitBloco;

                
                if (Input.GetMouseButtonDown(0))
                {
                    hitBloco = hit.point - hit.normal*0.5f;
                }
                else
                {
                    hitBloco = hit.point + hit.normal * 0.5f;
                }
                    

                Bloco b = Mundo.AjustaBlocoID(hitBloco);

                hitBl = b.dono;

               
                if (Input.GetMouseButtonDown(0))
                {
                    b.DestBloco();
                }                    
                else
                {
                    b.ConstBloco(Bloco.tipoTextura.grama);
                }

                    
                    List<string> chunkAtualiza = new List<string>();
                    float chunkX = hitBl.chunk.gameObject.transform.position.x;
                    float chunkY = hitBl.chunk.gameObject.transform.position.y;
                    float chunkZ = hitBl.chunk.gameObject.transform.position.z;

                    if (b.posicao.x == 0)
                    {
                        chunkAtualiza.Add(Mundo.ChunkNome(new Vector3(chunkX - Mundo.chunkTam, chunkY, chunkZ)));
                    }
                    if (b.posicao.x == Mundo.chunkTam - 1)
                    {
                        chunkAtualiza.Add(Mundo.ChunkNome(new Vector3(chunkX + Mundo.chunkTam, chunkY, chunkZ)));
                    }
                    if (b.posicao.y == 0)
                    {
                        chunkAtualiza.Add(Mundo.ChunkNome(new Vector3(chunkX, chunkY - Mundo.chunkTam, chunkZ)));
                    }
                    if (b.posicao.y == Mundo.chunkTam - 1)
                    {
                        chunkAtualiza.Add(Mundo.ChunkNome(new Vector3(chunkX, chunkY + Mundo.chunkTam, chunkZ)));
                    }
                    if (b.posicao.z == 0)
                    {
                        chunkAtualiza.Add(Mundo.ChunkNome(new Vector3(chunkX, chunkY, chunkZ - Mundo.chunkTam)));
                    }
                    if (b.posicao.z == Mundo.chunkTam - 1)
                    {
                        chunkAtualiza.Add(Mundo.ChunkNome(new Vector3(chunkX, chunkY, chunkZ + Mundo.chunkTam)));
                    }

                    foreach (string nome in chunkAtualiza)
                    {
                        Mundo_Chunk c;

                        if (Mundo.chunks.TryGetValue(nome, out c))
                        {

                            c.ReDesenhaBloco();


                        }
                    }


            }
            
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward) * 10;

        Debug.DrawRay(Camera.main.transform.position, forward, Color.green);
    }
}

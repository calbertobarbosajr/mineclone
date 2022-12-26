using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MOVE_PERSONAGEM : MonoBehaviour
{
    public float speed = 5;    
    public Vector3 moveDir = Vector3.zero;
    public CharacterController personagem;
    public float gravidade = -9.8f;    
    public bool noChao;
    public Camera cam;
    public Transform check;
    public LayerMask layer;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(Tempo());
    }

    IEnumerator Tempo()
    {
        yield return new WaitForSeconds(8);
        personagem.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        noChao = Physics.CheckSphere(check.position, 0.1f, layer);

 

        if(noChao)
        {
            var hori = Input.GetAxisRaw("Horizontal");
            var vert = Input.GetAxisRaw("Vertical");

            Vector3 frente = cam.transform.forward;
            Vector3 direita = cam.transform.right;

            frente.Normalize();
            direita.Normalize();

            moveDir = frente * vert + direita * hori;
            moveDir.y = 0;
            moveDir *= speed;


            

        }
        else
        {
            moveDir.y += gravidade * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) && noChao)
        {
            print("pulo");
            moveDir.y = 5;
        }

        personagem.Move(moveDir * Time.deltaTime);

    }


}

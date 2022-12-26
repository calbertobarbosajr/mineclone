﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadCena : MonoBehaviour
{
    private int progresso;



    void Start()
    {
        StartCoroutine(LoadGameProg());
    }


    IEnumerator LoadGameProg()
    {        

        AsyncOperation async = SceneManager.LoadSceneAsync(1,LoadSceneMode.Additive);

        
        while (!async.isDone)
        {
            progresso = (int)(async.progress * 100);
            
            yield return null;
        }


    }
}

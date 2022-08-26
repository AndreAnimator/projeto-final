using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Lista : MonoBehaviour
{
    void Start(){
        List<Mensagens> mensagem = new List<Mensagens>();

        //mensagem.add(new Mensagens("Olá.", 1, "EU"));
        //mensagem.add(new Mensagens("Tudo bem?", 2, "EU"));
        //mensagem.add(new Mensagens("Tchau.", 3, "NAO EH MAIS EU"));

        foreach(Mensagens mm in mensagem){
            print (mm.mensagem + " " + mm.humor);
        }
    }
}

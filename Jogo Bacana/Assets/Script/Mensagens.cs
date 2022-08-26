using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Mensagens : MonoBehaviour
{
    public string mensagem;
    public int humor;
    public string name;

    public Mensagens(string newMensagem, int newHumor, string newName){
        mensagem = newMensagem;
        humor = newHumor;
        name = newName;
    }
}

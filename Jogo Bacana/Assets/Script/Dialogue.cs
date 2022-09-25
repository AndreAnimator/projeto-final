using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue
{
    public GameObject dialogue1;//tem q resolver isso ae
    public GameObject dialogue2;//isso msm arranjar uma forma de referenciar a própria classe
    //cada personagem tem um animator diferente
    public int cena;
    public int[] humor;
    public int[] som;
    public int numEscolhas;
    //adicionei os negocio a mais
    public string[] name;

    [TextArea(3, 10)]
    public string[] sentences;

}

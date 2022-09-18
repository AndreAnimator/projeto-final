using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Animator animator;
    public Animator personagem1;
    public Animator personagem2;//calmala
    public Animator optionChoice;//calmala
    private bool isDialoguing = false;
    private bool isChoosing = false;
    private bool isNotChoosing = false;
    private int escolha = 1;
    private int numEscolha;
    //testando um negocio
    private Dialogue dialogo;

    //FIRST IN F<string>IRST OUT LISTA
    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<int> humores;
    private Queue<int> sounds;
    //comparativos
    private string nomePrevio;
    private string nomeInicial;
    private string nomeAtual;
    //osnomedasmusica
    private string musicaInicial = "Theme";
    private string musicaUm = "Sapo";
    private int cenaAtual;
    //comparativos de musica (int)
    private int musicaIni;
    private int musicaAtual;
    private int musicaPrevia;
    //comparativos de humor
    private int humorInicial;
    private int humorPrevio;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
        humores = new Queue<int>();
        sounds = new Queue<int>();
    }

    void Update()
    {
        if(isDialoguing && Input.GetKeyDown(KeyCode.Space) && !isChoosing)
        {
            DisplayNextSentence(FindObjectOfType<DIalogueTrigger>().dialogue, numEscolha);//mudei aqui
        }
        //tentando fazer o sistema de escolha
        if(isChoosing && Input.GetKeyDown(KeyCode.DownArrow)){
            if(escolha == 1){
                escolha = numEscolha;
                FindObjectOfType<AudioManager>().Play("Cima");
            }
            else{
                escolha--;
                FindObjectOfType<AudioManager>().Play("Baixo");
            }
            Debug.Log(escolha);
            optionChoice.SetInteger("option", escolha);
        }
        if(isChoosing && Input.GetKeyDown(KeyCode.UpArrow)){
            if(escolha == numEscolha){
                escolha = 1;
                FindObjectOfType<AudioManager>().Play("Cima");
            }
            else{
                escolha++;
                FindObjectOfType<AudioManager>().Play("Baixo");
            }
            Debug.Log(escolha);
            optionChoice.SetInteger("option", escolha);
        }
        if(!isDialoguing && isChoosing && Input.GetKeyDown(KeyCode.Space)){
            personagem1.SetBool("isMudarHumor", false);
            personagem2.SetBool("isMudarHumor", false);
            if(escolha == 1){
                StartDialogue(dialogo.dialogue1.GetComponent<DIalogueTrigger>().dialogue);
                isChoosing = false;
            }
            else if(escolha == numEscolha){
                StartDialogue(dialogo.dialogue2.GetComponent<DIalogueTrigger>().dialogue);
                isChoosing = false;
            }
            FindObjectOfType<AudioManager>().Pause("Sapo");
            FindObjectOfType<AudioManager>().Play("Theme");
            Debug.Log(escolha);
            optionChoice.SetBool("isShow", false);
            escolha = 1;
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        cenaAtual = dialogue.cena;
        if(cenaAtual == 0){
            FindObjectOfType<AudioManager>().Pause("Theme");
            FindObjectOfType<AudioManager>().Play("Sapo");
        }
        dialogo = dialogue;
        optionChoice.SetBool("isShow", false);
        animator.SetBool("isOpen", true);
        personagem1 = dialogue.personagem1;
        personagem2 = dialogue.personagem2;
        personagem1.SetBool("isShowing", true);
        personagem2.SetBool("isShowing", false);
        isDialoguing = true;
        //mudei aqui nameText.text = dialogue.name;

        names.Clear();
        sentences.Clear();
        humores.Clear();
        sounds.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach (string name in dialogue.name)
        {
            names.Enqueue(name);
        }
        foreach(int humor in dialogue.humor)
        {
            humores.Enqueue(humor);
        }
        foreach(int sound in dialogue.som)
        {
            sounds.Enqueue(sound);
        }
        humorInicial = dialogue.humor[0];
        humorPrevio = -1;
        nomePrevio = dialogue.name[0];
        nomeInicial = dialogue.name[0];
        nomeAtual = nomeInicial;
        numEscolha = dialogue.numEscolhas;
        DisplayNextSentence(FindObjectOfType<DIalogueTrigger>().dialogue, numEscolha);//mudei aqui
    }

    public void DisplayNextSentence(Dialogue dialogue, int numEscolhas)
    {
        Debug.Log(sentences.Count);
        Debug.Log(numEscolhas);
        if(sentences.Count == 1 && numEscolhas > 0)//mudei aqui
        {
            isNotChoosing = true;
            string name = names.Dequeue();
            nomeAtual = name;
            int humars = humores.Dequeue();
            nameText.text = name;
            int soms = sounds.Dequeue();
            TocarSom(soms);
            DisplayCharacter(name, humars);
            string sentencia = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentencia));
            isChoosing = true;
            isDialoguing = false;
            optionChoice.SetBool("isShow", true);
            optionChoice.SetInteger("option", 1);
        }
        else if(sentences.Count == 0 && !isChoosing){
            if(cenaAtual == 2){
                FindObjectOfType<AudioManager>().Play("Theme");
            }
            EndDialogue();
            return;
        }
        string nome = names.Dequeue();
        nomeAtual = nome;
        int humors = humores.Dequeue();
        int sons = sounds.Dequeue();
        TocarSom(sons);
        nameText.text = nome;
        DisplayCharacter(nome, humors);
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if(cenaAtual == 2 & sentences.Count == 0){
            FindObjectOfType<AudioManager>().Pause("Theme");
        }
    }

    public void DisplayCharacter(string nome, int humors){
        if(humors != humorPrevio){
            personagem1.SetInteger("humor", humors);
            personagem2.SetInteger("humor", humors);
            personagem1.SetBool("isMudarHumor", true);
            personagem2.SetBool("isMudarHumor", true);
            humorPrevio = humors;
        }
        if(nome != nomePrevio  && nome != nomeInicial){
            nomePrevio = nome;
            personagem1.SetBool("isShowing", false);
            personagem2.SetBool("isShowing", true);
            //personagem 2 aparece;
        }
        else if(nome != nomePrevio){
            nomePrevio = nome;
            personagem2.SetBool("isShowing", false);
            personagem1.SetBool("isShowing", true);
            //personagem 1 aparece;
        }
    }

    void Speak(){
        int som;
        if(nomeAtual == "Sapo"){
            Debug.Log("Frog: " + nomeAtual);
            som = Random.Range(0, 3);
            switch(som){
                case 1:
                    Debug.Log("TOCA");
                    FindObjectOfType<AudioManager>().Play("Frog1");
                    break;
                case 2:
                    FindObjectOfType<AudioManager>().Play("Frog2");
                    break;
                default:
                    FindObjectOfType<AudioManager>().Play("Frog1");
                    break;
            }
        }
    }

    void TocarSom(int num){
        switch(num){
            case 1:
                FindObjectOfType<AudioManager>().Play("Laugh");
                break;
            case 2:
                FindObjectOfType<AudioManager>().Play("LaughInverso");
                break;
            default:
                return;
        }
    }

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            Speak();
            yield return new WaitForSeconds(0.05f);
        }
    }

    void EndDialogue()
    {
        isChoosing = false;
        isDialoguing = false;
        animator.SetBool("isOpen", false);
        personagem1.SetBool("isShowing", false);
        personagem2.SetBool("isShowing", false);
    }
}

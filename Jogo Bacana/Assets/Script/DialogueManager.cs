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
    private int escolha = 1;
    private int numEscolha;
    //testando um negocio
    private Dialogue dialogo;

    //FIRST IN F<string>IRST OUT LISTA
    private Queue<string> sentences;
    private Queue<string> names;
    //comparativos
    private string nomePrevio;
    private string nomeInicial;
    //osnomedasmusica
    private string musicaInicial = "Theme";
    private string musicaUm = "Sapo";
    private int cenaAtual;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
        names = new Queue<string>();
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

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        foreach (string name in dialogue.name)
        {
            names.Enqueue(name);
        }
        nomePrevio = dialogue.name[0];
        nomeInicial = dialogue.name[0];
        numEscolha = dialogue.numEscolhas;
        DisplayNextSentence(FindObjectOfType<DIalogueTrigger>().dialogue, numEscolha);//mudei aqui
    }

    public void DisplayNextSentence(Dialogue dialogue, int numEscolhas)
    {
        Debug.Log(sentences.Count);
        Debug.Log(numEscolhas);
        if(sentences.Count == 1 && numEscolhas > 0)//mudei aqui
        {
            string name = names.Dequeue();
            nameText.text = name;
            DisplayCharacter(name);
            string sentencia = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentencia));
            isChoosing = true;
            optionChoice.SetBool("isShow", true);
            optionChoice.SetInteger("option", 1);
            isDialoguing = false;
        }
        else if(sentences.Count == 0 && !isChoosing){
            if(cenaAtual == 2){
                FindObjectOfType<AudioManager>().Play("Theme");
            }
            EndDialogue();
            return;
        }
        string nome = names.Dequeue();
        nameText.text = nome;
        DisplayCharacter(nome);
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        if(cenaAtual == 2 & sentences.Count == 0){
            FindObjectOfType<AudioManager>().Pause("Theme");
        }
    }

    public void DisplayCharacter(string nome){
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

    IEnumerator TypeSentence(string sentence){
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()){
            dialogueText.text += letter;
            yield return null;
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

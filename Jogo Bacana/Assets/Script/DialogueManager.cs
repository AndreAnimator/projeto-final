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
    public Animator personagem;
    public Animator optionChoice;//calmala
    private bool isDialoguing = false;
    private bool isChoosing = false;
    private bool isNotChoosing = false;
    private bool isFinishedTalking = false;
    private int escolha = 1;
    private int numEscolha;
    //testando um negocio
    private Dialogue dialogo;
    private GameObject gameObjeto; //hmmm
    //FIRST IN F<string>IRST OUT LISTA
    private Queue<string> sentences;
    private Queue<string> names;
    private Queue<int> humores;
    private Queue<int> sounds;
    //comparativos
    private string nomePrevio;
    private string nomeAtual;
    private int cenaAtual;
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
        if(isFinishedTalking){
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
                personagem.SetBool("isMudarHumor", false);
                if(escolha == 1){
                    StartDialogue(dialogo.dialogue1.GetComponent<DIalogueTrigger>().dialogue);
                    isChoosing = false;
                }
                else if(escolha == numEscolha){
                    StartDialogue(dialogo.dialogue2.GetComponent<DIalogueTrigger>().dialogue);
                    isChoosing = false;
                }
                Debug.Log("A escolha foi feita");
                Debug.Log(cenaAtual);
                SceneManager(-2);
                Debug.Log(escolha);
                optionChoice.SetBool("isShow", false);
                escolha = 1;
            }
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        cenaAtual = dialogue.cena;
        SceneManager(-1);
        dialogo = dialogue;
        optionChoice.SetBool("isShow", false);
        animator.SetBool("isOpen", true);
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
        nomeAtual = dialogue.name[0];
        if(cenaAtual == 0){
            personagem = findAnimator(nomeAtual);
        }
        DisplayCharacter(nomeAtual, humorInicial);
        numEscolha = dialogue.numEscolhas;
        DisplayNextSentence(FindObjectOfType<DIalogueTrigger>().dialogue, numEscolha);//mudei aqui
    }

    public void DisplayNextSentence(Dialogue dialogue, int numEscolhas)
    {
        isFinishedTalking = false;
        Debug.Log(sentences.Count);
        Debug.Log(numEscolhas);
        if(sentences.Count == 1 && numEscolhas > 0)//mudei aqui
        {
            isNotChoosing = true;
            theRealDisplaySentence();
            isChoosing = true;
            isDialoguing = false;
            optionChoice.SetBool("isShow", true);
            optionChoice.SetInteger("option", 1);
            return;
        }
        else if(sentences.Count == 0 && !isChoosing){
            SceneManager(-3);
            EndDialogue();
            return;
        }
        theRealDisplaySentence();
        SceneManager(sentences.Count);
    }

    public void theRealDisplaySentence(){
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
    }

    public void SceneManager(int count){
        switch(cenaAtual){
            case 0:
                if(count == -1){
                    FindObjectOfType<AudioManager>().Pause("Theme");
                    FindObjectOfType<AudioManager>().Play("Sapo");
                }
            break;
            case 1:
                if(count == 1){
                    FindObjectOfType<AudioManager>().Pause("Sapo");
                    FindObjectOfType<AudioManager>().Play("Theme");
                }
            break;
            case 2:
                if(count == -3){
                    FindObjectOfType<AudioManager>().Play("Theme");
                }
                else if(count == 0){
                    FindObjectOfType<AudioManager>().Pause("Theme");
                }
                else if(count == -2){
                    FindObjectOfType<AudioManager>().Pause("Sapo");
                    FindObjectOfType<AudioManager>().Play("Theme");
                }
            break;
            default:
                Debug.Log("Nao achou cena :(");
            break;
        }
    }

    public void DisplayCharacter(string nome, int humors){
        if(humors != humorPrevio){
            personagem.SetInteger("humor", humors);
            personagem.SetBool("isMudarHumor", true);
            humorPrevio = humors;
        }
        if(nome != nomePrevio){
            personagem.SetBool("isShowing", false);
            personagem = findAnimator(nome);
            personagem.SetBool("isShowing", true);
            nomePrevio = nome;
            //personagem 2 aparece;
        }
    }

    public Animator findAnimator(string name){
        if(name == "Sapo"){
            gameObjeto = GameObject.Find("FROG");
            return gameObjeto.GetComponent<AnimatorManager>().animator;
        }
        if(name == "Fox"){
            gameObjeto = GameObject.Find("FOX");
            return gameObjeto.GetComponent<AnimatorManager>().animator;
        }
        Debug.Log("Nao achou nenhum animador :]");
        return animator;
    }

    void Speak(){
        int som;
        if(nomeAtual == "Sapo"){
            som = Random.Range(0, 3);
            switch(som){
                case 1:
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
        Debug.Log("Terminou de falar");
        isFinishedTalking = true;
    }

    void EndDialogue()
    {
        isChoosing = false;
        isDialoguing = false;
        animator.SetBool("isOpen", false);
        personagem.SetBool("isShowing", false);
    }
}

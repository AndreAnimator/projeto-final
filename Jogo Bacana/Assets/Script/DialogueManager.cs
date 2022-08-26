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
    private bool isDialoguing = false;
    private bool isChoosing = false;
    private int escolha = 1;
    private int numEscolha;
    //testando um negocio
    private Dialogue dialogo;

    //FIRST IN F<string>IRST OUT LISTA
    private Queue<string> sentences;

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
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
            }
            else{
                escolha--;
            }
            Debug.Log(escolha);
        }
        if(isChoosing && Input.GetKeyDown(KeyCode.UpArrow)){
            if(escolha == numEscolha){
                escolha = 1;
            }
            else{
                escolha++;
            }
            Debug.Log(escolha);
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
            Debug.Log(escolha);
        }
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialogo = dialogue;
        animator.SetBool("isOpen", true);
        isDialoguing = true;
        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }
        numEscolha = dialogue.numEscolhas;
        DisplayNextSentence(FindObjectOfType<DIalogueTrigger>().dialogue, numEscolha);//mudei aqui
    }

    public void DisplayNextSentence(Dialogue dialogue, int numEscolhas)
    {
        Debug.Log(sentences.Count);
        Debug.Log(numEscolhas);
        if(sentences.Count == 1 && numEscolhas > 0)//mudei aqui
        {
            string sentencia = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentencia));
            isChoosing = true;
            isDialoguing = false;
        }
        else if(sentences.Count == 0 && !isChoosing){
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
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
    }
}

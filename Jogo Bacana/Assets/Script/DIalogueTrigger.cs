using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DIalogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private Dialogue dialogue1;//mudei pra private, n sei se faz diferença
    private Dialogue dialogue2;

    void Start(){
        dialogue1 = dialogue.dialogue1.GetComponent<DIalogueTrigger>().dialogue;
        dialogue2 = dialogue.dialogue2.GetComponent<DIalogueTrigger>().dialogue;
    }
    public void TriggerDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
    public void TriggerFirstDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue1);
    }
    public void TriggerSecondDialogue ()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue2);
    }
}

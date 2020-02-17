using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject dialogueBox;
    private Text dialogueText;
    
    public string[] praiseSentences;
    public string[] berateSentences;

    private AudioSource audioSource;
    public AudioClip mumble1;
    public AudioClip mumble2;
    public AudioClip mumble3;
    public AudioClip mumble4;

    public int counter = 5;

    // Use this for initialization
    void Awake()
    {
        dialogueText = dialogueBox.transform.GetChild(0).gameObject.GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    public void BeratePlayer()
    {
        if (Random.Range(0, 2) == 0)
        {
            audioSource.clip = mumble3;
        }
        else
        {
            audioSource.clip = mumble4;
        }

        PlayText(berateSentences);
    }

    public void PraisePlayer()
    {
        if (Random.Range(0, 2) == 0)
        {
            audioSource.clip = mumble1;
        }
        else
        {
            audioSource.clip = mumble2;
        }

        PlayText(praiseSentences);
    }

    private void PlayText(string[] sentences)
    {        
        audioSource.Play();
        dialogueBox.SetActive(true);
        int random = Random.Range(0, sentences.Length);
        StartCoroutine(TypeSentence(sentences[random]));
        counter = 5;
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        while (counter > 0)
        {
            yield return new WaitForSeconds(1);
            counter--;
        }
        dialogueBox.SetActive(false);
    }
}

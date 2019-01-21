using UnityEngine;
using System.Collections;
using KModkit;
using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;
public class alphabeticOrderScript : MonoBehaviour
{

    public KMSelectable[] keypad;
    public KMAudio audio;
    public KMNeedyModule needy;
    private string[] letters = new string[4];
    private int currentInt;
    private bool active;
    private string[] alphabet = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
    void Awake()
    {


        GetComponent<KMNeedyModule>().OnNeedyActivation += OnNeedyActivation;
        GetComponent<KMNeedyModule>().OnNeedyDeactivation += OnNeedyDeactivation;

        GetComponent<KMNeedyModule>().OnTimerExpired += OnTimerExpired;
        foreach (KMSelectable key in keypad)
        {
            KMSelectable pressedKey = key;
            key.OnInteract += delegate () { keyPressed(pressedKey); return false; };
        }
    }

    protected bool Solve()
    {
        active = false;
        GetComponent<KMNeedyModule>().OnPass();

        return false;
    }

    protected void OnNeedyActivation()
    {
        active = true;
        for (int i = 0; i < 4; i++)
        {
            letters[i] = alphabet[UnityEngine.Random.Range(0, 26)];
        }
        for (int i = 0; i < 4; i++)
        {
            keypad[i].GetComponentInChildren<TextMesh>().text = letters[i];
        }
        
        Array.Sort(letters);
        

        
            
        

    }

    protected void OnNeedyDeactivation()
    {
        active = false;
    }

    protected void OnTimerExpired()
    {
        GetComponent<KMNeedyModule>().OnStrike();
    }
    void oof()
    {
        
    }
    void keyPressed(KMSelectable pressedKey)
    {
        if (active)
        {
            Debug.Log("pressedKey");
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
            if (pressedKey.GetComponentInChildren<TextMesh>().text == letters[currentInt])
            {
                currentInt++;
            }
            else
            {
                OnTimerExpired();
                currentInt = 0;
                audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
            }
            if (currentInt == 4)
            {
                currentInt = 0;
                needy.HandlePass();
            }
        }
        
    }



}
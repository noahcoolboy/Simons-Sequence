using UnityEngine;
using System.Collections;
using KModkit;
using System.Linq;
using System;
using System.Text;
using System.Collections.Generic;
public class ExampleNeedyModule : MonoBehaviour
{
    public KMNeedyModule needy;
    public KMSelectable[] keypad;
    private string[] letters = new string[4];
    private int[] sort = new int[4];
    private int currentInt;
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
        GetComponent<KMNeedyModule>().OnPass();

        return false;
    }

    protected void OnNeedyActivation()
    {

        for (int i = 0; i < 4; i++)
        {
            letters[i] = alphabet[UnityEngine.Random.Range(0, 26)];
        }
        for (int i = 0; i < 4; i++)
        {
            keypad[i].GetComponentInChildren<TextMesh>().text = letters[i];
        }
        for (int i = 0; i < 4; i++)
        {
            sort[i] = Array.IndexOf(alphabet, letters[i]);
        }
       Array.Sort(sort);
        
        
        foreach (string letter in letters)
        {
            Debug.Log(sort);
        }

    }

    protected void OnNeedyDeactivation()
    {
        
    }

    protected void OnTimerExpired()
    {
        GetComponent<KMNeedyModule>().OnStrike();
    }
    void keyPressed(KMSelectable pressedKey)
    {

        
    }



}
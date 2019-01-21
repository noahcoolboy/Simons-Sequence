using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KModkit;
using System;
public class simonRemembersScript : MonoBehaviour {
    public KMSelectable[] buttons;
    public KMAudio audio;
    public Light[] lights;
    List<int> sequence = new List<int>();
    public KMNeedyModule needy;
    private int currentInt;
    private int random;
    private int addseconds = 0;
    private int lenght2;
	// Use this for initialization
	void Start () {
        
        needy.OnNeedyActivation += activation;
        needy.OnTimerExpired += expired;
        lights[0].enabled = false;
        lights[1].enabled = false;
        lights[2].enabled = false;
        lights[3].enabled = false;
        foreach  (KMSelectable key in buttons)
        {
            key.OnInteract += delegate () { bp(key); return false; };
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
    void activation()
    {
        needy.CountdownTime = 35 + addseconds;
        random = UnityEngine.Random.Range(0, 4);
        lights[random].enabled = true;
        sequence.Add(random);
        lenght2++;
        currentInt = 0;

    }
    void expired()
    {
        lights[random].enabled = false;

        sequence.Clear();
        lenght2 = 0;
        currentInt = 0;
        addseconds = 0;
        needy.HandleStrike();
    }
    void bp(KMSelectable key)
    {
        
            audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);
        
        if (Array.IndexOf(buttons,key) == sequence[currentInt])
        {
            currentInt++;
            
            
            
        }
        else
        {
            needy.OnStrike();
            currentInt = 0;
        }
        if (currentInt == lenght2)
        {
            needy.OnPass();
            lights[random].enabled = false;
            addseconds += 3;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

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
    static int moduleIdCounter = 1;
    int moduleId;
    // Use this for initialization
    void Start ()
    {
        moduleId = moduleIdCounter++;
        needy.OnNeedyActivation += activation;
        needy.OnTimerExpired += expired;
        lights[0].enabled = false;
        lights[1].enabled = false;
        lights[2].enabled = false;
        lights[3].enabled = false;
        float scalar = transform.lossyScale.x;
        for (var i = 0; i < lights.Length; i++)
            lights[i].range *= scalar;
        foreach  (KMSelectable key in buttons)
        {
            key.OnInteract += delegate () { bp(key); return false; };
        }
    }
    void activation()
    {
        needy.CountdownTime = 35 + addseconds;
        random = UnityEngine.Random.Range(0, 4);
        lights[random].enabled = true;
        sequence.Add(random);
        Debug.LogFormat("[Simon's Sequence #{0}] Sequence: {1}", moduleId, sequence.Select(x => "BYRG"[x]).Join(""));
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
        Debug.LogFormat("[Simon's Sequence #{0}] The counter expired before the sequence was received", moduleId);
    }
    void bp(KMSelectable key)
    {
        audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.ButtonPress, transform);

        if (lights.All(x => !x.enabled)) return;

        if (Array.IndexOf(buttons,key) == sequence[currentInt])
        {
            currentInt++;
        }
        else
        {
            needy.OnStrike();
            Debug.LogFormat("[Simon's Sequence #{0}] Press #{1} was incorrect, received {2} but expected {3}", moduleId, currentInt + 1, "BYRG"[Array.IndexOf(buttons, key)], "BYRG"[sequence[currentInt]]);
            currentInt = 0;
        }
        if (currentInt == lenght2)
        {
            needy.OnPass();
            Debug.LogFormat("[Simon's Sequence #{0}] Sequence received", moduleId);
            lights[random].enabled = false;
            addseconds += 3;
        }
    }

    //twitch plays
    #pragma warning disable 414
    private readonly string TwitchHelpMessage = @"!{0} BRYG [Presses the buttons blue, red, yellow, and green in that order]";
    #pragma warning restore 414
    IEnumerator ProcessTwitchCommand(string command)
    {
        command = command.Replace(" ", "");
        string lower = command.ToLowerInvariant();
        for (int i = 0; i < command.Length; i++)
        {
            if (!"byrg".Contains(lower[i]))
            {
                yield return "sendtochaterror!f The specified button '" + command[i] + "' is invalid!";
                yield break;
            }
        }
        if (lights.All(x => !x.enabled))
        {
            yield return "sendtochaterror Buttons cannot be pressed right now!";
            yield break;
        }
        yield return null;
        for (int i = 0; i < command.Length; i++)
        {
            buttons["byrg".IndexOf(lower[i])].OnInteract();
            yield return new WaitForSeconds(.1f);
        }
    }

    void TwitchHandleForcedSolve()
    {
        StartCoroutine(DealWithNeedy());
    }

    private IEnumerator DealWithNeedy()
    {
        while (true)
        {
            if (lights.Any(x => x.enabled))
            {
                buttons[sequence[currentInt]].OnInteract();
                yield return new WaitForSeconds(.1f);
            }
            else
                yield return null;
        }
    }
}

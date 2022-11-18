using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue
{
    public string[] sentences;

    public Dialogue(string text) {
        sentences = text.Split("\n");
    }
}

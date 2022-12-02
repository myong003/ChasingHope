using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "New CharacterExpression", menuName = "CharacterExpression")]
public class DialogueCharacter : ScriptableObject
{
    public string characterName;
    public string expression;
    public Sprite sprite;
}

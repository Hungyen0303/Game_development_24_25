using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Dialog 
{   
    private string name;
    private List<Node> lines;
    public List<Node> Lines{
        get{return lines;}
        set{lines=value;}
        }
    public string Name{
        set{name=value;}
        get{return name;}
    }
}


public abstract class Node
{
    public string Content { get; private set; }

    protected Node(string content)
    {
        Content = content;
    }
}

public class DialogueNode : Node
{
    public DialogueNode(string content) : base(content) { }
}


public class ChoiceNode : Node
{
    public List<(string ChoiceText, System.Action OnSelect)> Choices { get; private set; }

    public ChoiceNode(string content, List<(string, System.Action)> choices) : base(content)
    {
        Choices = choices;
    }
}


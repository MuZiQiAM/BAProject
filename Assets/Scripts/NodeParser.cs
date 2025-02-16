using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using XNode;

public class NodePhaser : MonoBehaviour
{
    public DialogueGraph graph;
    private Coroutine _parser;

    public TextMeshProUGUI speaker;
    public TextMeshProUGUI dialogue;
    public Image speakerImage;

    //find the first Node
    //running through the code inside.
    private void Start()
    {
        //find the first Node
        foreach (BaseNode b in graph.nodes)
        {
            if (b.GetString() == "Start")
            {
                Debug.Log(" started");
                graph.current = b;
                break;
            }
        }

        _parser = StartCoroutine(ParseNode());
    }

    IEnumerator ParseNode()
    {
        BaseNode b = graph.current;
        string data = b.GetString();
        string[] dataParts = data.Split('/');
        if (dataParts[0] == "Start")
        {
            Debug.Log(" started");
            NextNode("exit");
        }

        if (dataParts[0] == "DialogueNode")
        {
            speaker.text = dataParts[1];
            dialogue.text = dataParts[2];
            speakerImage.sprite = b.GetSprite();
            Debug.Log("mouse pressed");
            yield return new WaitUntil(() => Input.GetButtonDown("Jump"));
            Debug.Log("jump pressed");
            //yield return new WaitUntil(() => Input.GetMouseButtonUp();
            NextNode("exit");
        }
    }

    public void NextNode(string fieldName)
    {
        if (_parser != null)
        {
            StopCoroutine(_parser);
            _parser = null;
        }

        foreach (NodePort port in graph.current.Ports)
        {
            if (port.fieldName == fieldName)
            {
                graph.current = port.Connection.node as BaseNode;
                break;
            }
        }

        _parser = StartCoroutine(ParseNode());
    }
}
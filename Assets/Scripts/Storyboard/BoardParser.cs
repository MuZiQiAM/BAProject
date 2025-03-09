using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class BoardParser : MonoBehaviour
{
    /*
    [SerializeField] private StoryboardGraph _storyboardGraph;

    public Camera selectedCamera;

    public static int currentId;

    public List<BaseNode> cameraTransitionList;


    private void Start()
    {
        cameraTransitionList.Clear();
        ParseNode(GetFirstNodeOf(typeof(StartStoryNode1)));
        foreach (var VARIABLE in cameraTransitionList)
        {
            Debug.Log(VARIABLE.name + " " + VARIABLE.id);
        }
    }

    public BaseNode GetFirstNodeOf(Type type)
    {
        foreach (var node1 in _storyboardGraph.nodes)
        {
            var node = (BaseNode)node1;
            if (node.GetType() == type)
                return _storyboardGraph.currentNode = node;
        }

        Debug.LogError("NO Node with nodeType " + type + " was found");
        return null;
    }

    public void ParseDecisionNode(DecisionNode node)
    {
        node.id = cameraTransitionList.Count;
        cameraTransitionList.Add(node);
        List<BaseNode> exits = node.GetAllConnectedNodes<BaseNode>("exit");
        foreach (var VARIABLE in exits)
        {
            ParseNode(VARIABLE);
        }

        Debug.Log(node.name);
    }

    public void ParseNode(BaseNode node)
    {
        if (node == null)
        {
            Debug.LogError("There is no first node!");
            return;
        }

        Debug.Log(node.GetNodeType());
        switch (node.GetNodeType())
        {
            case NodeTypes.START_NODE:
                selectedCamera = ((StartStoryNode1)node).selectedCamera;
                break;
            case NodeTypes.BOARD_NODE:
                //((BoardNode)node).selectedCamera = selectedCamera;
                break;
            case NodeTypes.END_NODE:
                return;
            case NodeTypes.DECISION_NODE:
                //((DecisionNode)node).selectedCamera = selectedCamera;
                ParseDecisionNode((DecisionNode)node);
                break;
            default:
                Debug.LogError("NO NODE ASSIGNED");
                break;
        }

        if (node.GetType() == typeof(EndStoryNode))
        {
            return;
        }
        else
        {
            if (node.GetType() != typeof(DecisionNode))
            {
                node.id = cameraTransitionList.Count;
                cameraTransitionList.Add(node);
            }

            if (node.GetNextNode() != null)
            {
                ParseNode(node.GetNextNode());
            }
        }
    }
    */
}
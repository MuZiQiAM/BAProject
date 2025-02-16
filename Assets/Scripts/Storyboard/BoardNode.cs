using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using XNode;

[Serializable]
[NodeTint("0000FF")]
[NodeWidth(400)]
public class BoardNode : BaseNode
{
    [Input] public int entry;
    [Output] public int exit;

    public string scene;
    public Sprite storyboard;

    public GizmoObject gizmoReference;
    
    
    public override Sprite GetSprite()
    {
        return storyboard;
    }

    private void setScene(string newName)
    {
        scene = newName;
    }

    
}
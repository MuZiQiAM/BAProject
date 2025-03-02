using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNodeEditor;
using Image = Microsoft.Unity.VisualStudio.Editor.Image;

[CustomNodeEditor(typeof(DecisionNode))]
public class DecisionNodeDrawer : NodeEditor
{
    private DecisionNode _decisionNode;
    
}
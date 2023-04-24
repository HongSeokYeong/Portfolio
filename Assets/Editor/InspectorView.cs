using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

// �����̺��Ʈ���� ���� ���ο� �並 �߰��ϰ� ������ �̷��� ���ο� �� Ŭ������ ���� �߰����ش�.
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> { }

    Editor editor;

    public InspectorView()
    {

    }

    internal void UpdateSelection(SerializedBehaviourTree serializer, NodeView nodeView)
    {
        Clear();

        if (nodeView == null)
        {
            return;
        }

        var nodeProperty = serializer.FindNode(serializer.Nodes, nodeView.node);
        if (nodeProperty == null)
        {
            return;
        }

        // Auto-expand the property
        // TODO : isExpanded ã�ƺ���
        nodeProperty.isExpanded = true;

        // Property field
        var field = new PropertyField();
        field.label = nodeProperty.managedReferenceValue.GetType().ToString();
        field.BindProperty(nodeProperty);
        Add(field);

        //Object.DestroyImmediate(editor);
        //editor = Editor.CreateEditor(nodeView.node);
        //var container = new IMGUIContainer(() => 
        //{ 
        //    if (editor.target)
        //    {
        //        editor.OnInspectorGUI();
        //    }
        //});

        //Add(container);
    }
}

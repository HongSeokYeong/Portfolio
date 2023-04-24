using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> onNodeSelected;
    public SerializedBehaviourTree btSerializer;
    public Node node;
    public Port input;
    public Port output;

    public NodeView(SerializedBehaviourTree tree, Node node) : base("Assets/Editor/NodeView.uxml")
    {
        this.btSerializer = tree;
        this.node = node;
        this.title = node.GetType().Name;
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top = node.position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();
        SetupDataBinding();

        //Label descriptionLabel = this.Q<Label>("description");
        //descriptionLabel.bindingPath = "description";
        //descriptionLabel.Bind(new SerializedObject(node));
    }

    private void SetupDataBinding()
    {
        var nodeProperty = btSerializer.FindNode(btSerializer.Nodes, node);
        var descriptionProperty = nodeProperty.FindPropertyRelative("description");
        Label descriptionLabel = this.Q<Label>("description");
        descriptionLabel.BindProperty(descriptionProperty);
    }

    private void SetupClasses()
    {
        if (node is ActionNode)
        {
            AddToClassList("Action");
        }
        else if (node is CompositeNode)
        {
            AddToClassList("Composite");
        }
        else if (node is DecoratorNode)
        {
            AddToClassList("Decorator");
        }
        else if (node is RootNode)
        {
            AddToClassList("Root");
        }
    }

    private void CreateInputPorts()
    {
        if (node is ActionNode)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is CompositeNode)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            input = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {

        }

        if (input != null)
        {
            input.portName = "";
            input.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(input);
        }
    }

    private void CreateOutputPorts()
    {
        if (node is ActionNode)
        {
        }
        else if (node is CompositeNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (node is DecoratorNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (node is RootNode)
        {
            output = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }

        if (output != null)
        {
            output.portName = "";
            output.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(output);
        }
    }

    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Vector2 position = new Vector2(newPos.xMin, newPos.yMin);
        btSerializer.SetNodePosition(node, position);

        //Undo.RecordObject(node, "Behaviour Tree (Set Position)");
        //node.position.x = newPos.xMin;
        //node.position.y = newPos.yMin;
        //EditorUtility.SetDirty(node);
    }

    public override void OnSelected()
    {
        base.OnSelected();

        if (onNodeSelected != null)
        {
            onNodeSelected.Invoke(this);
        }
    }

    public void SortChildren()
    {
        if (node is CompositeNode composite)
        {
            composite.childNodeList.Sort(SotyByHorizontalPosition);
        }
    }

    private int SotyByHorizontalPosition(Node left, Node right)
    {
        return left.position.x < right.position.x ? -1 : 1;
    }

    public void UpdateState()
    {
        RemoveFromClassList("Success");
        RemoveFromClassList("Failure");
        RemoveFromClassList("Running");

        if (Application.isPlaying)
        {
            switch (node.state)
            {
                case E_State.Success:
                    AddToClassList("Success");
                    break;
                case E_State.Failure:
                    AddToClassList("Failure");
                    break;
                case E_State.Running:
                    if (node.started)
                    {
                        AddToClassList("Running");
                    }
                    break;
                default:
                    break;
            }
        }
    }
}

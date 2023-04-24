using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using System;
using System.Linq;
using UnityEngine;

// 노드들을 그릴 뷰
public class BehaviourTreeView : GraphView
{
    // TODO : class에 new 키워드의 의미에 대하여 찾아보기
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> { }

    public Action<NodeView> onNodeSelected;
    SerializedBehaviourTree serializer;

    public struct ScriptTemplate
    {
        public TextAsset templateFile;
        public string defaultFileName;
        public string subFolder;
    }

    public ScriptTemplate[] scriptFileAssets =
        {
            //new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().scriptTemplateActionNode, defaultFileName="NewActionNode.cs", subFolder="Actions" },
            //new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().scriptTemplateCompositeNode, defaultFileName="NewCompositeNode.cs", subFolder="Composites" },
            //new ScriptTemplate{ templateFile=BehaviourTreeSettings.GetOrCreateSettings().scriptTemplateDecoratorNode, defaultFileName="NewDecoratorNode.cs", subFolder="Decorators" },
        };

    public BehaviourTreeView()
    {
        Insert(0, new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/BehaviourTree_Editor.uss");
        styleSheets.Add(styleSheet);

        //Undo.undoRedoPerformed += OnUndoRedo;
        viewTransformChanged += OnViewTransformChanged;
    }

    private void OnViewTransformChanged(GraphView graphView)
    {
        Vector3 position = contentViewContainer.transform.position;
        Vector3 scale = contentViewContainer.transform.scale;
        serializer.SetViewTransform(position, scale);
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.guid) as NodeView;
    }

    public void ClearView()
    {
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements.ToList());
        graphViewChanged += OnGraphViewChanged;
    }

    internal void PopulateView(SerializedBehaviourTree tree)
    {
        serializer = tree;

        ClearView();

        // 노드뷰 생성
        serializer.tree.nodes.ForEach(n => CreateNodeView(n));

        // 엣지 생성
        serializer.tree.nodes.ForEach(n =>
        {
            var children = serializer.tree.GetChildren(n);

            children.ForEach(m =>
            {
                var parentView = FindNodeView(n);
                var childView = FindNodeView(m);

                var edge = parentView.output.ConnectTo(childView.input);
                AddElement(edge);
            });
        });

        //contentViewContainer.transform.position = serializer.tree.viewPosition;
        //contentViewContainer.transform.scale = serializer.tree.viewScale;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
    {
        if (graphViewChange.elementsToRemove != null)
        {
            graphViewChange.elementsToRemove.ForEach(x =>
            {
                var nodeView = x as NodeView;

                if (nodeView != null)
                {
                    serializer.DeleteNode(nodeView.node);
                    onNodeSelected(null);
                }

                var edge = x as Edge;
                if (edge != null)
                {
                    var parentView = edge.output.node as NodeView;
                    var childView = edge.input.node as NodeView;
                    serializer.RemoveChild(parentView.node, childView.node);
                }
            });
        }

        if (graphViewChange.edgesToCreate != null)
        {
            graphViewChange.edgesToCreate.ForEach(edge =>
            {
                var parentView = edge.output.node as NodeView;
                var childView = edge.input.node as NodeView;
                serializer.AddChild(parentView.node, childView.node);
            });
        }

        if (graphViewChange.movedElements != null)
        {
            nodes.ForEach(n =>
            {
                var view = n as NodeView;
                view.SortChildren();
            });
        }

        return graphViewChange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        //base.BuildContextualMenu(evt);

        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);

        // TODO : GetTypesDerivedFrom 함수에 대해서
        var actionTypes = TypeCache.GetTypesDerivedFrom<ActionNode>();
        foreach (var item in actionTypes)
        {
            evt.menu.AppendAction($"[Action]/{item.Name}", x => CreateNode(item, nodePosition));
        }

        var compositeTypes = TypeCache.GetTypesDerivedFrom<CompositeNode>();
        foreach (var item in compositeTypes)
        {
            evt.menu.AppendAction($"[Composite]/{item.Name}", x => CreateNode(item, nodePosition));
        }

        var decoratorTypes = TypeCache.GetTypesDerivedFrom<DecoratorNode>();
        foreach (var item in decoratorTypes)
        {
            evt.menu.AppendAction($"[Decorator]/{item.Name}", x => CreateNode(item, nodePosition));
        }
    }

    void CreateNode(Type type, Vector2 position)
    {
        var node = serializer.CreateNode(type, position);
        CreateNodeView(node);
    }

    void CreateNodeView(Node node)
    {
        var nodeView = new NodeView(serializer, node);
        nodeView.onNodeSelected = onNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            var view = n as NodeView;
            view.UpdateState();
        });
    }
}

using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;


public class KeyElement : VisualElement
{
    public new class UxmlFactory : UxmlFactory<KeyElement, UxmlTraits> { }

    private List<ListItem> list;
    private ScrollView container;

    public KeyElement()
    {
        AddToClassList("list-element");

        list = new List<ListItem>();
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/KeyElement.uss");
        styleSheets.Add(styleSheet);
    }

    public void Initialize()
    {
        Clear();

        container = new ScrollView(ScrollViewMode.Vertical);
        Add(container);

        list.Sort(ItemCompare);

        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];

            Label element = new Label(item.GetName());
            element.AddToClassList("list-element-item");
            element.tooltip = item.GetTooltip();
            element.userData = i;
            element.focusable = true;
            element.RegisterCallback<ClickEvent>(OnClickEvent);

            var icon = new VisualElement();
            icon.AddToClassList("list-element-item-icon");
            icon.style.backgroundColor = new StyleColor(item.GetColor());

            element.Add(icon);

            container.Add(element);
        }
    }

    public void AddItem(ListItem item)
    {
        list.Add(item);
    }

    public void ClearItems()
    {
        list.Clear();
    }

    private void OnClickEvent(ClickEvent evt)
    {
        if (evt.propagationPhase != PropagationPhase.AtTarget)
        {
            return;
        }

        var element = evt.target as VisualElement;
        int index = (int)element.userData;
        list[index].OnClick?.Invoke(element);
    }

    private int ItemCompare(ListItem lhs, ListItem rhs)
    {
        int compare = lhs.GetName().CompareTo(rhs.GetName());

        return compare;
    }
}

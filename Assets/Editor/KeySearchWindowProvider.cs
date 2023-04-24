using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class KeySearchWindowProvider : ScriptableObject, ISearchWindowProvider
{
    private struct KeyEntry
    {
        public GUIContent content;
        public object data;
        public Action<object> onSelect;

        public KeyEntry(GUIContent content, object data, Action<object> onSelect)
        {
            this.content = content;
            this.data = data;
            this.onSelect = onSelect;
        }
    }

    private List<KeyEntry> entries = new List<KeyEntry>();

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        entries.Sort(SortEntriesByGroup);

        List<SearchTreeEntry> treeEntries = new List<SearchTreeEntry>();

        treeEntries.Add(new SearchTreeGroupEntry( new GUIContent("Keys"), 0));

        List<string> groups = new List<string>();

        for (int i = 0; i < entries.Count; i++)
        {
            KeyEntry keyEntry = entries[i];

            string group = string.Empty;
            string[] paths = keyEntry.content.text.Split('/');
            int length = paths.Length - 1;

            for (int j = 0; j < length; j++)
            {
                string path = paths[j];

                group += path;
                if (!groups.Contains(group))
                {
                    treeEntries.Add(new SearchTreeGroupEntry(new GUIContent(path), j + 1));
                    groups.Add(group);
                }

                group += "/";
            }

            keyEntry.content.text = paths[length];
            SearchTreeEntry searchTreeEntry = new SearchTreeEntry(keyEntry.content);
            searchTreeEntry.userData = i;
            searchTreeEntry.level = paths.Length;
            treeEntries.Add(searchTreeEntry);
        }

        return treeEntries;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        KeyEntry keyEntry = entries[(int)SearchTreeEntry.userData];

        if (keyEntry.onSelect != null)
        {
            keyEntry.onSelect?.Invoke(keyEntry.data);
            return true;
        }

        return false;
    }

    private int SortEntriesByGroup(KeyEntry lhs, KeyEntry rhs)
    {
        string[] lhsPaths = lhs.content.text.Split('/');
        string[] rhsPaths = rhs.content.text.Split('/');

        int lhsLength = lhsPaths.Length;
        int rhsLength = rhsPaths.Length;
        int minLength = Mathf.Min(lhsLength, rhsLength);

        for (int i = 0; i < minLength; i++)
        {
            if (minLength - 1 == i)
            {
                int compareDepth = rhsLength.CompareTo(lhsLength);
                if (compareDepth != 0)
                {
                    return compareDepth;
                }
            }

            int compareText = lhsPaths[i].CompareTo(rhsPaths[i]);
            if (compareText != 0)
            {
                return compareText;
            }
        }

        return 0;
    }

    public void AddEntry(GUIContent content, object data, Action<object> onSelect)
    {
        entries.Add(new KeyEntry(content, data, onSelect));
    }

    public void AddEntry(GUIContent content, Action onSelect)
    {
        entries.Add(new KeyEntry(content, null, (data) => onSelect?.Invoke()));
    }

    public void AddEntry(string name, object data, Action<object> onSelect)
    {
        AddEntry(new GUIContent(name), data, onSelect);
    }

    public void AddEntry(string name, Action onSelect)
    {
        AddEntry(new GUIContent(name), null, (data) => onSelect?.Invoke());
    }

    public void Open(Vector2 position, float width = 0, float height = 0)
    {
        SearchWindow.Open(new SearchWindowContext(position, width, height), this);
    }

    public void Open(float width = 0, float height = 0)
    {
        Vector2 position = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
        Open(position, width, height);
    }

    public void Open(Rect buttonRect, float width = 0, float height = 0)
    {
        Rect screenRect = GUIUtility.GUIToScreenRect(buttonRect);

        Vector2 position = screenRect.position;
        position.x += screenRect.width / 2;
        position.y += screenRect.height + 15;

        width = Mathf.Max(0, width);
        width = width != 0 ? width : screenRect.width;

        Open(position, width != 0 ? width : screenRect.width, height);
    }

    public static KeySearchWindowProvider Create()
    {
        KeySearchWindowProvider window = CreateInstance<KeySearchWindowProvider>();
        return window;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourController : MonoBehaviour
{
    public BehaviourTree behaviourTree;

    // Storage container object to hold game object subsystems
    Context context;

    // Start is called before the first frame update
    void Start()
    {
        context = CreateBehaviourTreeContext();
        behaviourTree = behaviourTree.Clone();
        behaviourTree.Bind(context);
    }

    // Update is called once per frame
    void Update()
    {
        if (behaviourTree)
        {
            behaviourTree.BehaviourTreeUpdate();
        }
    }

    Context CreateBehaviourTreeContext()
    {
        return Context.CreateFromGameObject(gameObject);
    }
}

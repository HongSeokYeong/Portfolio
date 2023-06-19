using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourTreeController : MonoBehaviour
{
    public BehaviourTree behaviourTree;

    public Animator animator;

    public NavMeshAgent agent;

    // Storage container object to hold game object subsystems
    //Context context;

    // Start is called before the first frame update
    void Start()
    {
        //context = CreateBehaviourTreeContext();
        behaviourTree = behaviourTree.Clone();
        behaviourTree.Bind(null, this);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerNavMash : MonoBehaviour
{
    public Camera cam;

    public NavMeshAgent navMeshAgent;
    //[SerializeField] private Transform movePositionTransform;

    private void Awake()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        //navMeshAgent.destination = movePositionTransform.position;
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition); ;
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) 
            {
                navMeshAgent.SetDestination(hit.point);
            }
        }
    }

}

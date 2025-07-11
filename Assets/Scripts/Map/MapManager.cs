using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngineInternal;

public class MapManager : MonoBehaviour
{
    public MapNode initialMapNode;
    public MapNode occupiedMapNode;
    
    [SerializeField] private bool doVisitOnce = false; // enables/disables nodes not being reachable after visiting them once
    
    private bool inTransit = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TryTravelToNode(initialMapNode);
    }

    private void OnEnable()
    {
        MapNode.OnNodeClickedEvent += TryTravelToNode;
        MapNode.OnNodeHoverEvent += OnNodeHover;
        MapNode.OnNodeUnhoverEvent += OnNodeUnhover;
    }

    private void OnDisable()
    {
        MapNode.OnNodeClickedEvent -= TryTravelToNode;
        MapNode.OnNodeHoverEvent += OnNodeHover;
        MapNode.OnNodeUnhoverEvent += OnNodeUnhover;
    }

    private void OnNodeHover(MapNode n)
    {
        // check if the one we're hovering over is connected to the currently occupied node:
        if (occupiedMapNode != null) // it shouldn't ever be null when this function is called but just in case
        {
            if (occupiedMapNode.HasConnection(n))
            {
                var conn = occupiedMapNode.GetConnection(n);
                conn.SetHighlighted(true);
            }
        }
    }

    private void OnNodeUnhover(MapNode n)
    {
        if (occupiedMapNode != null)
        {
            if (occupiedMapNode.HasConnection(n))
            {
                var conn = occupiedMapNode.GetConnection(n);
                conn.SetHighlighted(false);
            }
        }
    }

    // function called when we click on any node, to see if we can travel to it.
    private void TryTravelToNode(MapNode n)
    {
        if (inTransit) return; // don't do anything if we are currently traveling to a node
        
        // have we visited this node once already and is visitOnce enabled?
        if (doVisitOnce && n.IsVisited)
        {
            Debug.Log("Selected node was already visited, not traveling anywhere!");
            return;
        }

        // if its the initial node we start in (aka if TryTravelToNode is called with initialMapNode as its argument)
        if (occupiedMapNode == null)
        {
            occupiedMapNode = n;
            occupiedMapNode.TravelTo();
        }
        
        // cases where we actually travel
        else
        {
            if (occupiedMapNode.HasConnection(n)) // have a connection to the node
            {
                occupiedMapNode.OnLeaveNode();
                var conn = occupiedMapNode.GetConnection(n); // get the connector so we can activate its travel anim
                conn.StartTravelAnimation(occupiedMapNode, OnFinishTraveling);
                
                occupiedMapNode = n;
                inTransit = true;
            }
            else // only get here if the clicked node is unreachable from the current node
            {
                Debug.Log("selected node is unreachable from the current node!");
            }
        }
    }

    private void OnFinishTraveling(MapNode n)
    {
        occupiedMapNode.TravelTo(); // meowwwwy
        inTransit = false;
    }
}

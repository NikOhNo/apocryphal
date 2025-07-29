using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MapNode : MonoBehaviour
{
    [Tooltip("Data/options for this node! Create a MapNodeData scriptable object to put here")] public SO_MapNodeData mapNodeData;
    
    [SerializeField] private Sprite sprite;
    [SerializeField] private Color normalColor; // (sprite overlay) color when not being highlighted and not occupied
    [SerializeField] private Color highlightColor; // color for mouse highlight
    [SerializeField] private Color occupiedColor; // color when Occupied
    
    [SerializeField] private List<MapNode> nodeConnections;
    [SerializeField] private List<MapNodeConnector> connectors;
    
    [SerializeField] private GameObject nodeConnectorPrefab;
    
    public string message;
    
    public static event Action<MapNode> OnNodeClickedEvent; // called when this node is CLICKED
    public static event Action<MapNode> OnNodeHoverEvent; // called when the mouse HOVERS over this node
    public static event Action<MapNode> OnNodeUnhoverEvent; // called when the mouse STOPS HOVERING over this node
    
    public event Action OnEncounterStartEvent; // note: not static
    public event Action OnEncounterEndEvent;
    
    
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer occIndSpriteRenderer; // occupiedIndicatorSpriteRenderer was just too long
    private bool occupied = false;
    private bool visited = false; // true if we visited this node already
    
   
    
    public bool IsVisited => visited;
    
    // [SerializeField] [SerializedDictionary("MapNode", "MapNodeConnector")] SerializedDictionary<MapNode, MapNodeConnector> connectionDict = new(); // for keeping track of created MapNodeConnectors.
    // could alternatively make this on the MapManager so that each node doesn't have an entire dictionary. although these
    // dicts are going to be only 2 or 3 entries long, so it won't be that much more space complexity overhead to store one 
    // dict per node versus one large dict on the mapmanager. worth considering. 

    // [SerializeField] MapNode[] connectedNodes = Array.Empty<MapNode>();
    // [SerializeField] MapNodeConnector[] nodeConnectors = Array.Empty<MapNodeConnector>(); 
    
    void Awake()
    {
        // sprite is a child so it can have a separate transform/scale
        var baseSpriteObj = transform.Find("BaseSprite");
        spriteRenderer = baseSpriteObj.GetComponent<SpriteRenderer>();
        var occupiedIndicator = transform.Find("OccupiedIndicator");
        occIndSpriteRenderer = occupiedIndicator.GetComponent<SpriteRenderer>();
    }
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (mapNodeData != null)
        {
            // normalColor = mapNodeData.unselectedColor;
            // highlightColor = mapNodeData.selectedColor;
            // occupiedColor = mapNodeData.selectedColor; // aaahh probably do like a transparent circle sprite around it like in slay the spire????

            // spriteRenderer.sprite =
            //     mapNodeData.baseSprite != null ? mapNodeData.baseSprite : spriteRenderer.sprite; // evil line of code
        }
        
        spriteRenderer.sprite = sprite;

        if (nodeConnections != null)
        {
            foreach (MapNode n in nodeConnections)
            {
                // instantiate a MapNodeConnector for every connection. 
                // if the connection ALREADY has a connector to THIS NODE, then we don't create one.
                // this is kind of a messy spaghetti way of doing it BUT it's really easy. and simple. 
                // i don't think it will become an issue later :) // ADDENDUM 7/13/25 - :(

                if (!HasConnection(n))
                {
                    Debug.Log($"node {this} doesn't have a connection with node {n}! so i'm creating a connection with Them!");
                    CreateConnection(n);
                }
            }
        }

        if (!occupied)
        {
            spriteRenderer.color = normalColor;
        }
        else
        {
            // is occupied, make the occupiedsprite visible
            occIndSpriteRenderer.gameObject.SetActive(true); 
        }
    }
    

    // called when the player travels to this node
    public MapNode TravelTo()
    {
        // Debug.Log($"traveling to node: {this}, its message is {message}");
        if (message != "")
        {
            Debug.Log($"traveling to {this} with message {message}");
        }
        occupied = true;
        spriteRenderer.color = occupiedColor;
        occIndSpriteRenderer.gameObject.SetActive(true); 
        OnTravelTo(); // should be overridden in inheritors
        return this;
    }

    protected virtual void OnTravelTo()
    {
        if (mapNodeData && mapNodeData.onTravelScene != null)
        {
            MapEncounterManager.Instance.StartNewEncounter(mapNodeData.onTravelScene); // this instantiates the scene and everything
        }
        else
        {
            Debug.LogWarning("map node has no onTravelScene!");
        }
    }

    // called when the player was occupying this node and then travels to a different node
    public void OnLeaveNode()
    {
        // Debug.Log($"leaving node: {this}");
        occupied = false;
        visited = true;
        spriteRenderer.color = normalColor;
        occIndSpriteRenderer.gameObject.SetActive(false); 
    }

    void OnMouseEnter()
    {
        if (!occupied)
        {
            spriteRenderer.color = highlightColor;
        }
        
        OnNodeHoverEvent?.Invoke(this);
    }

    void OnMouseExit()
    {
        if (!occupied)
        {
            spriteRenderer.color = normalColor;
        }
        
        OnNodeUnhoverEvent?.Invoke(this);
    }

    void OnMouseDown()
    {
        // do any vfx business... if it needs to happen on this script?
        OnNodeClickedEvent?.Invoke(this);
    }

    public List<MapNode> GetConnectedNodes()
    {
        return nodeConnections;
    }

    // creates a MapNodeConnector between this MapNode and MapNode `node`
    public void CreateConnection(MapNode otherNode)
    {
        GameObject connGO = Instantiate(nodeConnectorPrefab);
        MapNodeConnector conn = connGO.GetComponent<MapNodeConnector>();
        conn.Initialize(this, otherNode);
        AddConnectionToList(otherNode, conn);
        otherNode.AddConnectionToList(this, conn);
    }

    // create and return a MapNodeConnector
    public MapNodeConnector CreateConnectionEditor()
    {
        GameObject connGO = Instantiate(nodeConnectorPrefab);
        MapNodeConnector conn = connGO.GetComponent<MapNodeConnector>();
        return conn;
    }

    public void DestroyConnection(MapNode otherNode, bool inEditor)
    {
        if (HasConnection(otherNode))
        {
            MapNodeConnector conn = GetConnection(otherNode);
            if (inEditor) DestroyImmediate(conn.gameObject);
            else Destroy(conn.gameObject);
            RemoveConnectionFromList(otherNode);
            otherNode.RemoveConnectionFromList(this);
        }
        else
        {
            Debug.Log($"otherNode {this} doesn't have a connection with otherNode {otherNode}, aborting");
        }
    }

    public void AddConnectionToList(MapNode otherNode, MapNodeConnector conn)
    {
        if (HasConnection(otherNode))
        {
            Debug.Log("attempt to add entry to array but it's already present. what");
        }
        else 
        {
            // add them both, should be at the same index (important)
            nodeConnections.Add(otherNode);
            connectors.Add(conn);
        }
    }

    public void RemoveConnectionFromList(MapNode otherNode)
    {
        if (HasConnection(otherNode))
        {
            // this is quite inefficient so make sure this method isn't being called too often
            int index = nodeConnections.IndexOf(otherNode);
            nodeConnections.RemoveAt(index);
            connectors.RemoveAt(index);
        }
    }

    // check if this MapNode has a connection (i.e., a MapNodeConnector) with MapNode `node`
    public bool HasConnection(MapNode node)
    {
        int index = nodeConnections.IndexOf(node);
        if (index >= 0)
        {
            return connectors.Count > index; // presence in the nodeConnectors array indicates that there is a connection
        }
        return false;
    }

    public MapNodeConnector GetConnection(MapNode node)
    {
        if (HasConnection(node))
        {
            int index = nodeConnections.IndexOf(node);
            if (index >= 0)
            {
                return connectors[index];
            }
        }
        return null;
    }
    
    // IN-EDITOR DRAWING
    private void OnDrawGizmos()
    {
        // changing the color and sprite when you change them in the editor. basically when you put in a new mapNodeData SO into this
        // map node it'll change its appearance immediately without having to run the game to see the changes!
        // if (mapNodeData != null)
        // {
        //     normalColor = mapNodeData.unselectedColor != null ? mapNodeData.unselectedColor : Color.gray;
        //     highlightColor = mapNodeData.selectedColor != null ? mapNodeData.selectedColor : Color.white;
        //     
        //     gameObject.GetComponent<SpriteRenderer>().color = normalColor;
        //
        //     gameObject.GetComponent<SpriteRenderer>().sprite = 
        //         mapNodeData.baseSprite != null ? mapNodeData.baseSprite : spriteRenderer.sprite; // evil line of code
        // }

        
        var baseSpriteObj = transform.Find("BaseSprite");
        spriteRenderer = baseSpriteObj.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite; // update this Stuff at editor runtime
        spriteRenderer.color = normalColor;
        
        var occupiedIndicator = transform.Find("OccupiedIndicator");
        occIndSpriteRenderer = occupiedIndicator.GetComponent<SpriteRenderer>();
        


        Gizmos.color = Color.white;

        if (nodeConnections != null)
        {
            foreach (MapNode n in nodeConnections)
            {
                // drawing conns between nodes in editor. this is deprecated because connections are now created as gameobjects in the editor
                // Gizmos.DrawLine(this.transform.position, n.transform.position);
                
                // below is my cool math that draws arrows in the direction of the connections, intended for one-way connections.
                // since it's literally useless (for now...) it's commented out lol but i didn't want to remove it bc its cool
                
                // since the connections are one way, draw an indicator of the direction the line is going. just like. an arrow
                // this them being one way thing is subject to change
                // do some complicated mathematics
                // Vector2 direction = (n.transform.position - this.transform.position).normalized;
                // Vector2 orth = new Vector2(direction.y, -direction.x);
                // Vector2 clamped = Vector2.ClampMagnitude(orth, 0.25f);
                // Vector2 l = Quaternion.AngleAxis(-45.0f, Vector3.forward) * clamped;
                // Vector2 r = Vector2.Reflect(l, orth);
                // Vector2 basePosition = Vector2.Lerp(this.transform.position, n.transform.position, 0.75f);
                // // draw lines
                // Gizmos.DrawLine(basePosition, basePosition + l);
                // Gizmos.DrawLine(basePosition, basePosition + r);
            }
        }
    }
    
}
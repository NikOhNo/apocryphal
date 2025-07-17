using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class MapNodeConnector : MonoBehaviour
{
    // tolerance for determining whether 
    const float DISTANCE_TOLERANCE = 0.001f;
    
    // class for connectors between map nodes
    // world map "paths"
    // basically there can be one of these between any two nodes, represented as the fields `firstNode` and `secondNode`
    // MapNodes will create one of these between them and each of their connections
    // currently this does both visual logic and travel time/encounter logic which is subject to change. might want to separate them.
    
    public MapNode firstNode;
    public MapNode secondNode;
    
    [SerializeField] private float travelTime = 1.0f; // travel time in seconds
    
    // visual stuff, very subject to change
    public float lineWidth = 0.1f;
    public Color normalLineColor = Color.gray;
    public Color highlightedLineColor = Color.white;
    
    private LineRenderer mainLine;
    private LineRenderer chevronTop;
    private LineRenderer chevronBottom;
    
    private SpriteRenderer transitSpriteRenderer;

    void Awake()
    {
        // its component time!
        mainLine = GetComponent<LineRenderer>();
        chevronTop = transform.Find("ChevronTop").GetComponent<LineRenderer>();
        chevronBottom = transform.Find("ChevronBottom").GetComponent<LineRenderer>();
        
        transitSpriteRenderer = transform.Find("TravelSprite").GetComponent<SpriteRenderer>();
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeLine();
        transitSpriteRenderer.enabled = false;
    }

    public void Initialize(MapNode first, MapNode second) 
    {
        this.firstNode = first;
        this.secondNode = second;
        
        // reposition to the centerpoint between both nodes, for visual clarity
        this.transform.position = Vector2.Lerp(firstNode.transform.position, secondNode.transform.position, 0.5f); 
        
        InitializeLine();
    }

    public void SetHighlighted(bool val)
    {
        if (val)
        {
            mainLine.startColor = highlightedLineColor;
            mainLine.endColor = highlightedLineColor;
        }
        else
        {
            mainLine.startColor = normalLineColor;
            mainLine.endColor = normalLineColor;
        }
    }

    public void StartTravelAnimation(MapNode startNode, Action<MapNode> onFinishedCallback = null)
    {
        transitSpriteRenderer.transform.position = startNode.transform.position;
        transitSpriteRenderer.enabled = true;
        
        var endNode = startNode == firstNode ? secondNode : firstNode; // we can start from either end, so set the end position to the other node that is not the start
        StartCoroutine(DoTravelAnimation(travelTime, startNode.transform.position, endNode.transform.position, startNode, onFinishedCallback));
    }

    private IEnumerator DoTravelAnimation(float travelLength, Vector2 startPos, Vector2 endPos, MapNode startNode, Action<MapNode> onFinishedCallback = null)
    {
        float elapsedTime = 0.0f;
        Debug.Log(travelLength);
        Vector2 diff = new Vector2(transitSpriteRenderer.transform.position.x,  transitSpriteRenderer.transform.position.y) - endPos;
        float dist = new Vector2(Mathf.Abs(diff.x),  Mathf.Abs(diff.y)).magnitude;
        Debug.Log(dist);
        while (dist > DISTANCE_TOLERANCE && elapsedTime < travelLength) // stop moving if we've reached our destination OR taken longer than the travelLength indicates we should
        {
            float interp = Mathf.Clamp(elapsedTime / travelLength, 0.0f, 1.0f); // clamp to ensure we don't get any funky values
            transitSpriteRenderer.transform.position = Vector3.Lerp(startPos, endPos, interp);
            elapsedTime += Time.deltaTime;
            diff = new Vector2(transitSpriteRenderer.transform.position.x,  transitSpriteRenderer.transform.position.y) - endPos;
            dist = new Vector2(Mathf.Abs(diff.x),  Mathf.Abs(diff.y)).magnitude;
            yield return null;
        }
        
        Debug.Log("FUCK");
        
        transitSpriteRenderer.enabled = false;
        SetHighlighted(false);
        
        onFinishedCallback?.Invoke(startNode);
    }
    
    
    // function for initializing the visual parts of the connector basically.
    // will position the line between MapNodes firstNode and secondNode (which are properties on this script, set in Initialize())
    private void InitializeLine()
    {
        Vector2 start = firstNode.transform.position;
        Vector2 end = secondNode.transform.position;
        
        chevronTop = transform.Find("ChevronTop").GetComponent<LineRenderer>();
        chevronBottom = transform.Find("ChevronBottom").GetComponent<LineRenderer>();
        mainLine = GetComponent<LineRenderer>();
        
        // millions must boilerplate
        chevronTop.sortingOrder = mainLine.sortingOrder; // don't wanna have to change all three at the same time in the editor
        chevronBottom.sortingOrder = mainLine.sortingOrder;
        
        mainLine.startWidth = lineWidth;
        mainLine.endWidth = lineWidth;
        chevronTop.startWidth = mainLine.startWidth;
        chevronTop.endWidth = mainLine.endWidth;
        chevronBottom.startWidth = mainLine.startWidth;
        chevronBottom.endWidth = mainLine.endWidth;

#if UNITY_EDITOR
        chevronTop.sharedMaterial = mainLine.sharedMaterial;
        chevronBottom.sharedMaterial = mainLine.sharedMaterial;
#else
        chevronTop.material = mainLine.material;
        chevronBottom.material = mainLine.material;
#endif
        
        mainLine.startColor = normalLineColor;
        mainLine.endColor = normalLineColor;
        
        mainLine.SetPosition(0, start);
        mainLine.SetPosition(1, end);
        
        return;
        
        // placing chevrons in the correct positions
        // !!! assuming our start and end positions are set!
        
        // (this is the same code as in MapNode to draw the gizmos)
        // since the connections are not two way, draw an indicator of the direction the line is going. just like. an arrow
        // do some complicated mathematics
        // Vector2 direction = (end - start).normalized;
        // Vector2 orth = new Vector2(direction.y, -direction.x);
        // Vector2 clamped = Vector2.ClampMagnitude(orth, 0.25f);
        // Vector2 l = Quaternion.AngleAxis(-45.0f, Vector3.forward) * clamped;
        // Vector2 r = Vector2.Reflect(l, orth);
        // Vector2 basePosition = Vector2.Lerp(start, end, 0.75f);
        //
        // // finally set chevron positions
        // chevronTop.SetPosition(0, basePosition);
        // chevronTop.SetPosition(1, basePosition + l);
        // chevronBottom.SetPosition(0, basePosition);
        // chevronBottom.SetPosition(1, basePosition + r);
    }
}

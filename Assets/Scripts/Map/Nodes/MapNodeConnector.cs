using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MapNodeConnector : MonoBehaviour
{
    // tolerance for determining whether we've reached the end of the connector
    const float DISTANCE_TOLERANCE = 0.001f;
    
    // class for connectors between map nodes
    // world map "paths"
    // basically there can be one of these between any two nodes, represented as the fields `firstNode` and `secondNode`
    // MapNodes will create one of these between them and each of their connections
    // currently this does both visual logic and travel time/encounter logic which is subject to change. might want to separate them.
    
    
    
    public MapNode firstNode;
    public MapNode secondNode;
    
    [SerializeField] private float travelTime = 1.0f; // travel time in seconds
    [SerializeField] private SO_MapNodeConnectorData data;
    
    // visual stuff, very subject to change
    public float lineWidth = 0.1f;
    public Color normalLineColor = Color.gray;
    public Color highlightedLineColor = Color.white;
    
    public Color pausedColor = Color.blue;
    
    private Color beforePausedColor;
    
    private LineRenderer mainLine;
    private LineRenderer chevronTop;
    private LineRenderer chevronBottom;
    
    private SpriteRenderer transitSpriteRenderer;
    
    private bool paused = false; // if this is true and the DoTravelAnimation coroutine is running, it will NOT move and will NOT load any encounters.
    // intended to be used when an encounter is.. encountered so that the player's movement over the connector is paused
    
    private bool hadEncounter = false;

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

    public void PauseTravel()
    {
        paused = true;
        beforePausedColor = mainLine.startColor;
        mainLine.startColor = pausedColor;
        mainLine.endColor = pausedColor;
    }

    public void UnpauseTravel()
    {
        paused = false;
        mainLine.startColor = beforePausedColor;
        mainLine.endColor = pausedColor;
    }

    public void StartTraveling(MapNode startNode, Action<MapNode> onFinishedCallback = null)
    {
        transitSpriteRenderer.transform.position = startNode.transform.position;
        transitSpriteRenderer.enabled = true;

        bool willEncounter = false;
        if (data)
        {
            float encounterChance = data.encounterChance;
        
            // generate a random number from 0 to 1
            float roll = Random.value;
            willEncounter = (roll < encounterChance) && (data.encounterScene != null);
        }
        
        var endNode = startNode == firstNode ? secondNode : firstNode; // we can start from either end, so set the end position to the other node that is not the start
        StartCoroutine(DoTravelAnimation(travelTime, startNode.transform.position, endNode.transform.position, startNode, willEncounter, onFinishedCallback));
    }

    // coroutine might not be necessary, because it needs to be pausable
    private IEnumerator DoTravelAnimation(float travelLength, Vector2 startPos, Vector2 endPos, MapNode startNode, bool willEncounter, Action<MapNode> onFinishedCallback = null)
    {
        float elapsedTime = 0.0f;
        
        float encounterProgressThreshold = -1.0f;
        if (willEncounter)
        {
            encounterProgressThreshold = 0.5f;
        }
        
        Vector2 diff = new Vector2(transitSpriteRenderer.transform.position.x,  transitSpriteRenderer.transform.position.y) - endPos;
        float dist = new Vector2(Mathf.Abs(diff.x),  Mathf.Abs(diff.y)).magnitude;
        while (dist > DISTANCE_TOLERANCE && elapsedTime < travelLength) // stop moving if we've reached our destination OR taken longer than the travelLength indicates we should
        {
            // if we're paused just stop here
            if (paused)
            {
                yield return null;
            }
            else
            {
                float interp =
                    Mathf.Clamp(elapsedTime / travelLength, 0.0f, 1.0f); // clamp to ensure we don't get any funky values
                
                if (!hadEncounter && willEncounter && interp >= encounterProgressThreshold)
                {
                    hadEncounter = true;
                    MapEncounterManager.Instance.StartNewEncounter(data.encounterScene); // raghhhh very temporary for now until more functionality is defined for running into encounters on conns
                }
                
                transitSpriteRenderer.transform.position = Vector3.Lerp(startPos, endPos, interp);
                elapsedTime += Time.deltaTime;
                diff = new Vector2(transitSpriteRenderer.transform.position.x,
                    transitSpriteRenderer.transform.position.y) - endPos;
                dist = new Vector2(Mathf.Abs(diff.x), Mathf.Abs(diff.y)).magnitude;
                yield return null;
            }
        }
        
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


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems ;



public class PanelBoard : MonoBehaviour
{
    static PanelBoard s_instance;
    public static PanelBoard Instance { get { return s_instance; } }

    //셀사이즈
    [Header("CreateInfo")]
    [SerializeField] float m_cellSize = 64f;

    //스프라이트 리소스
    [Header("Resources")]
    [SerializeField] Sprite[] m_spriteResources;

    //Piece Prefab
    GameObject m_prefabPiece;

    //가상의 노드
    Node[,] m_nodeList;

    int m_cellWidthCount;
    int m_cellHeigthCount;
    float m_blankWidth;
    float m_blankHeight;
    float m_startX;
    float m_startY;

    //드래그 관리해줄 구조체
    PieceDragHandler mDragHandler = new PieceDragHandler();

    private void Awake()
    {
        s_instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }


    void UpdateGravity()
    {
        //어떤 타일을 드래깅 중이라면 Gravity Check 적용 x
        if (mDragHandler.IsNull() == false)
            return;
        //움직임 요청 이벤트

    }

    void CreateBoard()
    {

    }

    Piece CreateRandomPiece(Index index, Vector2 anchoredPosition)
    {
        return;
    }

    public bool CanInput()
    {
        return;
    }

    public void OnPointerDown(PointerEventData eventData, Piece targetPiece)
    {

    }

    public void OnDrag(PointerEventData eventData, Piece targetPiece)
    {

    }
    public void OnPointerup(PointerEventData eventData, Piece targetPiece)
    {

    }
    public void MoveToSwap(Node a, Node b)
    {

    }
    public void OnEventEndSwap(List<PieceMoveHandler> moveList)
    {

    }
    void OnEventGravityEnd(List<PieceMoveHandler> moveHandlerList)
    {

    }

    List<Node> CheckThreeMatch(Index startIndex)
    {
        return;
    }

    void MergeNodeList(List<Node> targetNode, List<Node> tempNode)
    {

    }

    bool IsOutOfBoundIndex(Index Index)
    {
        return;
    }

    Node GetNode(Index Index)
    {
        return;
    }

    PieceType GetPieceType(Index Index)
    {
        return;
    }

    void SetPieceToNode(Piece piecem Node node)
    {

    }
    void DestroyPiece(Node node)
    {

    }

}

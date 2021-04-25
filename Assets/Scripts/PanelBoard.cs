
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

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    void CreateBoard()
    {
        //Piece 프리팹 로드
        m_prefabPiece = Resources.Load("Prefabs/Piece") as GameObject;

        //현재 보드 Transform
        RectTransform boardTransform = GetComponent<RectTransform>();

        //가로, 세로 셀 수
        m_cellWidthCount = (int)(boardTransform.sizeDelta.x / m_cellSize);
        m_cellHeigthCount = (int)(boardTransform.sizeDelta.y / m_cellSize);

        //가로, 세로 셀을 배치하고 남는 여백
        m_blankWidth = (boardTransform.sizeDelta.x % m_cellSize) * 0.5f;
        m_blankHeight = (boardTransform.sizeDelta.y % m_cellSize) * 0.5f;

        //그리드로 사용할 노드
        m_nodeList = new Node[m_cellHeigthCount, m_cellWidthCount];

        for(int y = 0; y < m_cellHeigthCount; ++y )
        {
            for(int x = 0; x < m_cellWidthCount; ++x)
            {
                Vector2 position = new Vector2(m_startX + m_cellSize * x, m_startY - m_cellSize * y);
                Piece newPiece = CreateRandomPiece(new Index(x, y), position);

                //노드도 해당 위치에 생성
                m_nodeList[y, x] = new Node(x, y, newPiece.rectTransform.anchoredPosition, newPiece);
            }
        }
    }

    void UpdateGravity()
    {
        //어떤 타일을 드래깅 중이라면 Gravity Check 적용 x
        if (mDragHandler.IsNull() == false)
            return;
        //움직임 요청 이벤트
        PieceMoveEvent moveEvent = null;
        for (int x = 0; x < m_cellWidthCount; ++x)
        {
            int createCount = 0; //해당 행에서 생성된 Piece 수
            for(int y = m_cellHeigthCount -1; y > 0; --y) //맨 아래노드 부터 검사
            {
                Index index = new Index(x, y); //현재 인덱스
                Node node = GetNode(index); //현재 노드
                if(node.piece != null) // 조각이 있는 노드는 패스.
                {
                    continue;
                }
                //조각이 없는 빈 노드라면 위의 노드들을 검사하며 조각을 찾는다
                for(int ny = y -1; ny >= -1; --ny)
                {
                    //위에 있는 모든 Node를 검사했는데 해당 노드로 끌어 내릴 조각이 없다
                    //->새로 Piece를 생성해 주어야 한다.
                    if(ny == -1)
                    {
                        //새로 생성되어야 할 위치
                        Vector2 position = new Vector2();
                        position.x = m_startX + m_cellSize * x;
                        position.y = m_startY + m_cellSize * y;

                        //새 Piece
                        Piece newPiece = CreateRandomPiece(index, position);

                        if(moveEvent == null)
                        {
                            moveEvent = new PieceMoveEvent();
                        }
                        //새로 생성된 Piece MoveEvent에 추가
                        moveEvent.AddMoveHandler(new PieceMoveHandler(newPiece, node));
                        //해당 Piece 목표 Node에 바인딩
                        SetPieceToNode(newPiece, node);
                        //생성 수++
                        ++createCount;
                        break;
                    }
                    else
                    {
                        //다음 노드 인덱스
                        Index nextIndex = new Index(x, ny);
                        Node nextNode = GetNode(nextIndex);
                        //다음 노드가 빈 노드라면 스팁
                        if(nextNode.piece == null)
                        {
                            continue;
                        }

                        //여기까지 왔다면 Piece를 찾았다는 뜻
                        if(moveEvent == null)
                        {
                            moveEvent = new PieceMoveEvent();
                        }
                        //해당 Piece targetNode로
                        moveEvent.AddMoveHandler(new PieceMoveHandler(nextNode.piece, node));
                        //해당 Piece node로 바인딩
                        SetPieceToNode(nextNode.piece, node);
                        //해당 노드에 piece가 없는 상황
                        nextNode.piece = null;
                        break;
                    }
                }
            }
        }
        //moveEvent가 있다면
        if(moveEvent != null)
        {
            moveEvent.EventMoveEnd += OnEventGravityEnd;
            PieceMoveManager.Instance.AddMoveEvent(moveEvent);
        }
    }


    //새로운 Piece 생성 및 Random한 타입 설정
    Piece CreateRandomPiece(Index index, Vector2 anchoredPosition)
    {
        GameObject newObject = Instantiate(m_prefabPiece, transform);
        Piece piece = newObject.GetComponent<Piece>();

        int random = Random.Range(0, m_spriteResources.Length);
        PieceType pieceType = (PieceType)random;

        piece.Init(index, pieceType, m_spriteResources[random]);
        piece.rectTransform.anchoredPosition = anchoredPosition;
        return piece;
    }

    public bool CanInput()
    {
        return mDragHandler.IsNull();
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

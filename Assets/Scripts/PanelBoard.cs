using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using DG.Tweening;

/**********************************************************************************
 * Piece가 차있는 보드를 관리하는 클래스
 * 3Match의 실제 룰은 이곳에서 관리
 **********************************************************************************/
public class PanelBoard : MonoBehaviour
{
    //싱글톤
    static PanelBoard sInstance;
    public static PanelBoard Instance { get { return sInstance; } }
    //셀 사이즈
    [Header("CreateInfo")]
    [SerializeField] float m_cellSize = 64f;
    //스프라이트 리소스
    [Header("Resource")]
    [SerializeField] Sprite[] m_sprireResources;
    //Piece 프리팹
    GameObject mPrefabPiece;
    //가상의 노드들
    Node[,] mNodeList;

    int mCellWidthCount;
    int mCellHeightCount;
    float mBlankWidth;
    float mBlankHeight;
    float mStartX;
    float mStartY;
    bool m_CheckCreate = false;

    //드래그 관리해 줄 구조체
    PieceDragHandler mDragHanlder = new PieceDragHandler();

    //--------------------------------------------------------------------------------
    private void Awake()
    {
        sInstance = this;
    }

    //--------------------------------------------------------------------------------
    private void Start()
    {

        CreateBoard();
        DOTween.Init();

    }

    //--------------------------------------------------------------------------------
    private void Update()
    {
        UpdateGravity();
    }

    //--------------------------------------------------------------------------------
    void UpdateGravity()
    {
        //어떤 타일을 드래깅 중이라면 Gravity Check 적용 X
        if (mDragHanlder.IsNull() == false)
            return;
        //움직임 요청 이벤트
        PieceMoveEvent moveEvent = null;

        for (int x = 0; x < mCellWidthCount; ++x)
        {
            int createCount = 0; //해당 행에서 몇개의 새로운 Piece 생성했는지 수
            for (int y = mCellHeightCount - 1; y >= 0; --y)  //맨 아래 노드들 부터 검사
            {
                Index index = new Index(x, y); //현재 인덱스
                Node node = GetNode(index); //현재 노드
                if (node.piece != null) continue;   //조각이 있는 노드면 딱히 별도의 처리 필요 X 

                //조각이 없는 빈 노드라면 위의 노드들을 검사하면서 조각을 찾는다. 
                for (int ny = y - 1; ny >= -1; --ny)
                {
                    //위에 있는 모든 Node를 검사했는데 해당 노드로 끌어 내릴 조각이 없다는 뜻 => 새로 Piece를 생성해 주어야 한다는 뜻
                    if (ny == -1)
                    {
                        //새로 생성되어야 할 위치
                        Vector2 position = new Vector2();
                        position.x = mStartX + m_cellSize * x;
                        position.y = mStartY + m_cellSize * (createCount + 1);
                        //새 Piece
                        Piece newPiece = CreateRandomPiece(index, position);

                        if (moveEvent == null)
                            moveEvent = new PieceMoveEvent();
                        //새로 생성된 Piece MoveEvent에 추가
                        moveEvent.AddMoveHandler(new PieceMoveHandler(newPiece, node));
                        //해당 Piece 목표 Node에 바인딩
                        SetPieceToNode(newPiece, node);
                        //생성 수 ++ 
                        ++createCount;

                        break;
                    }
                    else
                    {
                        //다음 노드 인덱스
                        Index nextIndex = new Index(x, ny);
                        Node nextNode = GetNode(nextIndex);
                        //다음 노드가 빈 노드라면 스킵 
                        if (nextNode.piece == null)
                        {
                            continue;
                        }

                        //여기까지 왔다면 Piece를 찾았다는 뜻

                        if (moveEvent == null)
                            moveEvent = new PieceMoveEvent();
                        //해당 Piece targetNode로 끌어내리자
                        moveEvent.AddMoveHandler(new PieceMoveHandler(nextNode.piece, node));
                        //해당 Piece node로 바인딩
                        SetPieceToNode(nextNode.piece, node);
                        //이제 해당 노드에는 piece가 없는 상황
                        nextNode.piece = null;
                        break;
                    }
                }
            }
        }
        //MoveEvent가 있다면
        if (moveEvent != null)
        {
            moveEvent.EventMoveEnd += OnEventGravityEnd;
            PieceMoveManager.Instance.AddMoveEvent(moveEvent);
        }
    }

    //--------------------------------------------------------------------------------
    void CreateBoard()
    {
        if (m_CheckCreate == false)
        {
            //Piece 프리팹 로드
            mPrefabPiece = Resources.Load("Prefabs/Piece") as GameObject;
            //현재 보드 Transform
            RectTransform boardTransform = GetComponent<RectTransform>();
            //가로,세로 셀 수 
            mCellWidthCount = (int)(boardTransform.sizeDelta.x / m_cellSize);
            mCellHeightCount = (int)(boardTransform.sizeDelta.y / m_cellSize);
            //가로,세로 셀을 배치하고 남는 여백
            mBlankWidth = (boardTransform.sizeDelta.x % m_cellSize) * 0.5f;
            mBlankHeight = (boardTransform.sizeDelta.y % m_cellSize) * 0.5f;
            //시작지점
            mStartX = -boardTransform.sizeDelta.x * 0.5f + mBlankWidth + m_cellSize * 0.5f;
            mStartY = boardTransform.sizeDelta.y * 0.5f - mBlankHeight - m_cellSize * 0.5f;

            //그리드로 사용할 노드
            mNodeList = new Node[mCellHeightCount, mCellWidthCount];
            //퍼즐 조각들 생성 시작
            for (int y = 0; y < mCellHeightCount; ++y)
            {
                for (int x = 0; x < mCellWidthCount; ++x)
                {
                    //새로운 Piece 생성
                    Vector2 position = new Vector2(mStartX + m_cellSize * x, mStartY - m_cellSize * y);
                    Piece newPiece = CreateRandomPiece(new Index(x, y), position);
                    //노드도 해당 위치에 생성
                    mNodeList[y, x] = new Node(x, y, newPiece.rectTransform.anchoredPosition, newPiece);
                }
            }
        }
        
        //결과 반환할 Result
        List<Node> result = new List<Node>()

        //검사할 방향
        Index[] arrayDirection = new Index[4];
        arrayDirection[0] = Index.left;
        arrayDirection[1] = Index.right;
        arrayDirection[2] = Index.top;
        arrayDirection[3] = Index.bottom;


        for(int x = 1; x < mCellWidthCount; ++x)
        {
            for(int y = 1; y < mCellHeightCount; ++y)
            {
                for (int i = 0; i < 4; i += 2)
                {
                    //left,right,up,down 식으로 배열이 이루어져 있으므로 양방향 검사 한번에 하기위해서 이렇게 처리
                    Index[] directions = new Index[2];
                    directions[0] = arrayDirection[i];
                    directions[1] = arrayDirection[i + 1];
                    //매치된 수 
                    int matchCount = 0;
                    List<Node> line = new List<Node>();
                    for (int j = 0; j < directions.Length; ++j)
                    {
                        Index temp = new Index(x,y);
                        Index index = Index.Add(temp, directions[j]);
                        Node node = GetNode(index);
                        if (node != null && node.piece != null && node.piece.pieceType == GetPieceType(temp))
                        {
                            ++matchCount;
                            line.Add(node);
                        }
                    }

                    if (matchCount >= 2)
                    {
                        MergeNodeList(result, line);
                    }
                }
            }
        }
    }

    //--------------------------------------------------------------------------------
    //새로운 Piece 생성 후 Random한 Type 지정
    Piece CreateRandomPiece(Index index, Vector2 anchoredPosition)
    {
        GameObject newObject = Instantiate(mPrefabPiece, transform);
        Piece piece = newObject.GetComponent<Piece>();

        int random = UnityEngine.Random.Range(0, m_sprireResources.Length);
        PieceType pieceType = (PieceType)random;

        piece.Init(index, pieceType, m_sprireResources[random]);
        piece.rectTransform.anchoredPosition = anchoredPosition;

        return piece;
    }

    //--------------------------------------------------------------------------------
    //키 입력 받을 수 있는지
    public bool CanInput()
    {
        //현재 키 입력 중이 아니라면 
        return mDragHanlder.IsNull();
    }

    //--------------------------------------------------------------------------------
    public void OnPointerDown(PointerEventData eventData, Piece targetPiece)
    {
        //움직이는 Piece가 존재한다면 return 
        if (PieceMoveManager.Instance.HasMoveEvent())
            return;
        //이미 입력을 받고 있다면 return 
        if (mDragHanlder.IsNull() == false)
            return;
        //피킹 한 Piece가 없다면 return 
        if (targetPiece == null)
            return;

        //인덱스는 현재 피킹한 Piece의 인덱스
        Index index = targetPiece.index;
        //Drag에 데이터 셋
        mDragHanlder.Set(targetPiece, mNodeList[index.y, index.x]);
        Debug.Log("OnPointerDown : " + targetPiece.name);
    }
    //--------------------------------------------------------------------------------
    public void OnDrag(PointerEventData eventData, Piece targetPiece)
    {
        if (PieceMoveManager.Instance.HasMoveEvent())
            return;

        if (mDragHanlder.IsNull())
            return;

        mDragHanlder.OnDrag(eventData);
    }
    //--------------------------------------------------------------------------------
    public void OnPointerUp(PointerEventData eventData, Piece targetPiece)
    {
        if (mDragHanlder.IsNull())
            return;

        //드래그 한 방향 받아온다. 
        Index direction = new Index(mDragHanlder.GetDragDirection());
        //아래가 인덱스 증감 처리이므로 뒤집어준다.
        direction.y *= -1;
        //시작점 인덱스
        Index originIndex = mDragHanlder.originNode.Index;
        //스왑할 지점 인덱스
        Index swapIndex = Index.Add(originIndex, direction);

        //만약 스왑할 지점이 Board 밖이 아니라면
        if (IsOutOfBoundIndex(swapIndex) == false)
        {
            //스왑
            MoveToSwap(mDragHanlder.originNode, GetNode(swapIndex));
        }
        //스왑할 지점이 보드 밖이라면
        else
        {
            //Piece 다시 원래 자리로 세팅
            mDragHanlder.targetPiece.rectTransform.anchoredPosition = mDragHanlder.originNode.position;
        }

        //드래깅 끝났으므로 Reset 
        mDragHanlder.Reset();
        //targetPiece.transform.DOPunchScale(Vector3.one, 0.5f, 1);
        Debug.Log("OnPointerUp : " + targetPiece.name);
    }

    //--------------------------------------------------------------------------------
    public void MoveToSwap(Node a, Node b)
    {
        //MoveEvent 생성
        PieceMoveEvent moveEvent = new PieceMoveEvent();

        Piece aPiece = a.piece;
        Piece bPiece = b.piece;
        //aPiece는 bNode로, bPiece는 aNode로 이동
        moveEvent.AddMoveHandler(new PieceMoveHandler(aPiece, b));
        moveEvent.AddMoveHandler(new PieceMoveHandler(bPiece, a));

        //aPiece.transform.DOPunchScale(Vector3.one, 0.4f , 2 ,3);
        //bPiece.transform.DOPunchScale(Vector3.one, 0.4f , 2 ,3);

        //해당 Piece들 이동할 Node에 바인딩
        SetPieceToNode(bPiece, a);
        SetPieceToNode(aPiece, b);
        //이동 끝났을 때 알림받을 함수 바인딩
        moveEvent.EventMoveEnd += OnEventEndSwap;
        PieceMoveManager.Instance.AddMoveEvent(moveEvent);
    }

    //--------------------------------------------------------------------------------
    //스왑 Move가 끝났을 경우에 실행
    public void OnEventEndSwap(List<PieceMoveHandler> moveList)
    {
        Debug.Log("PanelBoard::OnEventEndSwap");
        //match된 Piece들 받을 리스트
        List<Node> matchList = new List<Node>();
        //움직인 Piece들 기준으로 ThreeMatch 검사
        for (int i = 0; i < moveList.Count; ++i)
        {
            //매치된 리스트 받아옴
            List<Node> tempList = CheckThreeMatch(moveList[i].targetNode.Index);
            //하나의 리스트로 합친다
            MergeNodeList(matchList, tempList);
        }

        //매치된게 없다면 다시 되돌려야 함
        if (matchList.Count == 0)
        {
            //스왑으로 이동했으므로 moveList는 2개 
            PieceMoveEvent moveEvent = new PieceMoveEvent();

            Node aNode = moveList[0].targetNode;
            Node bNode = moveList[1].targetNode;
            Piece aPiece = aNode.piece;
            Piece bPiece = bNode.piece;

            moveEvent.AddMoveHandler(new PieceMoveHandler(aPiece, bNode));
            moveEvent.AddMoveHandler(new PieceMoveHandler(bPiece, aNode));

            SetPieceToNode(bPiece, aNode);
            SetPieceToNode(aPiece, bNode);

            PieceMoveManager.Instance.AddMoveEvent(moveEvent);
        }
        else
        {
            //매치된 리스트들 전부 삭제
            for (int i = 0; i < matchList.Count; ++i)
            {
                DestroyPiece(matchList[i]);
            }
        }
    }
    //--------------------------------------------------------------------------------
    //중력이동 끝났다면
    void OnEventGravityEnd(List<PieceMoveHandler> moveHandlerList)
    {
        //매치 리스트
        List<Node> matchList = new List<Node>();
        for (int i = 0; i < moveHandlerList.Count; ++i)
        {
            //전부 합침
            List<Node> tempCheck = CheckThreeMatch(moveHandlerList[i].targetNode.Index);
            if (tempCheck.Count != 0)
            {
                MergeNodeList(matchList, tempCheck);
            }
        }
        //매치된 애들 전부 삭제
        for (int i = 0; i < matchList.Count; ++i)
        {
            DestroyPiece(matchList[i]);
        }
    }



    //--------------------------------------------------------------------------------
    //매치된 Piece들 검사
    List<Node> CheckThreeMatch(Index startIndex)
    {
        //메인 PieceType
        PieceType startPieceType = GetPieceType(startIndex);
        if (startPieceType == PieceType.Null)
            return null;

        //결과 반환할 Result
        List<Node> result = new List<Node>();

        //검사할 방향
        Index[] arrayDirection = new Index[4];
        arrayDirection[0] = Index.left;
        arrayDirection[1] = Index.right;
        arrayDirection[2] = Index.top;
        arrayDirection[3] = Index.bottom;

        //{{ 각 방향으로 매치 검사 ~
        for (int i = 0; i < arrayDirection.Length; ++i)
        {
            List<Node> line = new List<Node>();
            //가로 검사냐 세로 검사냐에 따라 최대 셀 수가 달라짐
            int maxCellCount = arrayDirection[i].x != 0 ? mCellWidthCount : mCellHeightCount;

            int matchCount = 0; //매치된 수
            //해당 방향으로 검사
            for (int j = 1; j < maxCellCount; ++j)
            {
                Index nextIndex = Index.Add(startIndex, Index.Mul(arrayDirection[i], j));
                //같은 타입이라면
                if (GetPieceType(nextIndex) == startPieceType)
                {
                    //추가
                    Node node = GetNode(nextIndex);
                    line.Add(node);
                    ++matchCount;
                }
                //아니라면 해당 방향으로 검사 그만
                else
                {
                    break;
                }
            }
            //2개이상 매치가 되었다면 시작점을 포함해서 3match가 된것이므로 결과값에 포함해주자
            if (matchCount >= 2)
            {
                MergeNodeList(result, line);
            }
        }
        // }}

        // {{ 가운데로부터 상하좌우 검사 ~
        for (int i = 0; i < 4; i += 2)
        {
            //left,right,up,down 식으로 배열이 이루어져 있으므로 양방향 검사 한번에 하기위해서 이렇게 처리
            Index[] directions = new Index[2];
            directions[0] = arrayDirection[i];
            directions[1] = arrayDirection[i + 1];
            //매치된 수 
            int matchCount = 0;
            List<Node> line = new List<Node>();
            for (int j = 0; j < directions.Length; ++j)
            {
                Index index = Index.Add(startIndex, directions[j]);
                Node node = GetNode(index);
                if (node != null && node.piece != null && node.piece.pieceType == startPieceType)
                {
                    ++matchCount;
                    line.Add(node);
                }
            }

            if (matchCount >= 2)
            {
                MergeNodeList(result, line);
            }
        }
        // }}

        for(int i =0; i < result.Count; ++i)
        {

            //if (result[i].piece.transform.DOComplete() == 1)
            //    break;
            result[i].piece.transform.DOPunchScale(Vector3.one, 0.4f, 4, 3);
            Piece startPiece = GetPiece(startIndex);
            //if (startPiece.transform.DOComplete() == 1)
            //    startPiece.transform.DOPunchScale(Vector3.one, 0.2f, 4, 3);

        }

        

        //매치된 결과가 있다면 시작 노드 추가
        if (result.Count != 0)
        {
            Node node = GetNode(startIndex);
            if (result.Contains(node) == false)
                result.Add(GetNode(startIndex));
        }

        return result;
    }

    //--------------------------------------------------------------------------------
    //targetNode에 tempNode 원소 중복안되게 추가
    void MergeNodeList(List<Node> targetNode, List<Node> tempNode)
    {
        for (int i = 0; i < tempNode.Count; ++i)
        {
            if (targetNode.Contains(tempNode[i])) continue;

            targetNode.Add(tempNode[i]);
        }
    }

    //--------------------------------------------------------------------------------
    //해당 인덱스가 범위를 벗어 나는지
    bool IsOutOfBoundIndex(Index Index)
    {
        if (Index.x < 0 || Index.x >= mCellWidthCount) return true;
        if (Index.y < 0 || Index.y >= mCellHeightCount) return true;
        return false;
    }

    //--------------------------------------------------------------------------------
    Node GetNode(Index Index)
    {
        if (IsOutOfBoundIndex(Index))
            return null;

        return mNodeList[Index.y, Index.x];
    }

    //--------------------------------------------------------------------------------
    Piece GetPiece(Index Index)
    {
        Node node = GetNode(Index);
        if (node == null)
            return null;
        return node.piece;
    }

    //--------------------------------------------------------------------------------
    PieceType GetPieceType(Index Index)
    {
        Piece piece = GetPiece(Index);
        if (piece == null)
            return PieceType.Null;

        return piece.pieceType;
    }
    //--------------------------------------------------------------------------------
    void SetPieceToNode(Piece piece, Node node)
    {
        piece.index = node.Index;
        node.piece = piece;
    }

    //--------------------------------------------------------------------------------
    void DestroyPiece(Node node)
    {
        if (node == null || node.piece == null)
            return;

        node.piece.DestroyPiece();
        node.piece = null;
    }
}

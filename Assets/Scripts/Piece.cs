using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum PieceType
{

    Cube = 0,
    Cylinder = 1,
    Pryamid = 2,
    Sphere = 3,
    Diamond = 4,
    Cat = 5,
    Dog = 6,
    Mouse = 7,
    Panda = 8,
    Rabbit = 9,
    Null = 10


}




public class Piece : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] PieceType m_pieceType;
    Image mImage;
    RectTransform mTransform;
    Index mIndex;
    Effect m_effect;
    public RectTransform rectTransform { get { return mTransform; } }
    public PieceType pieceType { get { return m_pieceType; } }
    public Index index { get { return mIndex; } set { mIndex = value; } }

    //--------------------------------------------------------------------------------
    public void Init(Index index, PieceType pieceType, Sprite sprite)
    {
        mImage = GetComponent<Image>();
        mTransform = GetComponent<RectTransform>();

        m_pieceType = pieceType;
        mImage.sprite = sprite;
        mIndex = index;

    }

    //--------------------------------------------------------------------------------
    public void OnPointerDown(PointerEventData eventData)
    {
        PanelBoard.Instance.OnPointerDown(eventData, this);
    }

    //--------------------------------------------------------------------------------
    public void OnDrag(PointerEventData eventData)
    {
        PanelBoard.Instance.OnDrag(eventData, this);
    }

    //--------------------------------------------------------------------------------
    public void OnPointerUp(PointerEventData eventData)
    {
        PanelBoard.Instance.OnPointerUp(eventData, this);
    }

    //--------------------------------------------------------------------------------
    public void DestroyPiece()
    {
        //Destroy(gameObject);
        StartCoroutine(DesolvePiece());
    }

    //--------------------------------------------------------------------------------
    IEnumerator DesolvePiece()
    {
        while (transform.localScale.x < 1.6f)
        {
            float value = 2f * Time.deltaTime;
            this.transform.DOPunchScale(Vector3.one, 0.5f);
            //mTransform.localScale = mTransform.localScale - new Vector3(value, value, value);
            yield return null;
        }
        if (this.gameObject != null)
        {
            //m_effect.GetComponent<Effect>().CreatEffect(this.transform);
            Effect.instance.CreatEffect(this.transform);
            Destroy(gameObject);
        }


    }
}

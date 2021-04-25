using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum PieceType
{
    Cat = 0,
    Dog = 1,
    Mouse = 2,
    Panda = 3,
    Rabbit = 4,
    Null =5

}

public class Piece : MonoBehaviour , IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] PieceType m_pieceType;
    Image m_image;
    RectTransform m_transform;
    Index m_index;

    public RectTransform rectTransform { get { return m_transform; }}
    public PieceType pieceType { get { return m_pieceType; } }
    public Index index { get { return m_index; } }

    public void Init(Index index, PieceType pieceType, Sprite sprite)
    {
        m_image = GetComponent<Image>();
        m_transform = GetComponent<RectTransform>();

        m_pieceType = pieceType;
        m_image.sprite = sprite;
        m_index = index;

    }


    public void OnPointerDown(PointerEventData eventData)
    {
        PanelBoard.Instance.OnPointerDown(eventData, this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        PanelBoard.Instance.OnDrag(eventData, this);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        PanelBoard.Instance.OnPointerup(eventData, this);
    }

    public void DestroyPiece()
    {
        StartCoroutine(DesolvePiece());
    }

    IEnumerator DesolvePiece()
    {
        while(m_transform.localScale.x > 0.2f)
        {
            float value = 2f * Time.deltaTime;
            m_transform.localScale = m_transform.localScale - new Vector3(value, value, value);
            yield return null;
        }
        Destroy(gameObject);
    }
}

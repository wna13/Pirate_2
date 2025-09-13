using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
// using DG.Tweening;

public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

	int pointerId = -99;

	public Transform outCircleTrans;
	private Canvas rootCanvas;
	private Camera myUICamera;

	public Vector2 CONTROLVECTOR {get; private set;}
	public Vector2 CONTROLNORMALVECTOR{get; private set;}
	public float CONTROLSTRENGTH {get; private set;}

	public Transform PlayerMove;


	void Awake()
	{
		rootCanvas = this.GetComponentInParent<Canvas>();
		myUICamera = this.GetComponentInParent<Camera>();
	}

    public void OnPointerDown(PointerEventData eventData)
    {
		if(pointerId != -99)
			return;
		
		this.pointerId = eventData.pointerId;

		Vector2 pos = this.GetLocalPosition(eventData.position);
		outCircleTrans.localPosition = pos;
    }
	
    public void OnDrag(PointerEventData eventData)
    {
		if(pointerId != eventData.pointerId)
			return;

		Vector2 pos = this.GetLocalPosition(eventData.position);
		outCircleTrans.localPosition = pos;

		PlayerMover.Instance.shipMover.PlayerMove(pos);
    }


    public void OnPointerUp(PointerEventData eventData)
    {
		if(pointerId != eventData.pointerId)
			return;

		Vector2 pos = this.GetLocalPosition(eventData.position);
		outCircleTrans.localPosition = pos;

		PlayerMover.Instance.shipMover.PlayerStop();

		
		Reset();
    }


	private void Reset()
	{
		CONTROLVECTOR = Vector2.zero;
		CONTROLNORMALVECTOR = Vector2.zero;
		CONTROLSTRENGTH = 0f;
		pointerId = -99;
	}

	Vector2 GetLocalPosition(Vector2 screenPos)
	{
		Vector2 resultPos;
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
			(this.transform as RectTransform),
			screenPos,
			rootCanvas.renderMode == RenderMode.ScreenSpaceCamera ? myUICamera : null,
			out resultPos))
		{
			float dist = resultPos.magnitude; // 로컬좌표의 길이를 가져온다.
			CONTROLNORMALVECTOR = resultPos.normalized; //로컬좌표의 노말벡터를 가져온다 //노말벡터는 길이가 1 (방향만 가지고 있는 벡터)

			Vector2 pos = CONTROLNORMALVECTOR * dist;

			return pos;
		}
		else
		{
			return Vector2.zero;
		}
	}
}

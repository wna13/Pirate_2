using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{

	
	public TouchPad myTouchPad;
	private Canvas rootCanvas;
	private Camera myUICamera;

	int pointerId = -99;		// 터치 id   마우스 클릭, 핸드폰 터치 했을경우 터치 아이디를 저장할 변수.
								// -99 이면 아무터치도 들어오지 않은 상태
	void Awake()
	{
		rootCanvas = this.GetComponentInParent<Canvas>();
		myUICamera = this.GetComponentInParent<Camera>();


		myTouchPad.gameObject.SetActive(false);
	}

    public void OnPointerDown(PointerEventData eventData)
    {
		//중복 터치를 막기위해 터치아이디를 기본값이랑 비교를 한다.
		//터치가 1도 안들어와있으면 스크립트를 진행 하지만 터치가 이미 들어와있따. 그러면 리턴
		if(pointerId != -99)
			return;	

		// //Debug.Log( eventData.pointerId);
		this.pointerId = eventData.pointerId;

		Vector2 pos = this.GetLocalPosition(eventData.position);

		myTouchPad.gameObject.SetActive(true);
		myTouchPad.transform.localPosition = pos;

		// (myTouchPad.transform as RectTransform).anchoredPosition = pos;


		//같은 이벤트 데이터를 전송하기 위해
		myTouchPad.OnPointerDown(eventData);

		// //Debug.Log("OnPointerDown");
    }


    public void OnDrag(PointerEventData eventData)
    {
		//내가 터치한 아이디값과 이벤트가 들어온 터치아이디 값이 똑같아야만 진행
		if(pointerId != eventData.pointerId)
			return;

		myTouchPad.OnDrag(eventData);

		// //Debug.Log("OnDrag");
		
    }

    public void OnPointerUp(PointerEventData eventData)
    {
		//내가 터치한 아이디값과 이벤트가 들어온 터치아이디 값이 똑같아야만 진행
		if(pointerId != eventData.pointerId)
			return;
		
		//터치 업 콜 들어왔을 경우 아이디 초기화, 터치패드 오프
		myTouchPad.OnPointerUp(eventData);
		myTouchPad.gameObject.SetActive(false);
		pointerId = -99;

		// //Debug.Log("OnPointerUp");
    }

	Vector2 GetLocalPosition(Vector2 screenPos)
	{
		Vector2 resultPos;
		//내가 터치한 스크린포인트를 현재 이 컴포넌트가 붙어있는오브젝트의 로컬좌표로 변환해서 가져온다.
		if(RectTransformUtility.ScreenPointToLocalPointInRectangle(
			(this.transform as RectTransform),
			screenPos,
			rootCanvas.renderMode == RenderMode.ScreenSpaceCamera ? myUICamera : null,
			out resultPos))
		{
			return resultPos;
		}
		else
		{
			return Vector2.zero;
		}
	}
}

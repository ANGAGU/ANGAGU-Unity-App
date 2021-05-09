using UnityEngine;
using UnityEngine.EventSystems;


// 꼐속 누르고 있을 동안만 반복해주기 위해서 IPointerUpHandler, IPointerDownHandler 를 호출해준다.
public class OnPress : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public ArDrawLine _drawLineST;
    
    private bool _pressed = false;
    //Detect current clicks on the GameObject (the one with the script attached)
    public void OnPointerDown(PointerEventData pointerEventData) // 버튼을 클릭/터치하는 순간 실행됨
    {
        _pressed = true;
    }

    //Detect if clicks are no longer registering
    public void OnPointerUp(PointerEventData pointerEventData) // 버튼 클릭/터치를 떼는 순간 실행됨
    {
        _pressed = false;
        _drawLineST.StopDrawLine();
    }

    private void Update()
    {
        if (_pressed)
        {
            _drawLineST.StartDrawLine();
        }
    }
}
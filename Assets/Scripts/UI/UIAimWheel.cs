using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class UIAimWheel : MonoBehaviour
{
    public Graphic UI_Element;
    public UIPlayerUpdate playerUpdate;

    RectTransform rectT;
    Vector2 centerPoint;
    Vector2 startPoint;

    private int direction;
    public float degreesPerTick; // Degrees we need to rotate the wheel for one unit of aim

    float wheelAngle = 0f;
    float wheelPrevAngle = 0f;
    private float totalWheelMove;

    void Start()
    {
        rectT = UI_Element.rectTransform;
        InitEventsSystem();
    }

    void Update()
    {

        // Rotate the wheel image
        //rectT.Rotate(Vector3.back * wheelAngle * Time.deltaTime);
        rectT.localEulerAngles = Vector3.back * wheelAngle;
        //rectT.localEulerAngles = Vector3.back * wheelAngle;
    }

    void InitEventsSystem()
    {
        // Try to get an EventTrigger component on the Aim Wheel
        EventTrigger events = UI_Element.gameObject.GetComponent<EventTrigger>();

        // If there isn't one, create it
        if (events == null)
            events = UI_Element.gameObject.AddComponent<EventTrigger>();

        // If there are no triggers defined, add a trigger
        if (events.triggers == null)
            events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

        // Add trigger for pointer down (mouse or touch)
        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>(PressEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback = callback;

        events.triggers.Add(entry);

        // Add trigger for drag (mouse or touch)
        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>(DragEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.Drag;
        entry.callback = callback;

        events.triggers.Add(entry);

        // Add trigger for pointer up (mouse or touch)
        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>(ReleaseEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback = callback;

        events.triggers.Add(entry);
    }

    public void PressEvent(BaseEventData eventData)
    {
        // Executed when mouse/finger starts touching the steering wheel
        Vector2 pointerPos = ((PointerEventData)eventData).position;
        playerUpdate.InteractedUI();

        centerPoint = RectTransformUtility.WorldToScreenPoint(((PointerEventData)eventData).pressEventCamera, rectT.position);
        wheelPrevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
        totalWheelMove = 0;
    }

    public void DragEvent(BaseEventData eventData)
    {
        // Executed when mouse/finger is dragged over the steering wheel
        Vector2 pointerPos = ((PointerEventData)eventData).position;

        float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
        // Do nothing if the pointer is too close to the center of the wheel
        if (Vector2.Distance(pointerPos, centerPoint) > 20f)
        {
            if (pointerPos.x > centerPoint.x)
            {
                wheelAngle += wheelNewAngle - wheelPrevAngle;
                totalWheelMove += wheelNewAngle - wheelPrevAngle; // + Clockwise, - Counter-clockwise
            }
            else
            {
                wheelAngle -= wheelNewAngle - wheelPrevAngle;
                totalWheelMove += -(wheelNewAngle - wheelPrevAngle); // + Clockwise, - Counter-clockwise
            }
        }


        wheelPrevAngle = wheelNewAngle;
        playerUpdate.InteractedUI();

        // Check if the wheel has moved enough for us to trigger an aim change
        if (totalWheelMove >= degreesPerTick)
        {
            playerUpdate.AimTick(1); // Clockwise
            totalWheelMove -= degreesPerTick;
        }
        else if (totalWheelMove <= -degreesPerTick)
        {
            playerUpdate.AimTick(-1); // Counter-Clockwise
            totalWheelMove -= -degreesPerTick;
        }
    }

    public void ReleaseEvent(BaseEventData eventData)
    {
        // Executed when mouse/finger stops touching the steering wheel
        // Performs one last DragEvent, just in case
        DragEvent(eventData);

        direction = 0;
    }
}
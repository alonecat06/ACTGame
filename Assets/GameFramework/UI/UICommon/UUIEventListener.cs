using UnityEngine;  
using System.Collections;  
using UnityEngine.EventSystems; 
 
public class UUIEventListener : MonoBehaviour,  
                                IPointerClickHandler,  
                                IPointerDownHandler,  
                                IPointerEnterHandler,  
                                IPointerExitHandler,  
                                IPointerUpHandler,  
                                ISelectHandler,  
                                IUpdateSelectedHandler,  
                                IDeselectHandler,  
                                IDragHandler,  
                                IEndDragHandler,  
                                IDropHandler,  
                                IScrollHandler,  
                                IMoveHandler  
{
    public delegate void BaseDelegate(GameObject go, BaseEventData eventData);
    public delegate void PointerDelegate(GameObject go, PointerEventData eventData);
    public delegate void AxisDelegate(GameObject go, AxisEventData eventData);

    public PointerDelegate onClick;
    public PointerDelegate onDown;
    public PointerDelegate onEnter;
    public PointerDelegate onExit;
    public PointerDelegate onUp;
    public BaseDelegate onSelect;
    public BaseDelegate onUpdateSelect;
    public BaseDelegate onDeSelect;
    public PointerDelegate onDrag;
    public PointerDelegate onDragEnd;
    public PointerDelegate onDrop;  
    public PointerDelegate onScroll;
    public AxisDelegate onMove;

    //public object parameter;

    public void OnPointerClick(PointerEventData eventData) 
    { 
        if (onClick != null) 
            onClick(gameObject, eventData); 
    }
    public void OnPointerDown(PointerEventData eventData) 
    { 
        if (onDown != null) 
            onDown(gameObject, eventData); 
    }
    public void OnPointerEnter(PointerEventData eventData) 
    { 
        if (onEnter != null) 
            onEnter(gameObject, eventData); 
    }
    public void OnPointerExit(PointerEventData eventData) 
    { 
        if (onExit != null) 
            onExit(gameObject, eventData); 
    }
    public void OnPointerUp(PointerEventData eventData) 
    { 
        if (onUp != null)
            onUp(gameObject, eventData); 
    }
    public void OnSelect(BaseEventData eventData) 
    { 
        if (onSelect != null) 
            onSelect(gameObject, eventData); 
    }
    public void OnUpdateSelected(BaseEventData eventData) 
    { 
        if (onUpdateSelect != null) 
            onUpdateSelect(gameObject, eventData);
    }
    public void OnDeselect(BaseEventData eventData) 
    { 
        if (onDeSelect != null) 
            onDeSelect(gameObject, eventData); 
    }
    public void OnDrag(PointerEventData eventData) 
    { 
        if (onDrag != null) 
            onDrag(gameObject, eventData); 
    }
    public void OnEndDrag(PointerEventData eventData) 
    { 
        if (onDragEnd != null) 
            onDragEnd(gameObject, eventData); 
    }
    public void OnDrop(PointerEventData eventData) 
    { 
        if (onDrop != null) 
            onDrop(gameObject, eventData); 
    }
    public void OnScroll(PointerEventData eventData) 
    { 
        if (onScroll != null)
            onScroll(gameObject, eventData); 
    }
    public void OnMove(AxisEventData eventData) 
    { 
        if (onMove != null) 
            onMove(gameObject, eventData); 
    }  
  
    static public UUIEventListener Get(GameObject go)  
    {
        UUIEventListener listener = go.GetComponent<UUIEventListener>();
 
        if (listener == null)
            listener = go.AddComponent<UUIEventListener>();

        return listener;  
    }  
}
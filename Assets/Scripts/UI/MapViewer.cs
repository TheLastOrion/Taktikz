using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace UI
{
    public class MapViewer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Camera _mainCamera;
        private void Start()
        {
            _mainCamera = Camera.main;

        }
        private void Update()
        {
            if (Input.GetMouseButtonDown((int)MouseButton.MiddleMouse))
            {
                OnBeginDrag(new PointerEventData(EventSystem.current));
            }
            if (Input.GetMouseButtonUp((int) MouseButton.MiddleMouse))
            {
                OnEndDrag(new PointerEventData(EventSystem.current));
            }

            if (Input.GetMouseButton((int) MouseButton.MiddleMouse))
            {
                OnDrag(new PointerEventData(EventSystem.current));
            }

        
        }
        public void OnDrag(PointerEventData eventData)
        {
            // Debug.LogFormat("Drag!  MouseAxis X: {0}  MouseAxis Y: {1}", Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            _mainCamera.transform.Translate(Input.GetAxis("Mouse X") * -1, Input.GetAxis("Mouse Y") * -1, 0);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
             Debug.LogFormat("Begin Drag!  MouseAxis X: {0}  MouseAxis Y: {1}", Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.LogFormat("End Drag!  MouseAxis X: {0}  MouseAxis Y: {1}", Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace UI
{
    public class MapViewer : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private Camera _mainCamera;

        //private Vector3 startMousePos;

        //void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        startMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        startMousePos.z = 0.0f;
        //    }

        //    if (Input.GetMouseButton(0))
        //    {
        //        Vector3 nowMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //        nowMousePos.z = 0.0f;
        //        transform.position += startMousePos - nowMousePos;
        //    }
        //}
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

            if (Input.mouseScrollDelta.y < 0)
            {
                if (_mainCamera.orthographicSize < 50)
                {
                    _mainCamera.orthographicSize++;
                }
                //Debug.LogFormat("Mouse Down!:{0}", Input.mouseScrollDelta.y);
            }

            if (Input.mouseScrollDelta.y> 0)
            {
                if (_mainCamera.orthographicSize > 10)
                {
                    _mainCamera.orthographicSize--;
                }
                //Debug.LogFormat("Mouse Up!:{0}", Input.mouseScrollDelta.y);
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

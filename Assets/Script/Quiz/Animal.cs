using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Script.Quiz
{
    public class Animal : MonoBehaviour, IDragHandler, IDropHandler, IBeginDragHandler, IEndDragHandler
    {
        public Image icon;
        public string desc;
        public bool isFlying, isInsect, isOmnivorous, isLivesInGroup, isLaysEggs;
        
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;
        private Vector3 _lastpos;
        private Transform _parent;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _lastpos = transform.position;
            _parent = transform.parent;
        }
        
        public void OnBeginDrag(PointerEventData eventData) => transform.SetParent(Bucket.Instance.done);

        public void OnDrag(PointerEventData eventData) => _rectTransform.anchoredPosition += eventData.delta / Bucket.Instance.canvas.scaleFactor;
        
        public void OnEndDrag(PointerEventData eventData) => transform.SetParent(_parent);

        public void OnDrop(PointerEventData eventData) => transform.position = _lastpos;
    }
}

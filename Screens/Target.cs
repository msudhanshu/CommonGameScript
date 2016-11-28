using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace Album3D {

	//TODO
	// to define level/toughness of target we can have a interface which will be implemented by all factor
	// like block,screen effect, screen, image   to calculate final toughness

	//or it has link to gridblock,screen,imagedetail...etc

	public class Target : CoreBehaviour ,IPointerClickHandler,IBeginDragHandler /*,IDragHandler,IEndDragHandler*/ {

		//TEMP . TODO 
		Screen screen;

		void Start () {
			TargetManager.GetInstance ().AddTarget (this);
			screen = GetComponent<Screen>();
		}

		public string ShownName() {
			return screen.ImageName;
		}

		public bool IsValidTarget() {
			return !(screen.ImageName==null || screen.ImageName.Equals(""));
		}

		virtual public void OnPointerClick (PointerEventData eventData){
			EventClicked(eventData);
		}
		
		virtual public void OnBeginDrag(PointerEventData eventData) {
			EventClicked(eventData);
		}
		
//		virtual public void OnDrag(PointerEventData eventData) {
//			EventClicked(eventData);
//		}
//		
//		virtual public void OnEndDrag(PointerEventData eventData) {
//			EventClicked(eventData);
//		}

		public void EventClicked(PointerEventData eventData) {
			TargetManager.GetInstance().ClickedTarget(this);
		}
	}
}
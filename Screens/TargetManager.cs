using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Album3D {

public class TargetManager : Manager<TargetManager> {

		//TEMP TODO
		public Text targetName;
		void Update() {
			if(activeTarget==null && allTargets.Count!=0) {
				InitiateNewTarget();
			}
		}


	public bool temploadflag = false;
	List<Target> allTargets = new List<Target> ();

	public Target activeTarget = null;

	/* Implement this function */
	override public void StartInit(){

	}
	
	/* Implement this function */
	override public void PopulateDependencies(){
			//dependencies.Add(ScreenManager);
		}

	public void AddTarget(Target target) {
		allTargets.Add (target);
	}
	
	public void ClickedTarget(Target target) {
			if(target == activeTarget) {
				allTargets.Remove(target);
					//TODO
		     //calculate score....

				Destroy(target.gameObject);
				InitiateNewTarget();
			}
	}

	public void InitiateNewTarget() {
			//randomaly pick one among all and make it active target... show its name on top/questquide
			if(allTargets.Count>0) {
					activeTarget = allTargets[ Random.Range(0,allTargets.Count) ];
					if(activeTarget.IsValidTarget()) {
						targetName.text = activeTarget.ShownName();
					Debug.Log("need to click/find= "+activeTarget.ShownName());
				} else
					activeTarget = null;
			} else
				Debug.LogError("No target available");
	}

}

}


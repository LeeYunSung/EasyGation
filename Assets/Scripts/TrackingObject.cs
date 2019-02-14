using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TrackingObject : MonoBehaviour, ITrackableEventHandler {

    private TrackableBehaviour mTrackableBehaviour;

    public Animation obj_animation_;
    public bool is_detected_ = false;
    public string name_;
    public int floor_;
    public int x, y;
    private double distance;

	// Use this for initialization
	void Start () {
        mTrackableBehaviour = GetComponent<TrackableBehaviour>();
        if(mTrackableBehaviour){
            mTrackableBehaviour.RegisterTrackableEventHandler(this);
        }
	}

    public void OnTrackableStateChanged(TrackableBehaviour.Status previousStatus, TrackableBehaviour.Status newStatus){
        if(newStatus == TrackableBehaviour.Status.DETECTED || newStatus == TrackableBehaviour.Status.TRACKED){
            is_detected_ = true;
        }else{
            is_detected_ = false;
        }
    }	
}

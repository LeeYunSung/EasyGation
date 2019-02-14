/*==============================================================================
Copyright (c) 2010-2014 Qualcomm Connected Experiences, Inc.
All Rights Reserved.
Confidential and Proprietary - Protected under copyright and other laws.
==============================================================================*/

using UnityEngine;

namespace Vuforia
{
    /// <summary>
    /// A custom handler that implements the ITrackableEventHandler interface.
    /// </summary>
    public class DefaultTrackableEventHandler2 : MonoBehaviour,
                                                ITrackableEventHandler
    {
        #region PRIVATE_MEMBER_VARIABLES
        private TrackableBehaviour mTrackableBehaviour;
        #endregion // PRIVATE_MEMBER_VARIABLES
        #region UNTIY_MONOBEHAVIOUR_METHODS
    
        void Start()
        {
            mTrackableBehaviour = GetComponent<TrackableBehaviour>();
            if (mTrackableBehaviour)
            {
                mTrackableBehaviour.RegisterTrackableEventHandler(this);
            }
        }

        private int a,b,c,d;

        #endregion // UNTIY_MONOBEHAVIOUR_METHODS

        #region PUBLIC_METHODS

        /// <summary>
        /// Implementation of the ITrackableEventHandler function called when the
        /// tracking state changes.
        /// </summary>
        public void OnTrackableStateChanged(
                                        TrackableBehaviour.Status previousStatus,
                                        TrackableBehaviour.Status newStatus)
        {
            

            if (newStatus == TrackableBehaviour.Status.DETECTED ||
                newStatus == TrackableBehaviour.Status.TRACKED ||
                newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
            {
                Debug.Log("Tracking Start");
                OnTrackingFound();
                Debug.Log("Tracking End");
            }
            else
            {
                Debug.Log("Lost Start");
                OnTrackingLost();
                Debug.Log("Lost End");
            }
        }

        #endregion // PUBLIC_METHODS

        #region PRIVATE_METHODS


        private void OnTrackingFound()
        {

            a = Connector.a;
            b = Connector.b;
            d = Connector.d;

            Debug.Log("Start : " + d);

            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Enable rendering:
            foreach (Renderer component in rendererComponents)
            {

                TextMesh textObject;
                textObject = GameObject.Find("Arrow_info").GetComponent<TextMesh>();

                if (mTrackableBehaviour.TrackableName == "crocdile")
                {
                    c = 0;
                    d = Connector.d;
                    Debug.Log("Test : " + d);

                    if (d == 1)
                    {   //right
                        textObject.text = "Turn right";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;
                        component.enabled = true;
                    }
                    else if (d == 2)
                    {
                        //front
                        //textObject.GetComponent(TextMesh).text = "Go Stright";
                        textObject.text = "Go Stright";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                    else if (c == b)
                    {
                        //star
                        textObject.text = "Success";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = false;
                        rendererComponents[1].enabled = true;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                    else
                    {
                        //back
                        textObject.text = "Turn back";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                }
                else if (mTrackableBehaviour.TrackableName == "turtle")
                {
                    c = 1;
                    d = Connector.d;
                    Debug.Log("Test : " + d);

                    textObject = GameObject.Find("Arrow_info").GetComponent<TextMesh>();
                    if (d == 0)
                    {
                        //left
                        textObject.text = "Turn left";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                    else if (d == 5)
                    {
                        //right
                        textObject.text = "Turn right";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (c == b)
                    {
                        //star
                        textObject.text = "Success";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = false;
                        rendererComponents[1].enabled = true;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else
                    {
                        //?
                        textObject.text = "Turn back";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                }
                else if (mTrackableBehaviour.TrackableName == "bird")
                {
                    c = 2;
                    d = Connector.d;
                    Debug.Log("Test : " + d);

                    textObject = GameObject.Find("Arrow_info").GetComponent<TextMesh>();

                    if (d == 0)
                    {
                        //back
                        textObject.text = "Turn back";
                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));
                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (d == 4)
                    {
                        //front
                        textObject.text = "Go Stright";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (d == 2)
                    {
                        Debug.Log("TextMesh");
                        GameObject arrow = GameObject.Find("/2/arrow");
                        GameObject star = GameObject.Find("/2/star");
                        GameObject arrow_info = GameObject.Find("/2/Arrow_info");
                        //right
                        textObject.text = "Turn right";
                        arrow_info.GetComponent<TextMesh>().text = "Turn Right";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                    else if (c == b)
                    {
                        //star
                        textObject.text = "Success";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = false;
                        rendererComponents[1].enabled = true;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                    else
                    {
                        //?
                        textObject.text = "Turn back";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                }

                else if (mTrackableBehaviour.TrackableName == "monkey")
                {
                    c = 3;
                    d = Connector.d;
                    Debug.Log("Test : " + d);

                    textObject = GameObject.Find("Arrow_info").GetComponent<TextMesh>();

                    if (d == 2)
                    {
                        //back
                        textObject.text = "Go Stright";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (c == b)
                    {
                        //star
                        textObject.text = "Success";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = false;
                        rendererComponents[1].enabled = true;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else
                    {
                        //?
                        textObject.text = "Turn back";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                }

                else if (mTrackableBehaviour.TrackableName == "graff")
                {
                    c = 4;
                    d = Connector.d;
                    Debug.Log("Test : " + d);

                    textObject = GameObject.Find("Arrow_info").GetComponent<TextMesh>();

                    if (d == 2)
                    {
                        //back
                        textObject.text = "Turn back";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (d == 6)
                    {
                        //right
                        textObject.text = "Turn right";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (c == b)
                    {
                        //star
                        textObject.text = "Success";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = false;
                        rendererComponents[1].enabled = true;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else
                    {
                        //?
                        textObject.text = "Turn back";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                }

                else if (mTrackableBehaviour.TrackableName == "penguin")
                {
                    c = 5;
                    d = Connector.d;
                    Debug.Log("Test : " + d);

                    textObject = GameObject.Find("Arrow_info").GetComponent<TextMesh>();

                    if (d == 1)
                    {
                        //back
                        textObject.text = "Turn back";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (d == 6)
                    {
                        //left

                        textObject.text = "Turn left";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (c == b)
                    {
                        //star
                        textObject.text = "Success";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = false;
                        rendererComponents[1].enabled = true;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else
                    {
                        //?
                        textObject.text = "Turn back";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                }

                else if (mTrackableBehaviour.TrackableName == "elephant")
                {
                    c = 6;
                    d = Connector.d;
                    Debug.Log("Test : " + d);

                    textObject = GameObject.Find("Arrow_info").GetComponent<TextMesh>();

                    if (d == 4)
                    {
                        //left
                        textObject.text = "Turn left";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (d == 5)
                    {
                        //right
                        textObject.text = "Turn right";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else if (c == b)
                    {
                        //star
                        textObject.text = "Success";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));

                        rendererComponents[0].enabled = false;
                        rendererComponents[1].enabled = true;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }

                    else
                    {
                        //?
                        textObject.text = "Turn back";

                        component.transform.rotation = Quaternion.Euler(new Vector3(0, -180, 0));

                        rendererComponents[0].enabled = true;
                        rendererComponents[1].enabled = false;
                        rendererComponents[2].enabled = true;

                        component.enabled = true;
                    }
                }
            }
            Debug.Log("End");
            // Enable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = true;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
        }

        private void OnTrackingLost()
        {
            Renderer[] rendererComponents = GetComponentsInChildren<Renderer>(true);
            Collider[] colliderComponents = GetComponentsInChildren<Collider>(true);

            // Disable rendering:
            foreach (Renderer component in rendererComponents)
            {
                component.enabled = false;
            }

            // Disable colliders:
            foreach (Collider component in colliderComponents)
            {
                component.enabled = false;
            }

            Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");

        }
        #endregion // PRIVATE_METHODS
    }
}

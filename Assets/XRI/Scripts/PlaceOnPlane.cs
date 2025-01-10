using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;  // se necessario per gestire UI

public class PlaceOnPlane : MonoBehaviour
{
    public GameObject objectToPlace;
    private ARRaycastManager raycastManager;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // Se l’utente tocca lo schermo
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // In questo esempio, posizioniamo l'oggetto al primo tocco
            if (touch.phase == TouchPhase.Began)
            {
                // Eseguiamo un raycast per vedere se abbiamo toccato un piano AR
                List<ARRaycastHit> hits = new List<ARRaycastHit>();
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    // Prendiamo il primo risultato e posizioniamo l’oggetto
                    Pose hitPose = hits[0].pose;

                    // Se l’oggetto non è ancora stato istanziato
                    if (objectToPlace == null) return;

                    Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
                }
            }
        }
    }
}

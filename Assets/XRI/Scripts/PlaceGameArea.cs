using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceGameArea : MonoBehaviour
{
    [Tooltip("Il prefab che definisce l'area di gioco (0.5x0.8m).")]
    public GameObject gameAreaPrefab;

    private ARRaycastManager raycastManager;
    private bool areaPlaced = false;
    private GameObject placedGameArea;

    public GameObject PlacedGameArea => placedGameArea;
    // Proprietà pubblica per permettere ad altri script
    // di accedere all'oggetto piazzato (se necessario).

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // Se l'area è già stata piazzata, non facciamo nulla
        if (areaPlaced) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // Raycast verso i piani AR
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;

                    // Istanzia l'area di gioco
                    placedGameArea = Instantiate(gameAreaPrefab, hitPose.position, hitPose.rotation);

                    areaPlaced = true;
                }
            }
        }
    }
}

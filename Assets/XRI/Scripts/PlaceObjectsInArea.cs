using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectsInArea : MonoBehaviour
{
    [Tooltip("Prefab dell'oggetto da piazzare")]
    public GameObject itemPrefab;

    [Tooltip("Riferimento allo script che piazza l'area di gioco")]
    public PlaceGameArea placeGameAreaScript;

    private ARRaycastManager raycastManager;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // Se l'area di gioco non è ancora piazzata, non fare nulla
        if (placeGameAreaScript == null || placeGameAreaScript.PlacedGameArea == null)
            return;

        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        // Quando inizia il tocco...
        if (touch.phase == TouchPhase.Began)
        {
            // Raycast verso i piani AR
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;

                // Calcola la posizione locale rispetto all'area di gioco
                GameObject gameArea = placeGameAreaScript.PlacedGameArea;
                Vector3 localPos = gameArea.transform.InverseTransformPoint(hitPose.position);

                // Se l'area è scalata a (0.5, 1, 0.8), allora la "metà" in X è 0.25 e in Z è 0.4
                // (perché 0.5 x 0.8 m = dimensione "totale" dell'area)
                float halfX = 0.25f;
                float halfZ = 0.4f;

                // Verifichiamo se localPos.x e localPos.z rientrano nella bounding box
                if (Mathf.Abs(localPos.x) <= halfX && Mathf.Abs(localPos.z) <= halfZ)
                {
                    // Siamo dentro l'area, piazziamo l'oggetto
                    Instantiate(itemPrefab, hitPose.position, gameArea.transform.rotation);
                }
                else
                {
                    Debug.Log("Tocco fuori dalla bounding box dell'area, oggetto non piazzato.");
                }
            }
        }
    }
}

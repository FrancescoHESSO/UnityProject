
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceAreaAndObject : MonoBehaviour
{
    [Header("Prefab dell'area di gioco (0.5 x 0.8 m)")]
    public GameObject gameAreaPrefab;

    [Header("Prefab degli oggetti da posizionare")]
    public GameObject itemPrefab;

    private ARRaycastManager raycastManager;
    private GameObject placedGameArea;
    private bool areaPlaced = false;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        // Se non c'è alcun tocco, interrompi
        if (Input.touchCount == 0) return;

        Touch touch = Input.GetTouch(0);

        // Consideriamo solo il tocco all'inizio (phase == Began)
        if (touch.phase == TouchPhase.Began)
        {
            // Facciamo un raycast AR: colpisce un piano AR?
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                // Prendi la prima collisione
                Pose hitPose = hits[0].pose;

                // Se l'area non è ancora stata piazzata...
                if (!areaPlaced)
                {
                    // ...instanzia l'area di gioco
                    placedGameArea = Instantiate(gameAreaPrefab, hitPose.position, hitPose.rotation);
                    areaPlaced = true;
                }
                else
                {
                    // L'area è già piazzata.  
                    // 1) Verifichiamo se il tocco cade "dentro" l'area di gioco  
                    //    assumendo che la GameArea sia 0.5 m in X e 0.8 m in Z.

                    // Calcoliamo la posizione locale rispetto all'oggetto "placedGameArea"
                    Vector3 localPos = placedGameArea.transform.InverseTransformPoint(hitPose.position);

                    // Metà dimensioni (se l'area è 0.5 x 0.8, allora la metà è 0.25 x 0.4)
                    float halfX = 0.25f;
                    float halfZ = 0.4f;

                    // Controlliamo se il tocco rientra nel rettangolo locale
                    // (attenzione: assumiamo un'area orizzontale, non inclinata)
                    if (Mathf.Abs(localPos.x) <= halfX && Mathf.Abs(localPos.z) <= halfZ)
                    {
                        // Se siamo dentro l'area, piazziamo l'oggetto
                        Instantiate(itemPrefab, hitPose.position, placedGameArea.transform.rotation);
                    }
                    else
                    {
                        Debug.Log("Tocco fuori dall'area di gioco, oggetto non piazzato.");
                    }
                }
            }
        }
    }
}


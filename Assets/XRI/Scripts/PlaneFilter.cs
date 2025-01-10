using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;

public class PlaneFilter : MonoBehaviour
{
    [SerializeField] ARPlaneManager planeManager;

    [Header("Dimensione minima (metri)")]
    [SerializeField] float minSizeX = 0.5f;
    [SerializeField] float minSizeY = 0.5f;

    void OnEnable()
    {
        // Per AR Foundation < 6.0
        planeManager.planesChanged += OnPlanesChanged;

        // Se usi ARFoundation >= 6.0 e l’evento planesChanged è deprecato,
        // puoi usare planeManager.trackablesChanged += OnTrackablesChanged; 
        // e adattare il codice di conseguenza.
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // Filtra i piani aggiunti
        FilterPlanes(args.added);

        // E se vuoi rivedere anche quelli aggiornati
        FilterPlanes(args.updated);
    }

    void FilterPlanes(List<ARPlane> planes)
    {
        foreach (var plane in planes)
        {
            // plane.extents.x / y ci dice la "metà" della dimensione in metri
            // Quindi se plane.extents.x < 0.5f, significa dimensione totale < 1m in orizzontale
            if (plane.extents.x < minSizeX || plane.extents.y < minSizeY)
            {
                // Disattiva il GameObject, così non appare a schermo
                plane.gameObject.SetActive(false);
            }
            else
            {
                plane.gameObject.SetActive(true);
            }
        }
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TetrisGenerator : MonoBehaviour
{
    [SerializeField] private CameraMovement cameraMovement;
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private GameUi gameUi;
    
    [Space]
    [SerializeField] private GameObject projection;
    [SerializeField] private GameObject startPlane;
    [SerializeField] private List<GameObject> tetrisObjectPrefabs;
    [SerializeField] private Color[] tetrisColors;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float generatorStartDelay = 2f;
    
    public Action OnTetrisObjectGenerated;

    private List<GameObject> tetrisObjects;
    private TetrisObject previousTetrisObject;
    private TetrisObject currentTetrisObject;

    private int currentDistance;
    
    private bool isPaused;

    private void Awake()
    {
        tetrisObjects = new List<GameObject>();

        stateMachine.OnGameStarted += StartTetrisGenerator;
        stateMachine.OnGameEnded += StopTetrisGenerator;
        stateMachine.OnMainMenuOpened += DeleteAllTetrisObjects;
        stateMachine.OnGameResumed += ResumeTetrisGenerator;
        stateMachine.OnGamePaused += PauseTetrisGenerator;
        stateMachine.OnGameRestarted += CreateStartPlane;
    }

    private void StartTetrisGenerator()
    {
        InvokeRepeating(nameof(TryCreateTetrisObject), generatorStartDelay, 1f);
    }

    private void StopTetrisGenerator()
    {
        CancelInvoke(nameof(TryCreateTetrisObject));
    }
    
    private void PauseTetrisGenerator()
    {
        isPaused = true;
    }
    
    private void ResumeTetrisGenerator()
    {
        isPaused = false;
    }

    private void TryCreateTetrisObject()
    {
        if (isPaused)
            return;
        
        if (currentTetrisObject != null)
        {
            if (!currentTetrisObject.IsPlaced())
                return;
            
            previousTetrisObject = currentTetrisObject;
            if(currentDistance < currentTetrisObject.GetTetrisMaxPoint())
                currentDistance = currentTetrisObject.GetTetrisMaxPoint();
            gameUi.UpdateDistanceText(currentDistance);
        }
        
        CreateTetrisObject();
    }

    private void CreateTetrisObject()
    {
        int randomIndex = Random.Range(0, tetrisObjectPrefabs.Count);

        var tetrisObject = Instantiate(tetrisObjectPrefabs[randomIndex], spawnPoint.position, spawnPoint.rotation, transform);
        currentTetrisObject =  tetrisObject.GetComponent<TetrisObject>();
        currentTetrisObject.projectionBeam = projection;
        currentTetrisObject.SetCameraMovement(cameraMovement);
        tetrisObjects.Add(tetrisObject);
        OnTetrisObjectGenerated?.Invoke();
    }

    private void DeleteAllTetrisObjects()
    {
        foreach (var item in tetrisObjects)
        {
            Destroy(item);
        }
        ResetCurrentDistance();
    }

    private void CreateStartPlane()
    {
        currentTetrisObject = null;
        StartTetrisGenerator();
        
        if(previousTetrisObject == null)
            return;
        
        Vector3 startPlanePosition = Vector3.up * currentDistance;
        var startPlaneObject = Instantiate(startPlane, startPlanePosition, Quaternion.identity, transform);
        tetrisObjects.Add(startPlaneObject);
        
        cameraMovement.CheckTowerTopPosition();
    }

    private void ResetCurrentDistance()
    {
        currentDistance = 0;
    }

    public TetrisObject GetCurrentTetrisObject()
    {
        return currentTetrisObject;
    }
}
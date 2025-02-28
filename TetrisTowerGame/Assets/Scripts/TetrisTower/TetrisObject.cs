using System;
using UnityEngine;

public class TetrisObject : MonoBehaviour
{
    [SerializeField] private Collider projectionCollider;
    
    public GameObject projectionBeam;
    
    private CameraMovement cameraMovement;
    
    private Rigidbody rb;
    private Vector3 lastPosition;
    private bool placed;
    private bool transformBlocked;

    private LayerMask blockingLayer;
    
    private void Start()
    {
        blockingLayer = LayerMask.GetMask("TetrisObjects");
        
        rb = GetComponent<Rigidbody>();
        lastPosition = transform.position;
        placed = false;
        transformBlocked = false;
        InvokeRepeating(nameof(DisableRigidBody), 1f, 0.5f);

        if (projectionBeam != null)
        {
            projectionBeam.SetActive(true);
        }
    }

    private void Update()
    {
        if(projectionBeam == null)
            return;
        
        if (placed)
        {
            projectionBeam = null;
        }
        else if (!transformBlocked && projectionCollider != null)
        {
            UpdateProjection();
        }
        
    }

    private void UpdateProjection()
    {
        Vector3 colliderSize = projectionCollider.bounds.size;
        Vector3 colliderCenter = projectionCollider.bounds.center;

        Vector3 boxHalfExtents = new Vector3((colliderSize.x - 0.1f) / 2, 0.1f, (colliderSize.z - 0.1f) / 2);
        Vector3 startRayPosition = new Vector3(colliderCenter.x, colliderCenter.y - colliderSize.y / 2, colliderCenter.z);
        Vector3 rayDirection = Vector3.down;

        float maxDistance = 100f;

        if (Physics.BoxCast(startRayPosition, boxHalfExtents, rayDirection, out RaycastHit hit, Quaternion.identity, maxDistance, blockingLayer))
        {
            float highestY = hit.point.y;
            float beamHeight = Mathf.Max(0.1f, transform.position.y - colliderSize.y / 2 - highestY);

            if (beamHeight <= 0.3f)
            {
                projectionBeam.SetActive(false);
                projectionBeam = null;
                return;
            }

            projectionBeam.transform.position = new Vector3(colliderCenter.x, highestY + beamHeight / 2, colliderCenter.z);
            projectionBeam.transform.localScale = new Vector3(colliderSize.x - 0.1f, beamHeight, colliderSize.z - 0.1f);
        }
    }

    private void DisableRigidBody()
    {
        if (lastPosition == transform.position)
        {
            //rb.isKinematic = true;
            placed = true;
            CancelInvoke(nameof(DisableRigidBody));
            cameraMovement.CheckTowerTopPosition();
            return;
        }

        lastPosition = transform.position;
    }
    
    public void BlockTransform()
    {
        transformBlocked = true;
        rb.drag = 0;
        if (projectionBeam != null)
        {
            projectionBeam.SetActive(false);
            projectionBeam = null;
        }
    }

    public void Rotate()
    {
        if (!transformBlocked)
            transform.Rotate(Vector3.right, 90);
    }

    public bool IsPlaced() => placed;

    public bool IsTransformBlocked() => transformBlocked;

    public void SetCameraMovement(CameraMovement _cameraMovement)
    {
        cameraMovement = _cameraMovement;
    }

    public int GetTetrisMaxPoint() => Mathf.RoundToInt(transform.position.y + projectionCollider.bounds.size.y / 2);

    public Collider GetTetrisCollider() => projectionCollider;
}

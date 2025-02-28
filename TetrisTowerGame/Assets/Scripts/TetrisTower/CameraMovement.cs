using DG.Tweening;
using TMPro;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    [SerializeField] private float moveDistance;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private float startPosition;
    
    [SerializeField] private TextMeshProUGUI distanceText;
    
    private void Awake()
    {
        stateMachine.OnMainMenuOpened += ResetCameraPosition;
    }

    private void Start()
    {
        ResetCameraPosition();
        boxCollider.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("TetrisObject"))
        {
            var newPos = transform.position;
            newPos.y += moveDistance;
            transform.position = newPos;
            distanceText.text = newPos.y + "m";
            boxCollider.enabled = false;
        }
    }

    private void ResetCameraPosition()
    {
        var vector3 = transform.position;
        vector3.y = startPosition;
        transform.position = vector3;
        distanceText.text = vector3.y + "m";
    }
    
    
    public void CheckTowerTopPosition()
    {
        boxCollider.enabled = true;
        DOVirtual.DelayedCall(0.3f, () => boxCollider.enabled = false);
    }
}

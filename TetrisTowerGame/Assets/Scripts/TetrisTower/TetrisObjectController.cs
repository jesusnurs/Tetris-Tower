using UnityEngine;

public class TetrisObjectController : MonoBehaviour
{
    [SerializeField] private TetrisGenerator tetrisGenerator;
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;

    private TetrisObject currentTetrisObject;
    private Collider currentTetrisObjectCollider;

    private Vector2 touchStartPos;
    private bool isDragging;
    public float speed = 0.1f;

    private void Awake()
    {
        tetrisGenerator.OnTetrisObjectGenerated += SetCurrentTetrisObject;
    }

    void Update()
    {
        if(currentTetrisObject == null)
            return;
        
        if(currentTetrisObject.IsTransformBlocked())
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            touchStartPos = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                touchStartPos = touch.position;
            }
        }

        if (isDragging)
        {
            var tetrisTransform = currentTetrisObject.transform;

            Vector2 currentPos = (Input.touchCount > 0) ? (Vector2)Input.GetTouch(0).position : (Vector2)Input.mousePosition;
            Vector2 offset = currentPos - touchStartPos;

            Vector2 newPosition = tetrisTransform.localPosition + new Vector3(offset.x * speed, 0f, 0f);
            newPosition.x = Mathf.Clamp(newPosition.x, minX + (currentTetrisObjectCollider.bounds.size.x - 0.1f) / 2, maxX - (currentTetrisObjectCollider.bounds.size.x - 0.1f) / 2);

            tetrisTransform.localPosition = newPosition;

            touchStartPos = currentPos;
        }
        else
        {
            CheckTetrisObjectPosition();
        }

        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false;
        }
    }

    private void CheckTetrisObjectPosition()
    {
        var tetrisTransform = currentTetrisObject.transform;
        
        Vector2 newPosition = tetrisTransform.localPosition;
        newPosition.x = Mathf.Clamp(newPosition.x, minX + (currentTetrisObjectCollider.bounds.size.x - 0.1f) / 2, maxX - (currentTetrisObjectCollider.bounds.size.x - 0.1f) / 2);

        tetrisTransform.localPosition = newPosition;
    }

    public void RotateTetrisObject()
    {
        currentTetrisObject.Rotate();
    }

    private void SetCurrentTetrisObject()
    {
        currentTetrisObject = tetrisGenerator.GetCurrentTetrisObject();
        currentTetrisObjectCollider = currentTetrisObject.GetTetrisCollider();
    }
}

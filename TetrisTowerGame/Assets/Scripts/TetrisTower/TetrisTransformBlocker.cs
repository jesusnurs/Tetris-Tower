using UnityEngine;

public class TetrisTransformBlocker : MonoBehaviour
{
    private TetrisObject tetrisObject;
    
    private void Start()
    {
        tetrisObject = transform.parent.GetComponent<TetrisObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (tetrisObject.IsPlaced())
            return;

        if (other.CompareTag("TransformBlocker"))
        {
            tetrisObject.BlockTransform();
        }
    }
}

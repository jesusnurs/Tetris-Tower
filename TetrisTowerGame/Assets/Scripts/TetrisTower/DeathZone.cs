using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField] private StateMachine stateMachine;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TetrisObject" && !other.GetComponent<Rigidbody>().isKinematic)
        {
            stateMachine.EndGame();
            Destroy(other.gameObject);
        }
    }
}

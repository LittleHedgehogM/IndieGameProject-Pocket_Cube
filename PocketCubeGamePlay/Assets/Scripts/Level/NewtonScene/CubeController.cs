using UnityEngine;

public class CubeController : MonoBehaviour
{
    Rigidbody rb;

    private bool isFalling = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    public void startsFalling()
    {
        //previousPosition = transform.position;
        isFalling = true;
        rb.isKinematic = false;
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entered collision with " + collision.gameObject.name);
    }

    // Gets called during the collision
    void OnCollisionStay(Collision collision)
    {
        Debug.Log("Colliding with " + collision.gameObject.name);
        isFalling = false;
        rb.isKinematic = true;

    }

    

    public bool getIsFalling()
    {
        return isFalling;

    }


}

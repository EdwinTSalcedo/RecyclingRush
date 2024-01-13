using UnityEngine;

public class Floats : MonoBehaviour
{
    /* This script is active on the "boat" GameObject, within each child element
     that acts to float at certain points on the boat.
     This script regulates the level to which the object is submerged. */
    
    // Reference to the boat's Rigidbody
    public GameObject boatObject;
    
    // Reference to the GameObject that acts as the water
    public GameObject water;
    
    // Water level (buoyancy level)
    public float waterLevel;
    
    // Buoyancy threshold that determines the growth of buoyant force
    public float waterThreshold;
    
    // Density of water affecting the change in the direction of buoyant force
    public float waterDensity;
    
    // Additional force applied to counteract gravity
    public float downwardForce;
    
    // Factor representing buoyant force
    float buoyantForceFactor;

    // Total force applied to the object
    public Vector3 buoyantForce;

    private void Start()
    {
        // Set the initial water level based on the water GameObject's position
        waterLevel = water.transform.position.y;
    }

    private void FixedUpdate()
    {
        // Calculate the buoyant force factor based on the object's position relative to the water
        buoyantForceFactor = 1.0f - ((transform.position.y - waterLevel) / waterThreshold);

        // Check if the buoyant force is applicable
        if (buoyantForceFactor > 1.0f)
        {
            // Calculate the buoyant force
            buoyantForce = -Physics.gravity * (buoyantForceFactor - boatObject.GetComponent<Rigidbody>().velocity.y * waterDensity);
            
            // Add additional downward force
            buoyantForce += new Vector3(0.0f, -downwardForce, 0.0f);
            
            // Apply the buoyant force to the object at its position
            boatObject.GetComponent<Rigidbody>().AddForceAtPosition(buoyantForce, transform.position);
        }
    }
}

using UnityEngine;

public class ThermResolver : MonoBehaviour
{
    

    // Update is called once per frame
    void FixedUpdate()
    {
        /***
         * Get all collisions.
         * Decide which are Thermobjects.
         * Get their temperatures and thermal conductivities.
         * 
         * use the following equation
         * T(t) = T0 + (T1 - T0) * (e^(-k*t))
         * 
         * maybe derive?
         * T'(t) = -k * (T1 - T0) * e^(-k*t)
         * 
         * t = time since contact
         * T1 = temperature of other object
         * T0 = temperature of this object at time of contact
         * k = thermal conductivity of other object
         * 
         * sum each component for each contact.
         * 
         */
    }
}

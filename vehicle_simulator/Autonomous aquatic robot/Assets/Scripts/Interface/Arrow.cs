using UnityEngine;

public class Arrow : MonoBehaviour
{
    // Referencia al script AutonomousMovement
    public AutonomousMovement autonomousMovement;

    // Velocidad máxima de rotación en grados por segundo
    public float maxRotationSpeed = 90f;

    // Actualización del objeto en cada frame
    void Update()
    {
        // Obtener el ángulo de predicción del script AutonomousMovement
        float anglePrediction = autonomousMovement.anglePrediction;

        // Obtener la rotación actual y la rotación deseada
        Quaternion currentRotation = transform.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, anglePrediction, 0f);

        // Calcular la rotación suave hacia el ángulo deseado
        float step = maxRotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.RotateTowards(currentRotation, targetRotation, step);
    }
}

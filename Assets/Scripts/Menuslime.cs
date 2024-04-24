using UnityEngine;

public class Menuslime : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Velocidad del objeto en unidades por segundo
    [SerializeField] private float durationForward = 5f; // Duration to move forward
    private float timer = 0f;
    private bool movingForward = true;

    // Update is called once per frame
    void Update()
    {
        // Increment timer
        timer += Time.deltaTime;

        if (movingForward)
        {
            // Movimiento hacia la derecha
            transform.Translate(Vector3.right * speed * Time.deltaTime);

            // Check if forward duration is reached
            if (timer >= durationForward)
            {
                // Reset timer and switch direction
                timer = 0f;
                movingForward = false;
            }
        }
        else
        {
            // Movimiento hacia la izquierda (retroceso)
            transform.Translate(Vector3.left * speed * Time.deltaTime);

            // Check if backward duration is reached
            if (timer >= durationForward)
            {
                // Reset timer and switch direction
                timer = 0f;
                movingForward = true;
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float xBound = 5f;
    [SerializeField] private float yBound = 5f;

    private Vector2 direction;

    private void Start()
    {
        StartCoroutine(ChangeDirection());
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector2 position = transform.position;
        position += direction * speed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, -xBound, xBound);
        position.y = Mathf.Clamp(position.y, -yBound, yBound);

        transform.position = position;
    }

    private IEnumerator ChangeDirection()
    {
        while (true)
        {
            direction = new Vector2(Random.Range(-2f, 2f), Random.Range(-5f, 5f)).normalized;
            yield return new WaitForSeconds(1f);
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class Bullet : MonoBehaviour
{
    public float speed = 8f;
    public float bulletTime = 3f;
    public float direction = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
         transform.Translate(direction * speed * Time.deltaTime * Vector3.right);
         bulletTime -= Time.deltaTime;
         if (bulletTime <= 0)
         {
             Destroy(gameObject);
         }
    }
}

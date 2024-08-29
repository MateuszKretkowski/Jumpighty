using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonScript : MonoBehaviour
{
    [SerializeField] float delay;
    public float delayMax;
    public GameObject bulletPrefab;
    public Transform bulletPosition;
    public GameObject player;
    Transform playerTransform;

    public float speed;

    void Start()
    {
        delay = delayMax;
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (delay <= 0) 
        {
            StartCoroutine(fire());
            delay = delayMax;
        }
        else
        {
            delay -= Time.fixedDeltaTime;
        }
    }

    private IEnumerator fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletPosition.position, Quaternion.identity);

        // Ruch pocisku w stronê gracza
        // Oblicz kierunek z bullet do gracza
        playerTransform = player.transform;
        Vector3 direction = (playerTransform.position - bullet.transform.position).normalized;
        while (bullet != null) // Dzia³a dopóki pocisk istnieje
        {
            bullet.transform.position += direction * speed * Time.deltaTime; // 5f to prêdkoœæ, któr¹ mo¿esz dostosowaæ
            yield return null; 
        }

        Destroy(bullet, 12f);
    }
}

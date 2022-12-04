using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField]
    private FirerainController _fireRain;

    [SerializeField]
    private DragonBreathController _dragonBreath;

    void Start()
    {
        StartCoroutine(DragonBreath());
    }

    IEnumerator DragonBreath()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            var dragonBreath = Instantiate(_dragonBreath, transform.position, Quaternion.identity);
            dragonBreath.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
        }
    }

    IEnumerator FireRain()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            var x = Random.Range(-10f, 11f);
            var firerain = Instantiate(_fireRain, new Vector2(transform.position.x + x, transform.position.y), Quaternion.identity);
            firerain.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -90));
        }
    }
}

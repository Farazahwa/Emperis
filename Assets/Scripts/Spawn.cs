using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private GameObject _goblin;

    void Start()
    {
        StartCoroutine(SpawnGoblin());
    }

    IEnumerator SpawnGoblin()
    {
        for(int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1);
            var x = transform.position.x;
            var y = transform.position.y;

            Instantiate(_goblin, new Vector2(x,y), Quaternion.identity);
        }
    }
}

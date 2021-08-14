using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public List<MobHandler> MobList;
    public float spawnTime;

    [SerializeField] private float spawnRadios = 3f;

    private void Start()
    {
        StartCoroutine(TryToSpawnMob());
    }

    private IEnumerator TryToSpawnMob()
    {
        while (true)
        {
            foreach (MobHandler mob in MobList)
            {
                if(mob.CurrentMobs < mob.Entry.MaxMobNumber && Vector3.Distance(transform.position, PlayerManager.instance.Player.transform.position) > 120)
                {
                    var enemy = Instantiate(mob.Entry.Mob, transform);
                    Vector3 pos = transform.position;

                    enemy.spawner = this;
                    mob.CurrentMobs++;
                }

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(spawnTime);
        } 
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, spawnRadios);
    }
}

[System.Serializable]
public class MobHandler
{
    public MobEntry Entry;
    public int CurrentMobs;
}

[System.Serializable]
public class MobEntry
{
    public EnemyCharacter Mob;
    public int MaxMobNumber;
}

public enum Mobs
{
    Slime = 0,
    Ork = 1,
    Ent = 2
}

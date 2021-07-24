using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    public List<LootEntry> LootTable;

    [SerializeField] private float lootExplosionForce = 10f;

    public void InstantiateLoot()
    {
        foreach (LootEntry entry in LootTable)
        {
            for (int i = 0; i < entry.NumberOfItemsGiven(); i++)
            {
                var obj = Instantiate(entry.LootableItem);
                obj.transform.position = transform.position + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));


                var rb = obj.GetComponent<Rigidbody>();
                var forceVector = new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 3f), Random.Range(-1f, 1f)) ;
                Debug.Log(forceVector);
                rb.AddForce(forceVector * lootExplosionForce);
            }  
        }
    }
}

[System.Serializable]
public class LootEntry
{
    public GameObject LootableItem;
    public int MinItensGiven;
    public int MaxItensGiven;

    public int NumberOfItemsGiven()
    {
        return Random.Range(MinItensGiven, MaxItensGiven);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    
     [Header("Inventory Items")]
     public List<string> InventoryID;

    public int InventorySize;

    public int CurrentInventory;

    // Start is called before the first frame update
    void Start()
    {
        CurrentInventory=0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void AddToInventory(ActivatableObject ObjectID,Sprite UI_Icon,GameObject objPrefab)
    {
        if(CurrentInventory<=InventorySize)
        {
            if(ObjectID!=null)
            {
                InventoryID.Add(ObjectID.gameObject.name);
            }
            else
            {
                InventoryID.Add("00x00");
            }
            
            Destroy(objPrefab);

        }
    }

    public bool checkItems(string lookingFor)
    {
        foreach(string item in InventoryID)
        {
            if(GameManager.instance.ExtractNumbers(item)==GameManager.instance.ExtractNumbers(lookingFor))
            {
                RemoveFromInventory(item);
                return true;
            }
        }

        return false;
    }

    public void RemoveFromInventory(string ObjectID)
    {
        if(InventoryID.Contains(ObjectID))
        {
            InventoryID.Remove(ObjectID);
            CurrentInventory--;
        }
        
    }
}

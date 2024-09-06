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
        UIHandler.instance.setInventory(InventorySize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void AddToInventory(ActivatableObject ObjectID,Sprite UI_Icon,GameObject objPrefab)
    {
        if(objPrefab.name=="Upgrade_DoubleJump")
        {
            GameManager.instance.UpgradeDoubleJump();
            if(objPrefab!=null)
            {
                Destroy(objPrefab);
            }

            return;
        }
        else if(objPrefab.name=="Upgrade_Throw_Mechanic")
        {
            GameManager.instance.UpgradeThrowMechanic();
            if(objPrefab!=null)
            {
                Destroy(objPrefab);
            }

            return;
        }
        if(CurrentInventory<=InventorySize)
        {
            if(ObjectID!=null)
            {
                InventoryID.Add(ObjectID.gameObject.name);
                UIHandler.instance.addInventory(ObjectID.gameObject.name,UI_Icon);
            }
            else
            {
                InventoryID.Add("00x00");
            }
            
            if(objPrefab!=null)
            {
                Destroy(objPrefab);
            }


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
            UIHandler.instance.removeInventory(ObjectID);
            CurrentInventory--;
        }
        
    }
}

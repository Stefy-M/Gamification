using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TreasureManager : MonoBehaviour {

    public int num_rewards;             // Number of rewards
    public int[] rewards;               // Array of rewards ID

    
    public Sprite opened;
    public Sprite empty_chest;


    public GameObject treasure_items;
    public GameObject inventory;
    private GameObject descriptionBox;

    private bool beenChecked = false;

    void Start()
    {
       
    }

    void initializeChestContents()
    {
        descriptionBox = GameObject.Find("DescriptionBox");
        treasure_items = GameObject.Find("Loot");
        inventory = GameObject.Find("InventoryDungeon");
        rewards = new int[30];

        for (int x = 0; x < 30; x++)    // NEEDS FORMULA. Currently randomly generates items between 1 and 11
        {
            if (x >= num_rewards)
            {
                rewards[x] = 0;
            }
            else
            {
                float y = Random.Range(1.0f, 100.0f);
                //Could add scaling to this
                //75% chance of regular set items
                //20% chance of lower rank item;
                //5% chance of higher rank item;

                if (y < 75) //Regular set
                {
                    rewards[x] = GlobalControl.Instance.dungeon.regular_rewards[Random.Range(0, GlobalControl.Instance.dungeon.regular_rewards.Length)];
                }
                else if (y >= 75 && y <= 95) //Lower set
                {
                    rewards[x] = GlobalControl.Instance.dungeon.lower_rewards[Random.Range(0, GlobalControl.Instance.dungeon.lower_rewards.Length)];
                }
                else  //Higher set
                {
                    rewards[x] = GlobalControl.Instance.dungeon.higher_rewards[Random.Range(0, GlobalControl.Instance.dungeon.higher_rewards.Length)];
                }
            }
        }

    }


    void OnTriggerEnter2D(Collider2D other)         // Opens loot interface when sees chest
    {
        if (other.gameObject.name == "Player")
        {
            //Instead of initializing chest contents when the dungeon is loaded, do it here. This will increase performance (less load time)
            if(!beenChecked)
                initializeChestContents();
            if (num_rewards > 0)
            {
                this.GetComponent<SpriteRenderer>().sprite = opened;    // Changes looks of chest to signify it's been looked in
            }

            treasure_items.GetComponent<ItemManager>().show = true;
            treasure_items.GetComponent<ItemManager>().SetItems(rewards, this);    // Sets up the loot interface to see items in this chest
        }
        
    }

    void OnTriggerExit2D(Collider2D other)              // Closes the loot interface when leaving chest
    {
        if (other.gameObject.name == "Player")
        {
            if(!beenChecked)
                beenChecked = true;

            if (num_rewards <= 0)
            {
                this.GetComponent<SpriteRenderer>().sprite = empty_chest; // Changes looks of chest to be empty
            }
            else
            {
                this.GetComponent<SpriteRenderer>().sprite = opened;
            }

            treasure_items.GetComponent<ItemManager>().show = false;
            descriptionBox.SetActive(false);
        }
    }

    public void TakeItem(int item_slot)
    {
        int x = inventory.GetComponent<InventoryManager>().HasRoom();
        int y = rewards[item_slot];
        if (y != 0 && x >= 0)
        {
            inventory.GetComponent<InventoryManager>().GetItem(x, y);
            treasure_items.GetComponent<ItemManager>().RemoveItem(item_slot, GlobalControl.Instance.full_items_list[0].image);
            rewards[item_slot] = 0;
            num_rewards--;
        }
    }

    public void PlaceItem(int item_id, int slot, int from_slot)
    {
        num_rewards++;
        rewards[slot] = item_id;
        inventory.GetComponent<InventoryManager>().RemoveItem(from_slot);
    }
}

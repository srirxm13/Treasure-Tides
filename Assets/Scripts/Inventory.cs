using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int currentItem; //1 Sword 2 Compass 3 SpyGlass 4 Food 5 Rum 6 lantern 7 Instrument
    [SerializeField] GameObject[] Equipments;
    [SerializeField] Image[] InventoryIcons;
    [SerializeField] RectTransform bgIcon;
    // Start is called before the first frame update
    void Start()
    {
        currentItem = 0;
        Equipment();
    }

    // Update is called once per frame
    void Update()
    {
        #region Select Equipment
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            currentItem = 0;
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentItem = 1;
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentItem = 2;
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentItem = 3;
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentItem = 4;
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentItem = 5;
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentItem = 6;
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentItem = 7; 
            Equipment();
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentItem = 8;
            Equipment();
        }
        #endregion

        bgIcon.position = InventoryIcons[currentItem].rectTransform.position;
    }

    void Equipment()
    {
        for (int i = 0; i < Equipments.Length; i++)
        {
            Equipments[i].SetActive(false);
        }
        Equipments[currentItem].SetActive(true);
    }
}

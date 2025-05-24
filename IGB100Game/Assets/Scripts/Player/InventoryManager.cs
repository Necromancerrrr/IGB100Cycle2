using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);

    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);

    [System.Serializable]
    public class WeaponUpgrade
    {
        public int weaponUpgradeIndex;
        public GameObject initialWeapon;
        public WeaponScriptableObject weaponData;
    }

    [System.Serializable]
    public class PassiveItemUpgrade
    {
        public int passiveItemUpgradeIndex;
        public GameObject initialPassiveItem;
        public PassiveItemScriptableObject passiveItemData;
    }


    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public GameObject upgradeButton;
        public TMP_Text levelDisplay;
        public Image AreaSizeIcon;
        public Image ProjectileCountIcon;
        public Image DurationIcon;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();                // List of upgrade options for weapons
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // List of upgrade options for passive items

    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();                            // List of UI for upgrade options present in the scene


    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
    }

    public void AddWeapon(int slotIndex, WeaponController weapon) // Add a weapon to a specific slot
    {
        weaponSlots[slotIndex] = weapon;
        weaponLevels[slotIndex] = weapon.weaponData.Level;
        weaponUISlots[slotIndex].enabled = true;
        weaponUISlots[slotIndex].sprite = weapon.weaponData.Icon;

        LevelUpEnd();
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem) // Add a passive item to a specific slot
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;


        LevelUpEnd();
    }


    public void LevelUpWeapon(int slotIndex, int upgradeIndex)
    {
        if (weaponSlots.Count > slotIndex)
        {
            WeaponController weapon = weaponSlots[slotIndex];
            if (!weapon.weaponData.NextLevelPrefab)
            {
                Debug.LogError("NO NEXT LEVEL FOR" + weapon.name);
                return;
            }

            GameObject upgradedWeapon = Instantiate(weapon.weaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedWeapon.transform.SetParent(transform); // Set the weapon to be the child of the player
            AddWeapon(slotIndex, upgradedWeapon.GetComponent<WeaponController>());
            Destroy(weapon.gameObject);
            weaponLevels[slotIndex] = upgradedWeapon.GetComponent<WeaponController>().weaponData.Level;

            weaponUpgradeOptions[upgradeIndex].weaponData = upgradedWeapon.GetComponent<WeaponController>().weaponData;

            LevelUpEnd();
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (passiveItemSlots.Count > slotIndex)
        {
            PassiveItem passiveItem = passiveItemSlots[slotIndex];
            if (!passiveItem.passiveItemData.NextLevelPrefab)
            {
                Debug.LogError("NO NEXT LEVEL FOR" + passiveItem.name);
                return;
            }

            GameObject upgradedPassiveItem = Instantiate(passiveItem.passiveItemData.NextLevelPrefab, transform.position, Quaternion.identity);
            upgradedPassiveItem.transform.SetParent(transform); // Set the weapon to be the child of the player
            AddPassiveItem(slotIndex, upgradedPassiveItem.GetComponent<PassiveItem>());
            Destroy(passiveItem.gameObject);
            weaponLevels[slotIndex] = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData.Level;

            passiveItemUpgradeOptions[upgradeIndex].passiveItemData = upgradedPassiveItem.GetComponent<PassiveItem>().passiveItemData;

            LevelUpEnd();
        }
    }

    private void LevelUpEnd()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.EndLevelUp();
        }
    }


    /// <summary>
    /// WEAPON AND PASSIVE ITEM UPGRADE SYSTEM
    /// </summary>

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        // START OF BRIAN CODE

        int weaponListLength = 0;
        for (int i = 0; i < weaponSlots.Count; i++) // Checks to see if the current weaponSlots are full
        {
            if (weaponSlots[i] != null)
            {
                weaponListLength += 1; // list.Count wasn't working :^)    
            }
        }
        int indexNum = availableWeaponUpgrades.Count - 1; // It's important to go through the list backwards as it prevents any errors as we cull options
        for (int i = availableWeaponUpgrades.Count - 1; i > -1; i--) // Repeat for every option in availableWeaponUpgrades
        {
            bool removeFromList = true;
            GameObject currentCheck = availableWeaponUpgrades[i].initialWeapon; // Initially sets the weapon controller object
            while (removeFromList == true)
            {
                for (int j = 0; j < weaponListLength; j++)
                {
                    if (currentCheck.GetComponent<WeaponController>().weaponData == weaponSlots[j].weaponData && currentCheck.GetComponent<WeaponController>().weaponData.NextLevelPrefab != null)
                    {   // If the weaponUpgradeOption matches the equipped option and a level up is available
                        removeFromList = false; // Do not remove it from the potential upgrades list
                    }
                    if (currentCheck.GetComponent<WeaponController>() == weaponSlots[j] && currentCheck.GetComponent<WeaponController>().weaponData.NextLevelPrefab == null)
                    {   // If the weaponUpgradeOption matches the equipped option and a level up is not available
                        break; // Exit function
                    }
                }
                if (currentCheck.GetComponent<WeaponController>().weaponData.NextLevelPrefab == null) // If there aren't any further levels
                {
                    if (weaponListLength == 6)
                    {
                        break; // Remove additional options when the weapon slots are full
                    }
                    else
                    {
                        removeFromList = false; // Keep any options that don't find a match when the weapon list isn't full 
                    }
                }
                else
                {
                    currentCheck = currentCheck.GetComponent<WeaponController>().weaponData.NextLevelPrefab; // Increases the level that is checked
                }
            }
            if (removeFromList == true)
            {
                availableWeaponUpgrades.Remove(weaponUpgradeOptions[i]);
            }
        }

        // Second verse, just as good as the first

        int statListLength = 0;
        for (int i = 0; i < passiveItemSlots.Count; i++) // Checks to see if the current passiveItemSlots are full
        {
            if (passiveItemSlots[i] != null)
            {
                statListLength += 1; // list.Count wasn't working :^)    
            }
        }
        indexNum = availablePassiveItemUpgrades.Count - 1; // It's important to go through the list backwards as it prevents any errors as we cull options
        for (int i = availablePassiveItemUpgrades.Count - 1; i > -1; i--) // Repeat for every option in availablePassiveItemUpgrades
        {
            bool removeFromList = true;
            GameObject currentCheck = availablePassiveItemUpgrades[i].initialPassiveItem; // Initially sets the passive controller object
            while (removeFromList == true)
            {
                for (int j = 0; j < statListLength; j++)
                {
                    if (currentCheck.GetComponent<PassiveItem>().passiveItemData == passiveItemSlots[j].passiveItemData && currentCheck.GetComponent<PassiveItem>().passiveItemData.NextLevelPrefab != null)
                    {   // If the passiveItemUpgradeOption matches the equipped option and a level up is available
                        removeFromList = false; // Do not remove it from the potential upgrades list
                    }
                    if (currentCheck.GetComponent<PassiveItem>() == passiveItemSlots[j] && currentCheck.GetComponent<PassiveItem>().passiveItemData.NextLevelPrefab == null)
                    {   // If the passiveItemUpgradeOption matches the equipped option and a level up is not available
                        break; // Exit function
                    }
                }
                if (currentCheck.GetComponent<PassiveItem>().passiveItemData.NextLevelPrefab == null) // If there aren't any further levels
                {
                    if (statListLength == 6)
                    {
                        break; // Remove additional options when the passive slots are full
                    }
                    else
                    {
                        removeFromList = false; // Keep any options that don't find a match when the passive list isn't full 
                    }
                }
                else
                {
                    currentCheck = currentCheck.GetComponent<PassiveItem>().passiveItemData.NextLevelPrefab; // Increases the level that is checked
                }
            }
            if (removeFromList == true)
            {
                availablePassiveItemUpgrades.Remove(passiveItemUpgradeOptions[i]);
            }
        }

        // END OF BRIAN CODE

        foreach (var upgradeOption in upgradeUIOptions) // Repeat for each card
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0) // If there are no available upgrades, exit
            {
                return;
            }

            int upgradeType; // Randomly generates a choice between weapon and passive upgrade

            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                upgradeType = Random.Range(1, 3);
            }

            if (upgradeType == 1) // upgradeType = 1 = Weapons
            {
                WeaponUpgrade chosenWeaponUpgrade = availableWeaponUpgrades[Random.Range(0, availableWeaponUpgrades.Count)]; // Checks what a player can upgrade

                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                if (chosenWeaponUpgrade != null) // Safety
                {
                    EnableUpgradedUI(upgradeOption);

                    bool newWeapon = false; // Set a flag, assuming the weapon is not new

                    for (int i = 0; i < weaponSlots.Count; i++) // Check the inventory, slot by slot
                    {
                        // If the weapon slot is null, it'll assume that weapon is new.
                        // If the weapon slot isn't null, it checks if that slot's item matches the same as the chosen upgrade
                        if (weaponSlots[i] != null && weaponSlots[i].weaponData == chosenWeaponUpgrade.weaponData)
                        {
                            newWeapon = false; // Confirming that newWeapon is false

                            if (!newWeapon)
                            {
                                // Stops the code if at item is at max level
                                if (!chosenWeaponUpgrade.weaponData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);

                                    break;
                                }

                                upgradeOption.upgradeButton.GetComponent<Button>().onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex)); // Apply button functionality

                                // Set the name and description of the weapon
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;
                                upgradeOption.levelDisplay.text = "Lvl. " + chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Level;
                            }
                            break;
                        }
                        else // If the upgrade option isn't found in the inventory, then it is a new weapon
                        {
                            newWeapon = true;
                        }
                    }

                    if (newWeapon) // Spawn a new weapon
                    {
                        upgradeOption.upgradeButton.GetComponent<Button>().onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon)); // Apply button functionality

                        // Set the name and description of the weapon
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                        upgradeOption.levelDisplay.text = "Lvl. " + chosenWeaponUpgrade.weaponData.Level;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
                    upgradeOption.AreaSizeIcon.enabled = chosenWeaponUpgrade.weaponData.AreaSizeEnabled;
                    upgradeOption.ProjectileCountIcon.enabled = chosenWeaponUpgrade.weaponData.ProjectileCountEnabled;
                    upgradeOption.DurationIcon.enabled = chosenWeaponUpgrade.weaponData.DurationEnabled;
                    upgradeOption.upgradeButton.GetComponent<CardBlink2>().Set();
                }
            }

            else if (upgradeType == 2) // UpgradeType = 2 = Passive Items
            {
                PassiveItemUpgrade chosenPassiveItemUpgrade = availablePassiveItemUpgrades[Random.Range(0, availablePassiveItemUpgrades.Count)]; // Checks what a player can upgrade

                availablePassiveItemUpgrades.Remove(chosenPassiveItemUpgrade);

                if (chosenPassiveItemUpgrade != null) // Safety
                {

                    EnableUpgradedUI(upgradeOption);

                    bool newPassiveItem = false; // Set a flag, assuming the passive item is not new

                    for (int i = 0; i < passiveItemSlots.Count; i++) // Check the inventory, slot by slot
                    {
                        // If the passive item slot is null, it'll assume that weapon is new.
                        // If the passive item isn't null, it checks if that slot's item matches the same as the chosen upgrade
                        if (passiveItemSlots[i] != null && passiveItemSlots[i].passiveItemData == chosenPassiveItemUpgrade.passiveItemData)
                        {
                            newPassiveItem = false; // Confirming that newPassiveItem is false

                            if (!newPassiveItem)
                            {
                                // Stops the code if item is at max level
                                if (!chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(upgradeOption);
                                    break;
                                }

                                upgradeOption.upgradeButton.GetComponent<Button>().onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex)); // Apply button functionality

                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
                                upgradeOption.levelDisplay.text = "Lvl. " + chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Level;
                            }
                            break;
                        }
                        else // If the upgrade option isn't found in the inventory, then it is a new passive item
                        {
                            newPassiveItem = true;
                        }
                    }

                    if (newPassiveItem) // Spawn a new passive item
                    {
                        upgradeOption.upgradeButton.GetComponent<Button>().onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem)); // Apply button functionality

                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                        upgradeOption.levelDisplay.text = "Lvl. " + chosenPassiveItemUpgrade.passiveItemData.Level;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                    upgradeOption.AreaSizeIcon.enabled = false;
                    upgradeOption.ProjectileCountIcon.enabled = false;
                    upgradeOption.DurationIcon.enabled = false;
                    upgradeOption.upgradeButton.GetComponent<CardBlink2>().Set();
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.GetComponent<Button>().onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    void DisableUpgradeUI(UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradedUI(UpgradeUI upgradeUI)
    {
        upgradeUI.upgradeNameDisplay.transform.parent.gameObject.SetActive(true);
    }
}

using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public List<WeaponController> weaponSlots = new List<WeaponController>(6);
    public int[] weaponLevels = new int[6];
    public List<Image> weaponUISlots = new List<Image>(6);

    public List<PassiveItem> passiveItemSlots = new List<PassiveItem>(6);
    public int[] passiveItemLevels = new int[6];
    public List<Image> passiveItemUISlots = new List<Image>(6);

    public List<PactItem> pactItemSlots = new List<PactItem>(2);
    public List<Image> pactItemUISlots = new List<Image>(2);

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
    public class PactItemUpgrade
    {
        public int pactItemUpgradeIndex;
        public GameObject initialPactItem;
        public PactItemScriptableObject pactItemData;
    }

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeNameDisplay;
        public TMP_Text upgradeDescriptionDisplay;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    public List<WeaponUpgrade> weaponUpgradeOptions = new List<WeaponUpgrade>();                // List of upgrade options for weapons
    public List<PassiveItemUpgrade> passiveItemUpgradeOptions = new List<PassiveItemUpgrade>(); // List of upgrade options for passive items
    public List<PactItemUpgrade> pactItemOptions = new List<PactItemUpgrade>();

    public List<UpgradeUI> upgradeUIOptions = new List<UpgradeUI>();                            // List of UI for upgrade options present in the scene
    public List<UpgradeUI> pactChoiceUIOptions = new List<UpgradeUI>();


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

        if (GameManager.instance != null)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPassiveItem(int slotIndex, PassiveItem passiveItem) // Add a passive item to a specific slot
    {
        passiveItemSlots[slotIndex] = passiveItem;
        passiveItemLevels[slotIndex] = passiveItem.passiveItemData.Level;
        passiveItemUISlots[slotIndex].enabled = true;
        passiveItemUISlots[slotIndex].sprite = passiveItem.passiveItemData.Icon;


        if (GameManager.instance != null)
        {
            GameManager.instance.EndLevelUp();
        }
    }

    public void AddPactItem(int slotIndex, PactItem pactItem)
    {
        pactItemSlots[slotIndex] = pactItem;
        pactItemUISlots[slotIndex].enabled = true;
        pactItemUISlots[slotIndex].sprite = pactItem.pactItemData.Icon;

        if (GameManager.instance != null)
        {
            GameManager.instance.EndPactChoice();
        }
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

            if (GameManager.instance != null)
            {
                GameManager.instance.EndLevelUp();
            }
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

            if (GameManager.instance != null)
            {
                GameManager.instance.EndLevelUp();
            }
        }
    }

    /// <summary>
    /// WEAPON AND PASSIVE ITEM UPGRADE SYSTEM
    /// </summary>

    void ApplyUpgradeOptions()
    {
        List<WeaponUpgrade> availableWeaponUpgrades = new List<WeaponUpgrade>(weaponUpgradeOptions);
        List<PassiveItemUpgrade> availablePassiveItemUpgrades = new List<PassiveItemUpgrade>(passiveItemUpgradeOptions);

        foreach (var upgradeOption in upgradeUIOptions)
        {
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
            {
                return;
            }

            int upgradeType;

            if (availableWeaponUpgrades.Count == 0 || weaponSlots[5] != null)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 0 || passiveItemSlots[5] != null)
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

                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chosenWeaponUpgrade.weaponUpgradeIndex)); // Apply button functionality

                                // Set the name and description of the weapon
                                upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.NextLevelPrefab.GetComponent<WeaponController>().weaponData.Name;

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
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnWeapon(chosenWeaponUpgrade.initialWeapon)); // Apply button functionality

                        // Set the name and description of the weapon
                        upgradeOption.upgradeDescriptionDisplay.text = chosenWeaponUpgrade.weaponData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenWeaponUpgrade.weaponData.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.weaponData.Icon;
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

                                upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, chosenPassiveItemUpgrade.passiveItemUpgradeIndex)); // Apply button functionality

                                upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Description;
                                upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.NextLevelPrefab.GetComponent<PassiveItem>().passiveItemData.Name;
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
                        upgradeOption.upgradeButton.onClick.AddListener(() => player.SpawnPassiveItem(chosenPassiveItemUpgrade.initialPassiveItem)); // Apply button functionality

                        upgradeOption.upgradeDescriptionDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Description;
                        upgradeOption.upgradeNameDisplay.text = chosenPassiveItemUpgrade.passiveItemData.Name;
                    }

                    upgradeOption.upgradeIcon.sprite = chosenPassiveItemUpgrade.passiveItemData.Icon;
                }
            }
        }
    }

    void RemoveUpgradeOptions()
    {
        foreach (var upgradeOption in upgradeUIOptions)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);
        }
    }

    public void RemoveAndApplyUpgrades()
    {
        RemoveUpgradeOptions();
        ApplyUpgradeOptions();
    }

    /// <summary>
    /// PACTS CHOICE SYSTEM
    /// </summary>
    /// 
    void ApplyPactOptions()
    {
        List<PactItemUpgrade> availablePactOptions = new List<PactItemUpgrade>(pactItemOptions);

        foreach (var pactOption in pactChoiceUIOptions)
        {
            PactItemUpgrade chosenPactItem = availablePactOptions[Random.Range(0, availablePactOptions.Count)]; // Checks what a player can upgrade

            availablePactOptions.Remove(chosenPactItem);

            EnableUpgradedUI(pactOption);

            bool newPactItem = true; // Set a flag, assuming the passive item is not new

            for (int i = 0; i < pactItemSlots.Count; i++) // Check the inventory, slot by slot
            {
                // If the passive item slot is null, it'll assume that weapon is new.
                // If the passive item isn't null, it checks if that slot's item matches the same as the chosen upgrade
                if (pactItemSlots[i] != null && pactItemSlots[i].pactItemData == chosenPactItem.pactItemData)
                {
                    newPactItem = false; // Confirming that newPassiveItem is false

                    if (!newPactItem)
                    {
                        DisableUpgradeUI(pactOption);
                        continue;
                    }
                }
                else
                {
                    newPactItem = true;
                }
            }
            if (newPactItem)
            {
                pactOption.upgradeButton.onClick.AddListener(() => player.SpawnPactItem(chosenPactItem.initialPactItem)); // Apply button functionality

                // Set the name and description of the weapon
                pactOption.upgradeDescriptionDisplay.text = chosenPactItem.pactItemData.Description;
                pactOption.upgradeNameDisplay.text = chosenPactItem.pactItemData.Name;
            }
            pactOption.upgradeIcon.sprite = chosenPactItem.pactItemData.Icon;
        }
    }



    void RemovePactOptions()
    {
        foreach(var pactOption in pactChoiceUIOptions)
        {
            pactOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(pactOption);
        }
    }

    public void RemoveAndApplyPacts()
    {
        RemovePactOptions();
        ApplyPactOptions();
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

private WeaponAnimationController weaponController;
private PlayerMovement2 playerMovement;

private void Awake()
{
    weaponController = handHold.GetComponent<WeaponAnimationController>();
    playerMovement = transform.root.GetComponent<PlayerMovement2>();
}

void Update()
{
    weaponController.HandleCooldown();
    startEquip = Mathf.FloorToInt(selectedSlotIndex);

    if (!inventoryStorage.activeSelf && !weaponController.isOnCooldown && (Input.GetAxis("Mouse ScrollWheel") != 0 || Int16.TryParse(Input.inputString, out int input)))
    {
        selectedSlotIndex -= Input.GetAxis("Mouse ScrollWheel") * selectionScrollSpeed;
        selectedSlotIndex = selectedSlotIndex >= 10f ? 0f : selectedSlotIndex < 0 ? 9.9999f : selectedSlotIndex;

        if (int.TryParse(Input.inputString, out int num) && num >= 0 && num <= 9)
        {
            selectedSlotIndex = num == 0 ? 9 : num - 1;
        }

        selectedSlotUI.transform.SetParent(selectSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)), false);

        if (startEquip != Mathf.FloorToInt(selectedSlotIndex))
        {
            StartCoroutine(weaponController.HandleSwap());
            transform.root.GetComponent<PhotonView>().RPC("PlayEquipAnimation", RpcTarget.All);
        }
    }

    if (itemHold)
    {
        isHoldingSomething = true;
        if (photonView.IsMine && !weaponController.isSwinging && !weaponController.isOnCooldown && Input.GetKeyDown(GetComponent<KeyCode>("Fire1")))
        {
            weaponController.SwingWeapon();
        }
        else if (photonView.IsMine && hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).childCount != 0 && hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).name == "FoodItem" && Input.GetKeyDown(GetComponent<KeyCode>("Use")))
        {
            weaponController.EatItem();
            RemoveFromItemList(hotBarSlotsUI.transform.GetChild(Mathf.FloorToInt(selectedSlotIndex)).GetChild(0).gameObject.name, 1);
        }
        else
        {
            weaponController.HandleSway();
            weaponController.HandleBob();
        }
    }
    else
    {
        isHoldingSomething = false;
        if (photonView.IsMine && !weaponController.isSwinging && !weaponController.isOnCooldown && Input.GetKeyDown(GetComponent<KeyCode>("Fire1")))
        {
            playerMovement.animator.SetTrigger("Punch");
        }
    }
}

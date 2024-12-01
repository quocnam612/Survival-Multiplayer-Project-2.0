using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssests.Character.FirstPerson;

public class KeybindDisplayer : MonoBehaviour
{
    public PlayerMovement2 PlayerMovement;
    Text keybindDisplayer;

    private void Start() {
        keybindDisplayer = GetComponent<Text>();

        keybindDisplayer.text += "Sprint - " + PlayerMovement.sprintKey;
        keybindDisplayer.text += "\nCrouch - " + PlayerMovement.crouchKey;
        keybindDisplayer.text += "\nZoom - " + PlayerMovement.zoomKey;
        keybindDisplayer.text += "\nAttack - " + PlayerMovement.attackKey;
        keybindDisplayer.text += "\nPick Up - " + PlayerMovement.pickUpKey;
        keybindDisplayer.text += "\nInteract - " + PlayerMovement.interactKey;
    }
}

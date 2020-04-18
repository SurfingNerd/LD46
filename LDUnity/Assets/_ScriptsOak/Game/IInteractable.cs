using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interact();
    Sprite GetInteractIcon();

    Vector3 GetPosition();

}

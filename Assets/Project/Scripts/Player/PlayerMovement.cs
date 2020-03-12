using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBGame
{
    public class PlayerMovement : MonoBehaviour
    {
        public Vector2 Move(Controls controls, float moveSpeed)
        {
            var movementInput = controls.Player.Movement.ReadValue<Vector2>();
            movementInput.Normalize();
            transform.Translate(Time.deltaTime * moveSpeed * movementInput);
            return movementInput;
        }
    }
}
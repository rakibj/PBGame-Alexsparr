using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI.Animation;
using UnityEngine;

namespace PBGame
{
    public class PlayerController : MonoBehaviour
    {
        private Controls m_controls;
        private PlayerMovement m_playerMovement;
        private PlayerAnimation m_playerAnimation;
        [SerializeField] private float moveSpeed;

        private void Awake()
        {
            m_controls = new Controls();
            m_playerMovement = GetComponent<PlayerMovement>();
            m_playerAnimation = GetComponent<PlayerAnimation>();
        }

        private void OnEnable()
        {
            m_controls.Player.Enable();
        }

        private void OnDisable()
        {
            m_controls.Player.Disable();
        }

        private void FixedUpdate()
        {
            var movement = m_playerMovement.Move(m_controls, moveSpeed);
            m_playerAnimation.Animate(movement);
        }

        
        
    }
}
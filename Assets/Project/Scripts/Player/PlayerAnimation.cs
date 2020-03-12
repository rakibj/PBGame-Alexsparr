using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PBGame
{
    public class PlayerAnimation : MonoBehaviour
    {
        private Animator m_animator;
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");

        private void Awake()
        {
            m_animator = GetComponent<Animator>();
        }

        public void Animate(Vector2 movement)
        {
            m_animator.SetFloat(Horizontal, movement.x);
            m_animator.SetFloat(Vertical, movement.y);
        }
    }
}
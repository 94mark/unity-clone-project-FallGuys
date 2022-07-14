using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevionGames
{
    public class PlayerInfo 
    {
        private string m_Tag = "Player";

        public PlayerInfo(string tag) {
            this.m_Tag = tag;
        }


        private GameObject m_GameObject;
        public GameObject gameObject {
            get {
				if (this.m_GameObject == null)
				{
					GameObject[] players = GameObject.FindGameObjectsWithTag(this.m_Tag);
					for (int i = 0; i < players.Length; i++)
					{
						GameObject player = players[i];

						this.m_GameObject = player;
					}
				}
				return this.m_GameObject;
			}
        }

        private Transform m_Transform;
        public Transform transform
        {
            get
            {
                if (gameObject != null)
                {
                    return this.gameObject.transform;
                }
                return null;
            }
        }

        private Collider m_Collider;
        public Collider collider
        {
            get
            {
                if (this.m_Collider == null && this.gameObject != null)
                {
                    this.m_Collider = this.gameObject.GetComponent<Collider>();
                }
                return this.m_Collider;
            }
        }

        private Collider2D m_Collider2D;
        public Collider2D collider2D
        {
            get
            {
                if (this.m_Collider2D == null && this.gameObject != null)
                {
                    this.m_Collider2D = this.gameObject.GetComponent<Collider2D>();
                }
                return this.m_Collider2D;
            }
        }

        private Animator m_Animator;
        public Animator animator
        {
            get
            {
                if (this.m_Animator == null && this.gameObject != null)
                {
                    this.m_Animator = this.gameObject.GetComponentInChildren<Animator>();
                }
                return this.m_Animator;
            }
        }

        public Bounds bounds
        {
            get
            {
                return UnityTools.GetBounds(gameObject);
            }
        }

      
    }
}
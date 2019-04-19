using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{

        [SerializeField]
        private GameObject punch;

        [SerializeField]
        private GameObject kick;

        [SerializeField]
        private GameObject tackle;

        private GameObject currentAttack;

        void Awake()
        {
            this.punch = Instantiate(this.punch, this.transform) as GameObject;
            this.kick = Instantiate(this.kick, this.transform) as GameObject;
            this.tackle = Instantiate(this.kick, this.transform) as GameObject;

            this.punch.GetComponent<AttackTarget>().owner = this.gameObject;
            this.kick.GetComponent<AttackTarget>().owner = this.gameObject;
            this.tackle.GetComponent<AttackTarget>().owner = this.gameObject;

            this.currentAttack = this.punch;
        }

        public void SelectAttack(int type)
        {
            if(type == 0)
            {
                this.currentAttack = this.punch;
            } else if (type == 1)
            {
                this.currentAttack = this.kick;
            } else
            {
                this.currentAttack = this.tackle;
            }
        }

       public void Act(GameObject target)
        {
            this.currentAttack.GetComponent<AttackTarget>().hit(target);
        }
}

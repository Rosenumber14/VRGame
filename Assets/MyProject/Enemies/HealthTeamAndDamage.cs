using UnityEngine;

namespace Assets.MyProject.Enemies
{
    //apply this class to anything you wish to have health
    public class HealthTeamAndDamage : MonoBehaviour
    {
        public enum Teams
        {
            Player,
            Enemy
        }
        public static bool FriendlyFire = false;
        public float CurrentHealth = 100;
        public Teams Team = Teams.Enemy;
        public int Damage = 0;

        public HealthTeamAndDamage enemyCollider;
        
        void Update()
        {
            if(enemyCollider != null)
                DoDamage(enemyCollider);

            UpdateFn();
        }

        //unity function
        public void OnTriggerEnter(Collider collider)
        {
            var enemy = collider.GetComponent<HealthTeamAndDamage>(); //do damage to any object that takes health
            if(enemy)
            {
                enemyCollider = enemy;
            }
        }
        public void OnTriggerExit(Collider collider)
        {
            enemyCollider = null;
        }

        public virtual void UpdateFn()
        {

        }

        public virtual bool TakeDamage(float damage, Teams team)
        {
            if (damage == 0)
                return false;
            if (FriendlyFire || Team != team)
            {
                CurrentHealth -= damage * Time.deltaTime;
                return true;
            }
            return false;
        }

        //return is damage was sucessfully done
        public virtual bool DoDamage(HealthTeamAndDamage enemy)
        {
            return enemy.TakeDamage(Damage, Team);
        }
    }
}

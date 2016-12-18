using UnityEngine;
using Assets.MyProject.Enemies;

public class Enemy : HealthTeamAndDamage {
    private enum Animations
    {
        run,
        die,
        gethit,
        attack
    }
    public float Speed = 0.01f;
    public Transform TargetDestination;
    private Animations currentAnimation = Animations.run;
    private Animation animationComponent;

    // Use this for initialization
    void Start () {
        animationComponent = GetComponent<Animation>();
    }

    public override void UpdateFn()
    {
        if (currentAnimation == Animations.run)
        {
            transform.position = Vector3.MoveTowards(transform.position, TargetDestination.position, Speed);
        }
        else if (currentAnimation == Animations.gethit)
        {
            if (!animationComponent.isPlaying)
            {
                SetAnimation(Animations.run);
            }
        }
        else if (currentAnimation == Animations.die)
        {
            if (!animationComponent.isPlaying)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void SetAnimation(Animations animationClip)
    {
        currentAnimation = animationClip;
        animationComponent.Play(animationClip.ToString());
    }

    public override bool TakeDamage(float damage, Teams team)
    {
        if (!base.TakeDamage(damage, team)) //if no damage was taken then dont do anything
        {
            return false;
        }

        if (CurrentHealth <= 0)
        {
            Destroy(GetComponent<BoxCollider>());
            Destroy(GetComponent<Rigidbody>());
            SetAnimation(Animations.die);
        }
        else
        {
            SetAnimation(Animations.gethit);
        }
        return true;
    }

    public override bool DoDamage(HealthTeamAndDamage collider)
    {
        if (base.DoDamage(collider))
        {
            SetAnimation(Animations.attack);
            return true;
        }
        
        return false;
    }
}

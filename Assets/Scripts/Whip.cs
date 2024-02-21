using UnityEngine;

public class Whip : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        //if collide with enemy, deal damage
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.name.Contains("Zombie") == true){
                if (collision.gameObject.name.Contains("Boss") == true)
                    collision.gameObject.GetComponent<ZombieBoss>().health -= 1.5f;
                else
                    collision.gameObject.GetComponent<ZombieAI>().health -= 1.5f;
            } else if (collision.gameObject.name.Contains("Skeleton") == true){
                if (collision.gameObject.name.Contains("Boss") == true)
                    collision.gameObject.GetComponent<SkeletonBoss>().health -= 1.5f;
                else
                    collision.gameObject.GetComponent<SkeletonAI>().health -= 1.5f;
            } else if (collision.gameObject.name.Contains("Mummy") == true){
                if (collision.gameObject.name.Contains("Boss") == true)
                    collision.gameObject.GetComponent<MummyBoss>().health -= 1.5f;
                else
                    collision.gameObject.GetComponent<MummyAI>().health -= 1.5f;
            }
        }
    }
}

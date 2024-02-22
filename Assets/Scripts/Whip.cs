using UnityEngine;

public enum WhipDirection
{
    Up,
    Down,
    Left,
    Right    
}

public class Whip : MonoBehaviour
{
    public WhipDirection whipDirection;
    public Transform Player;

    private void Update()
    {
        if (whipDirection == WhipDirection.Up)
        {
            transform.position = new Vector3(Player.position.x, Player.position.y + 1.1f, 0);
        }
        else if (whipDirection == WhipDirection.Down)
        {
            transform.position = new Vector3(Player.position.x, Player.position.y - 1.1f, 0);
        }
        else if (whipDirection == WhipDirection.Left)
        {
            transform.position = new Vector3(Player.position.x - 1.2f, Player.position.y - 0.4f, 0);
        }
        else if (whipDirection == WhipDirection.Right)
        {
            transform.position = new Vector3(Player.position.x + 1.2f, Player.position.y - 0.4f, 0);
        }
    }

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

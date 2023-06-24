using UnityEngine;

public class EnemyPathrollingController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool directionRight = true;
    [SerializeField] private GameObject leftCheck, rightCheck;
    public Vector2 checkSize;

    public LayerMask[] platform;

    private void Update()
    {
        float move = moveSpeed * Time.deltaTime;
        if (Physics2D.OverlapBox(leftCheck.transform.position, checkSize, platform.Length))
        {
            directionRight = !directionRight;
        }
        if (Physics2D.OverlapBox(rightCheck.transform.position, checkSize, platform.Length))
        {
            directionRight = !directionRight;
        }

        if (directionRight)
        {
            transform.Translate(Vector2.right * move);
        }
        else
        {
            transform.Translate(Vector2.left * move);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(leftCheck.transform.position, checkSize);   
        Gizmos.DrawWireCube(rightCheck.transform.position , checkSize);   
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            
        }
    }

}

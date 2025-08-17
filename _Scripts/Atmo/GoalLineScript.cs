using UnityEngine;

public class GoalLineScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        ICrossGoal hit = collision.GetComponent<ICrossGoal>();
        if (hit != null)
        {
            hit.CrossedFinishLine();
        }
    }
}

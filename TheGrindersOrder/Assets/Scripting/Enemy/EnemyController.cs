using UnityEngine;

public class EnemyController : MonoBehaviour

{
    public string enemyID;

    private EnemyData stats;
    private Transform target;

    void Start()
    {
        if (EnemyManager.enemyDatabase.TryGetValue(enemyID, out stats))
        {
            InitializeEnemy(stats);
        }

        // find target in scene (temporary player)
        target = GameObject.Find("Hi Im a Dummy Player").transform;  //target = player.transform;(copy paste on this line after put real player)
    }

    void Update()
    {
        if (target == null) return;

        //  simple movement toward target
        float speed = float.Parse(stats.moveSpeedValue); // from your CSV

        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime
        );
    }

    void InitializeEnemy(EnemyData data)
    {
        gameObject.name = data.displayName;
        Debug.Log($"Enemy {data.displayName} initialized with {data.health} HP.");
    }
}

using UnityEngine;

public class EnemyWorldState : MonoBehaviour
{
    public bool seenPlayer = false;
    public float timer;
    public float distanceToPlayer;

    public float DistanceToPlayer
    {
        get => distanceToPlayer;
        private set => distanceToPlayer = value;
    }

    private void Update()
    {
        if (seenPlayer)
        {
            timer += Time.deltaTime;

            if (timer >= 10f)
            {
                seenPlayer = false;
                timer = 0;
            }
        }
    }

    public void ResetTimer()
    {
        timer = 0;
        seenPlayer = true;
    }
}

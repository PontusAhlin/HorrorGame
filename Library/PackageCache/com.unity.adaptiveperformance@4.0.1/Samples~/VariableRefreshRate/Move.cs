using UnityEngine;

public class Move : MonoBehaviour
{
    Vector3 pos;
    public float speed = 10.0f;
    float min = -15f;
    float max = 15f;
    private float waitTime = 0;
    private float timer = 0;
    private float addedSpeed = 0;
    void Start()
    {
        pos = transform.position;
        waitTime = 1 / speed;
    }

    void Update()
    {
        var dt = Time.deltaTime;
        timer -= dt;
        if (timer < 0)
        {
            timer = waitTime;

            var timeForMove = Time.time;
            if (waitTime < dt)
            {
                timeForMove += (dt - waitTime);
            }

            addedSpeed = Mathf.Abs(Mathf.Sin(timeForMove * 1.5f) * (max - min));
            var z = (Mathf.PingPong((addedSpeed), max - min) + min);
            transform.position = new Vector3(pos.x, pos.y, z);
        }
    }
}

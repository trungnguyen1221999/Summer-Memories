void Update()
{
    if (isDead) return;

    if (transform.position.x < approachX)
    {
        // Di chuyển nhanh tới X = approachX
        transform.position += Vector3.right * speed * Time.deltaTime;
    }
    else
    {
        // Kiểm tra khoảng cách đến finalTarget
        float distanceToTarget = Vector3.Distance(transform.position, finalTarget);

        if (distanceToTarget <= 0.7f && distanceToTarget >= 0.5f)
        {
            // Nếu gần finalTarget, đi thẳng tới nó
            Vector3 direction = (finalTarget - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;
        }
        else
        {
            // Di chuyển theo pattern Y
            Vector3 move = Vector3.zero;

            switch (enemyType)
            {
                case EnemyType.Enemy1:
                    move += Vector3.up * yDirection * ySpeed * Time.deltaTime;
                    break;

                case EnemyType.Enemy2:
                    float zigzag = Mathf.Sin((Time.time - startTime) * zigZagFrequency) * zigZagAmplitude;
                    move += Vector3.up * (zigzag * Time.deltaTime + yDirection * ySpeed * Time.deltaTime);
                    break;

                case EnemyType.Enemy3:
                    float sinY = Mathf.Sin((Time.time - startTime) * sinFrequency) * sinAmplitude;
                    move += Vector3.up * (sinY * Time.deltaTime + yDirection * ySpeed * Time.deltaTime);
                    break;

                case EnemyType.Boss:
                    move += Vector3.down * speed * Time.deltaTime;
                    break;
            }

            // Luôn tiến về X finalTarget
            float stepX = speed * Time.deltaTime;
            move += new Vector3(stepX, 0f, 0f);

            transform.position += move;

            CheckYBounds();
        }
    }
}

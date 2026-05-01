using DG.Tweening;
using UnityEngine;

public class EnemyMoveToPlayer : MonoBehaviour
{
    float timer = 0f;

    public bool loopMovement = true;

    public MoveDirection[] moveDirections;

    bool movingForward = true;

    int moveIndex = 0;

    // Update is called once per frame
    void Update()
    {
        timer += MusicRhythmTimer.MusicDelta();

        if (timer >= MusicRhythmTimer.SecondsBetweenBeats())
        {
            Move();

            timer -= (float)MusicRhythmTimer.SecondsBetweenBeats();
        }
    }

    void Move()
    {
        if (moveIndex >= moveDirections.Length)
        {
            movingForward = false;
            moveIndex -= 1;
            ResetAllMovements();
        }

        if (moveIndex < 0)
        {
            movingForward = true;
            moveIndex = 0;
            ResetAllMovements();
        }
        
        Vector3 direction = movingForward ? StringToDirection(moveDirections[moveIndex].direction) : StringToDirectionInverted(moveDirections[moveIndex].direction);
        transform.DOMove(transform.position + direction, 0.2f).SetEase(Ease.InOutQuad);

        moveDirections[moveIndex].c_moveCount++;

        if (moveDirections[moveIndex].c_moveCount >= moveDirections[moveIndex].numMoves) moveIndex += movingForward ? 1 : -1;
    }

    Vector3 StringToDirection(string directionString)
    {
        switch(directionString.Trim().ToLower())
        {
            case "east": 
            case "right":
            return Vector3.right;

            case "west": 
            case "left":
            return Vector3.left;

            case "north":
            case "up":
            case "forward":
            return Vector3.forward;

            case "south":
            case "down":
            case "back":
            case "backward":
            return Vector3.back;
        }

        return Vector3.zero;
    }

    Vector3 StringToDirectionInverted(string directionString)
    {
        switch(directionString.Trim().ToLower())
        {
            case "east": 
            case "right":
            return Vector3.left;

            case "west": 
            case "left":
            return Vector3.right;

            case "north":
            case "up":
            case "forward":
            return Vector3.back;

            case "south":
            case "down":
            case "back":
            case "backward":
            return Vector3.forward;
        }

        return Vector3.zero;
    }

    void ResetAllMovements()
    {
        for (int i = 0; i < moveDirections.Length; ++i)
        {
            moveDirections[i].c_moveCount = 0;
        }
    }
}

[System.Serializable]
public class MoveDirection
{
    public string direction;
    public int numMoves;

    [HideInInspector] public int c_moveCount;
}
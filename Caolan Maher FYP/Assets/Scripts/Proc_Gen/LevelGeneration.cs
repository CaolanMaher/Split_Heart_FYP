using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
    // array of where start of level can be
    public Transform[] startingPositions;

    // array of rooms that can be placed
    public GameObject[] rooms; // index 0 -> LR, index 1 -> LRB, index 2 -> LRT, index 3 -> LRBT

    // pick which direction to go next
    private int direction;

    // amount to move on x axis to spawn new room
    public float moveAmountX;

    // amount to move on Y axis to spawn new room
    public float moveAmountY;

    // floats of the bounds of the level generation
    public float minX;
    public float maxX;
    public float minY;

    // bool to see if we should keep generating the level
    private bool generationIsStopped = false;

    // layermask for overlapsphere function
    public LayerMask roomMask;

    private void Start()
    {
        // get a random starting point, set this objects position to it, and spawn our first room
        int randStartingPos = Random.Range(0, startingPositions.Length);
        transform.position = startingPositions[randStartingPos].position;
        Instantiate(rooms[0], transform.position, Quaternion.identity);

        // set direction to random number between 1 and 5
        direction = Random.Range(1, 6);

        // spawn new room
        Move();
    }

    // move to spawn a new room
    private void Move()
    {
        // there is less chance of moving down
        // keep this for now as it makes it more likely the player has to go through more of level
        if(direction == 1 || direction == 2)
        {
            //print(transform.position.x + " : " + maxX);
            if(transform.position.x < maxX)
            {
                // we are within the maxX so we can move right
                Vector2 newPos = new Vector2(transform.position.x + moveAmountX, transform.position.y);
                transform.position = newPos;

                // all rooms have openings on right, so we pick on at random
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                // set direction to random number between 1 and 6
                direction = Random.Range(1, 6);

                // we check if this will mean it will try and go left on the next run
                if(direction == 3)
                {
                    // if so, we change it so that it will go right instead
                    // this is so that if it goes right, it can't go left and overwrite itself
                    direction = 2;
                }
                else if(direction == 4)
                {
                    // or we can change it to go down to keep the randomness
                    direction = 5;
                }
            }
            else
            {
                //print("Setting Direction to down from right");

                // we reached the maxX so we must move down
                direction = 5;
            }

            //print("Current Direction: " + direction);
        }
        else if(direction == 3 || direction == 4)
        {
            //print(transform.position.x + " : " + minX);
            if (transform.position.x > minX)
            {
                // we are within the minX so we can move left
                Vector2 newPos = new Vector2(transform.position.x - moveAmountX, transform.position.y);
                transform.position = newPos;

                // all rooms have openings on left, so we pick on at random
                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                // set direction to random number between 3 and 4
                // this is so that if it goes left, it can't go right and overwrite itself
                direction = Random.Range(3, 5);
            }
            else
            {
                //print("Setting Direction to down from Left");

                // we reached the minX so we must move down
                direction = 5;
            }

            //print("Current Direction: " + direction);
        }
        else if(direction == 5)
        {
            //print(transform.position.y + " : " + minY);
            if (transform.position.y > minY)
            {
                // Get the room we just created before this
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, roomMask);

                // check if the room found has a bottom opening
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {
                    // if not, destroy the room
                    roomDetection.GetComponent<RoomType>().RoomDestruction();

                    // we want to create a room that has a bottom opening
                    // we want an index of 1 or 3
                    int randomBottomRoom = Random.Range(1, 4);
                    // if we get 2
                    if(randomBottomRoom == 2)
                    {
                        // make it into 1
                        randomBottomRoom = 1;
                    }
                    Instantiate(rooms[randomBottomRoom], transform.position, Quaternion.identity);
                }

                // We are within the minY so we can move down
                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmountY);
                transform.position = newPos;

                // rooms with index 2 and 3 have top openings
                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                // set direction to random number between 1 and 6
                // we don't care what direction it moves in next
                direction = Random.Range(1, 6);
            }
            else
            {
                //print("Stop Generating Rooms");

                // We can stop the level gen here, or we can see if we can go left or right more
                // for now let's just stop level gen
                generationIsStopped = true;
            }
        }

        //print("Planning to make room at: " + transform.position.x + " " + transform.position.y);

        // check if we are still generating the level
        if (!generationIsStopped)
        {
            // if we can...

            // spawn new room
            Move();
        }
    }
}
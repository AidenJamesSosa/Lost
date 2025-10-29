# Lost

__Game Project that is like Expedition 33__

* Engine Configuration

|Engine | Version|
|-------|--------|
|Unity|6000.0.34f1|
----
## Structure

The project is composed of the following major classes:

#### SPlayer

The ```splayer``` class has the following responsibilities:

* Player inputs
* Player specific traits
* Reads Chest compontents next to the player

#### SStats
* Holds enemy and player stats
* Holds the abilty to shoot
* Uses the ```SMasterBulletHolder``` class to read what projectile to shoot
    - This class holds all of the bullet game objects
* Uses the ```SShoot``` class to determine projectile speed.
    - That class determines the projectile velocity and has an on trigger enter for hitting enemies

#### SRoom
* Reads from an array to see it's own ```SDoor``` class
* The doors open on a room clear
#### SDoor
* When The player collides with an open door the camera instantly moves
* The previous room is destroyed and a new one is created.
* An array and Ranis used to simulate randomness in room generation

 ```C#
  int randomIndex = Random.Range(0, mRooms.Length);
        GameObject mRandomRoom = mRooms[randomIndex];
  ```
#### Movement Controller

The MovementConroller class governs the movement of the character, it uses velocity and the ```CharacterController``` class to govern the move movement of the character. It handles:

* Movement
* Jump and Gravity
* Update Animation parameters
* Convert movement input to world directions
```C#
 Vector3 PlayerInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = Camera.main.transform.right;
        Vector3 fwdDir = Vector3.Cross(rightDir, Vector3.up);

        return rightDir * inputVal.x + fwdDir * inputVal.y;
    }
```

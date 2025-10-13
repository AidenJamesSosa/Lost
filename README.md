# Lost

__Game Project that is like Expedition 33__

* Engine Configuration

|Engine | Version|
|-------|--------|
|Unity|6000.1.2f1|
----
## Structure

The project is composed of the following major classes:

#### Player

The ```player``` class has the following responsibilities:

* Input Handling
* Spawning the view Camera
* Trigger the Battle Encounter
* Pass input to the MovementController

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

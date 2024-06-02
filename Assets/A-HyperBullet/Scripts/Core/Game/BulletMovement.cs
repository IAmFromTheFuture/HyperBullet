using UnityEngine;
using static Defines.Game;

public class BulletMovement : MonoBehaviour
{
    [SerializeField] private float _multiplier;
    [SerializeField] private float _minSpeed;

    [SerializeField] private CameraFollow _cameraFollow;

    private bool _isTurning;
    private float _acceleration;

    private void Start()
    {
        _isTurning = false;
        _acceleration = _minSpeed;
        _cameraFollow.UpdateTarget(transform, new Vector3(0, 1.3f, -2));
    }

    private void Update()
    {
        // Turn left or right
        handleTurn();

        // Speed up or down
        handleSpeed();
    }

    private void handleTurn()
    {
        if (!_isTurning
     && (Input.GetKeyDown(KeyCode.A)
     || Input.GetKeyDown(KeyCode.LeftArrow)))
        {
            _isTurning = true;
            rotateBullet(Direction.LEFT);
        }

        if (!_isTurning
            && (Input.GetKeyDown(KeyCode.D)
            || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            _isTurning = true;
            rotateBullet(Direction.RIGHT);
        }


        if (Input.GetKeyUp(KeyCode.A)
            || Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _isTurning = false;
        }

        if (Input.GetKeyUp(KeyCode.D)
            || Input.GetKeyUp(KeyCode.RightArrow))
        {
            _isTurning = false;
        }
    }

    private void handleSpeed()
    {
        if (Input.GetKeyDown(KeyCode.W)
            || Input.GetKeyDown(KeyCode.UpArrow))
        {
            _acceleration = 1;
        }

        if (Input.GetKeyDown(KeyCode.S)
            || Input.GetKeyDown(KeyCode.DownArrow))
        {
            _acceleration = 0.1f;
        }


        if (Input.GetKeyUp(KeyCode.W)
            || Input.GetKeyUp(KeyCode.UpArrow)
            || Input.GetKeyUp(KeyCode.S)
            || Input.GetKeyUp(KeyCode.DownArrow))
        {
            _acceleration = _minSpeed;
        }

        float _ = _acceleration * _multiplier * Time.deltaTime;

        transform.Translate(new Vector3(0, 0, _));
    }

    private void rotateBullet(Direction direction)
    {
        var _ = 0.0f;
        switch (direction)
        {
            case Direction.LEFT:
                _ = -90.0f;
                break;
            case Direction.RIGHT:
                _ = 90.0f;
                break;
        }

        Vector3 currentRotation;
        currentRotation = transform.localEulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y + _, currentRotation.z);
        transform.localEulerAngles = currentRotation;
    }

}

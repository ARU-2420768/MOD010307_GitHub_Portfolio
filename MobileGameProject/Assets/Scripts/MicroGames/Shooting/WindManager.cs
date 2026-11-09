using UnityEngine;

public class WindManager : MonoBehaviour
{

    [SerializeField] private Renderer displayRenderer;
    [SerializeField] private GameObject WindIndicatorAdjusted;
    [SerializeField] private GameObject WindIndicatorActual;
    [SerializeField] private GameConfigurationData gameConfigData;


    private Vector3 currentDirection;

    private float currentSpeed;
    private bool isScopeCalibrated = false;

    private void Start()
    {
        Vector3 position = WindIndicatorActual.transform.localPosition;

        position.x = Random.Range(-0.5f, 0.5f);
        position.y = Random.Range(-0.5f, 0.5f);

        WindIndicatorActual.transform.localPosition = position;

    }

    // Update is called once per frame
    private void Update()
    {
        if(isScopeCalibrated)
            return;

        HandleMovement();
    }

    public void MoveWindIndicator1(Vector2 direction)
    {

        Debug.Log($"MoveWindIndicator:  {direction}");

        Vector3 position = WindIndicatorAdjusted.transform.localPosition;
        position += (Vector3)direction * currentSpeed * Time.deltaTime;

        // clamp sphere within it's parents boundaries
        position.x = Mathf.Clamp(position.x, -0.45f, 0.45f);
        position.y = Mathf.Clamp(position.y, -0.45f, 0.45f);
        WindIndicatorAdjusted.transform.localPosition = position;
    }

    public void MoveWindIndicator(Vector2 direction)
    {
        Debug.Log($"MoveWindIndicator:  {direction}");
        currentDirection = direction;

    }

    public void StopMovement()
    {
        currentDirection = Vector3.zero;
    }

    public void StartMovement(Vector3 direction)
    {
        currentDirection = direction;
    }

    private void HandleMovement()
    {
        if (currentDirection == Vector3.zero)
        {
            currentSpeed = 0;
            return;
        }

        MovementProfileData movement = gameConfigData.movementProfile;

        // Accelerate up to max speed
        currentSpeed += movement.acceleration * Time.deltaTime;
        currentSpeed = Mathf.Clamp(currentSpeed, 0, movement.maxSpeed);

        Vector3 position = WindIndicatorAdjusted.transform.position;

        Bounds bounds = displayRenderer.bounds;
    
        position += (Vector3)currentDirection * currentSpeed * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, bounds.min.x, bounds.max.x);
        position.y = Mathf.Clamp(position.y, bounds.min.y, bounds.max.y);

        WindIndicatorAdjusted.transform.position = position;

        CheckForCalibration();

    }

    private void CheckForCalibration()
    {
        float distance = Vector2.Distance(WindIndicatorAdjusted.transform.localPosition, WindIndicatorActual.transform.localPosition);
        if(distance <= 0.005f)
            isScopeCalibrated = true;

    }


}

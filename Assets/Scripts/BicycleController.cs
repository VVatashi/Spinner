using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BicycleController : MonoBehaviour
{
    public Transform gear;
    public Transform leftPedal;
    public Transform rightPedal;
    public Text rpmText;

    public float angularVelocity = 0f;
    public float rpm = 0f;
    public float score = 0;

    private bool pressed = false;
    private Vector2 position = Vector2.zero;

    public void Update()
    {
        gear.Rotate(0, 0, -angularVelocity * Time.deltaTime);

        leftPedal.localRotation = Quaternion.Euler(gear.eulerAngles.z, 0, 0);
        rightPedal.localRotation = Quaternion.Euler(gear.eulerAngles.z, 0, 0);

        angularVelocity -= 0.2f * angularVelocity * Time.deltaTime;
        rpm = angularVelocity * 60f / 360f;
        score += Mathf.Abs(rpm * Time.deltaTime);

        rpmText.text = $"SCORE {(int)Mathf.Floor(score)}\n{(int)Mathf.Floor(rpm)} RPM";
    }

    public void OnPoint(InputValue input)
    {
        Vector2 newPosition = input.Get<Vector2>();
        newPosition.x = newPosition.x * 2f / Screen.width - 1f;
        newPosition.y = newPosition.y * 2f / Screen.height - 1f;

        Vector2 deltaPosition = newPosition - position;

        if (pressed)
        {
            if (deltaPosition.sqrMagnitude > 0)
            {
                Vector2 normal = newPosition.normalized;
                Vector2 tangent = Vector2.Perpendicular(normal);

                Vector2 direction = deltaPosition.normalized;
                angularVelocity -= 10f * 360f * Vector2.Dot(tangent, direction) * deltaPosition.magnitude * Time.deltaTime;
            }
        }

        position = newPosition;
    }

    public void OnClick(InputValue input)
    {
        pressed = input.Get<float>() > 0.5f;
    }
}

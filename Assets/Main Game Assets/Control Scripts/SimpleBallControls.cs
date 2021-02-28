using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class SimpleBallControls : MonoBehaviour
{
	public bool cameraRelativeMovement = false; // whether we use the camer's forward direction, otherwise use world space
	public float m_MovePower = 5; // The force added to the ball to move it.
	public bool m_UseTorque = true; // Whether or not to use torque to move the ball.
	public float m_MaxAngularVelocity = 25; // The maximum velocity the ball can rotate at.
	public float m_JumpPower = 2; // The force added to the ball when it jumps.
	
	private const float k_GroundRayLength = 1f; // The length of the ray to check if the ball is grounded.
	private Rigidbody m_Rigidbody;

	private Vector3 moveDirection;
    // the world-relative desired move direction, calculated from the camForward and user input.

    public Transform cam; // A reference to the main camera in the scenes transform
    private Vector3 camForward; // The current forward direction of the camera
    private bool jump; // whether the jump button is currently pressed


    private void Awake()
    {
		if (cameraRelativeMovement && cam == null) {
			Debug.LogError("Can't use camera relative movement in BallUserControl without a camera!");
		}
    }

	private void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		// Set the maximum angular velocity.
		GetComponent<Rigidbody>().maxAngularVelocity = m_MaxAngularVelocity;
	}

    private void Update()
    {
        // Get the axis and jump input.

        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        jump = CrossPlatformInputManager.GetButton("Jump");

        // calculate move direction
        if (cameraRelativeMovement)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            moveDirection = (v*camForward + h*cam.right).normalized;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            moveDirection = (v*Vector3.forward + h*Vector3.right).normalized;
        }
    }


    private void FixedUpdate()
    {
		// If using torque to rotate the ball...
		if (m_UseTorque)
		{
			// ... add torque around the axis defined by the move direction.
			m_Rigidbody.AddTorque(new Vector3(moveDirection.z, 0, -moveDirection.x)*m_MovePower);
		}
		else
		{
			// Otherwise add force in the move direction.
			m_Rigidbody.AddForce(moveDirection*m_MovePower);
		}
		
		// If on the ground and jump is pressed...
		if (Physics.Raycast(transform.position, -Vector3.up, k_GroundRayLength) && jump)
		{
			// ... add force in upwards.
			m_Rigidbody.AddForce(Vector3.up*m_JumpPower, ForceMode.Impulse);
		}
		jump = false;
    }
}

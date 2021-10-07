using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeflow : MonoBehaviour
{
	[Header("Components")]
	[SerializeField] private Animator animator = default;
	[SerializeField] private CharacterController controller = default;
	[SerializeField] private Camera playerCamera = default;

	[Header("Movement Properties")]
	[SerializeField] private float rotationSpeed = 0.2f;
	[SerializeField] private float movementSpeed = 5.3f;

	private Vector3 desiredMoveDirection = Vector3.zero;

	void Update()
	{
		Vector2 inputDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

		animator.SetFloat("Speed", inputDirection.normalized.sqrMagnitude, 0.1f, Time.deltaTime);

		if(inputDirection.sqrMagnitude > 0.1f)
			MoveAndRotate(inputDirection);

		if (!controller.isGrounded)
			controller.Move(new Vector3(0, Physics.gravity.y * Time.deltaTime, 0));
	}

	void MoveAndRotate(Vector2 moveAxis)
	{
		Vector3 forward = playerCamera.transform.forward;
		Vector3 right = playerCamera.transform.right;

		forward.y = 0f;
		right.y = 0f;

		forward.Normalize();
		right.Normalize();

		desiredMoveDirection = forward * moveAxis.y + right * moveAxis.x;

		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed);
		controller.Move(desiredMoveDirection * Time.deltaTime * movementSpeed);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAndRespawn : MonoBehaviour
{

    public float moveDistance;
    public float moveSpeed;
    public float respawnDelay;
    public Vector3 moveDirection = Vector3.forward;

    private Vector3 startingPosition;
    private float traveledDistance = 0f;
    private bool isMoving = true;
    private Renderer objectRenderer;

    void Start()
    {

        startingPosition = transform.position;

        objectRenderer = GetComponent<Renderer>();
        moveDirection = moveDirection.normalized;
    }

    void Update()
    {
        if (isMoving)
        {

            float step = moveSpeed * Time.deltaTime;

            transform.Translate(moveDirection * step);
            traveledDistance += step;


            if (traveledDistance >= moveDistance)
            {
                isMoving = false;

                if (objectRenderer != null)
                    objectRenderer.enabled = false;

                StartCoroutine(Respawn());
            }
        }
    }

    IEnumerator Respawn()
    {

        yield return new WaitForSeconds(respawnDelay);

        transform.position = startingPosition;

        traveledDistance = 0f;

        if (objectRenderer != null)
            objectRenderer.enabled = true;

        isMoving = true;
    }
}

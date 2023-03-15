using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private int numTargets = 20;
    [SerializeField] private float timePenalty = -0.01f;
    
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float moveSpeed = 5;
    private List<Transform> targets;
    public Transform environmentTransform;

    public override void OnEpisodeBegin()
    {
        if (targets == null)
        {
            targets = new List<Transform>();
        }
        else
        {
            // Destroy existing targets
            foreach (Transform target in targets)
            {
                Destroy(target.gameObject);
            }
            targets.Clear();
        }

        // Spawn new targets
        for (int i = 0; i < numTargets; i++)
        {
            Vector3 targetPosition = environmentTransform.TransformPoint(new Vector3(Random.Range(-19f, 19f), 0, Random.Range(-13f, 13f)));

            GameObject targetInstance = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
            targets.Add(targetInstance.transform);
        }

        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float rotateAmount = actions.ContinuousActions[0];
        float moveAmount = actions.ContinuousActions[1];


        // Rotate the agent
        transform.Rotate(Vector3.up, rotateAmount * Time.deltaTime * rotationSpeed);

        // Move the agent forward or backward
        transform.localPosition += transform.forward * moveAmount * Time.deltaTime * moveSpeed;

        SetReward(timePenalty * (1 - Mathf.Abs(rotateAmount) - Mathf.Abs(moveAmount)));
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Goal"))
        {
            SetReward(+1.0f);
            targets.Remove(other.transform);
            Destroy(other.gameObject);

            // End episode if all targets are collected
            if (targets.Count == 0)
            {
                float timeRemaining = (MaxStep - StepCount) / (float)MaxStep;
                SetReward(1f + timeRemaining);
                EndEpisode();
            }
        }

        if (other.CompareTag("Wall"))
        {
            SetReward(-10f);
            EndEpisode();
        }
    }
}

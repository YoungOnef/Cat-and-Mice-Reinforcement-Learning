using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System;
using Unity.MLAgents.Integrations.Match3;

public class MoveToGoalAgent : Agent
{
    // Serialized fields that can be set in the Unity Editor
    [SerializeField] private GameObject targetPrefab;
    private int numTargets = 10;
    [SerializeField] private float timePenalty = -0.0001f;
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float moveSpeed = 5;

    // List of targets and reference to the environment transform
    public List<Transform> targets;
    public Transform environmentTransform;
    //function to remove a target from the list and destroy its game object
    public void RemoveTarget(Transform target)
    {
        targets.Remove(target);
        Destroy(target.gameObject);
    }
    // function called at the start of each episode
    public override void OnEpisodeBegin()
    {
        // Initialize targets list if it is null
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

        // Spawn new targets at random positions within the environment
        for (int i = 0; i < numTargets; i++)
        {
            Vector3 targetPosition = environmentTransform.TransformPoint(new Vector3(UnityEngine.Random.Range(-19f, 19f), 0, UnityEngine.Random.Range(-13f, 13f)));


            GameObject targetInstance = Instantiate(targetPrefab, targetPosition, Quaternion.identity);
            targetInstance.GetComponent<FleeFromAgent>().SetMoveToGoalAgent(this);
            targets.Add(targetInstance.transform);
        }


        // Reset agent position and rotation
        transform.localPosition = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.identity;
    }
    // function called when the agent receives an action from the Reinforcement Learning algorithm
    public override void OnActionReceived(ActionBuffers actions)
    {
        // set the continuous actions
        float rotateAmount = actions.ContinuousActions[0];
        float moveAmount = actions.ContinuousActions[1];

        // Rotate the agent
        transform.Rotate(Vector3.up, rotateAmount * Time.deltaTime * rotationSpeed);
        // Move the agent forward
        transform.localPosition += transform.forward * moveAmount * Time.deltaTime * moveSpeed;

    }
    // Method to manually control the agent
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // Get the continuous actions
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }
    //function called when the agent collides with another object
    // Give reward or penalty depending on whether the agent collided with a target or a wall
    private void OnTriggerEnter(Collider other)
    {
        //If the agent collides with a target, remove the target from the list and add a reward
        if (other.CompareTag("Goal"))
        {
            SetReward(+0.1f);
            targets.Remove(other.transform);
            Destroy(other.gameObject);
            // End episode if all targets have been collected
            if (targets.Count == 0)
            {
                print("Targets collected");
                float timeRemaining = (MaxStep - StepCount) / (float)MaxStep;
                SetReward(1f + timeRemaining);
                EndEpisode();
            }
        }
        // If the agent collides with a wall, set a negative reward, end the episode
        if (other.CompareTag("Wall"))
        {
            SetReward(-1f);
            EndEpisode();
        }
    }

}

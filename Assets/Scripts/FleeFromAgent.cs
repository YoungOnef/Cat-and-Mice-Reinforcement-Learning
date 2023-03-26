using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static UnityEngine.GraphicsBuffer;

public class FleeFromAgent : Agent
{
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float timeSurvivedReward = 0.001f;
    private float timer;
    private MoveToGoalAgent moveToGoalAgent;
    public override void OnEpisodeBegin()
    {
        timer = 0f;

    }
    public void SetMoveToGoalAgent(MoveToGoalAgent agent)
    {
        moveToGoalAgent = agent;
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float rotateAmount = actions.ContinuousActions[0];
        float moveAmount = actions.ContinuousActions[1];


        transform.Rotate(Vector3.up, rotateAmount * Time.deltaTime * rotationSpeed);

        transform.localPosition += transform.forward * moveAmount * Time.deltaTime * moveSpeed;

        // Add reward for time survived
        timer += Time.deltaTime;
        AddReward(timeSurvivedReward * timer);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxis("Horizontal");
        continuousActions[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Agent"))
        {
            SetReward(-1f);
            EndEpisode();
        }


        if (other.CompareTag("Wall"))
        {
            print("Flee from agent Wall collided");
            SetReward(-1f);
            EndEpisode();
            moveToGoalAgent.OnEpisodeBegin();


        }
    }
}

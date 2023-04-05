using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using static UnityEngine.GraphicsBuffer;

public class FleeFromAgent : Agent
{
    // Serialized fields that can be set in the Unity Editor
    [SerializeField] private float rotationSpeed = 200f;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float timeSurvivedReward = 0.001f;
    private float timer;
    //
    private MoveToGoalAgent moveToGoalAgent;
    //Restarting the timer

    public override void OnEpisodeBegin()
    {
        timer = 0f;

    }
    //function to set the MoveToGoalAgent
    public void SetMoveToGoalAgent(MoveToGoalAgent agent)
    {
        moveToGoalAgent = agent;
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

        // Add reward for time survived
        timer += Time.deltaTime;
        AddReward(timeSurvivedReward * timer);

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
    private void OnTriggerEnter(Collider other)
    {
        // If the agent collides with another agent, set a negative reward and end the episode
        if (other.CompareTag("Agent"))
        {
            SetReward(-1f);
            EndEpisode();
        }
        // If the agent collides with a wall, set a negative reward, end the episode, and reset the MoveToGoalAgent object
        if (other.CompareTag("Wall"))
        {
            SetReward(-1f);
            EndEpisode();
            //moveToGoalAgent.OnEpisodeBegin();

        }
    }
}

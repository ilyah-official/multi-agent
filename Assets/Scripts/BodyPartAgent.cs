using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class BodyPartAgent : Agent
{
    [SerializeField] private string namaTim = "";
    [SerializeField] private float moveSpeed = 1f;

    // Internal variables
    [HideInInspector] public Transform buddyAgent;
    [HideInInspector] public Transform targetObject;
    [HideInInspector] public AgentManager agentManager;
    [HideInInspector] public bool randomizeStartPosition;
    [HideInInspector] public float moveX, moveZ;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            SetReward(-1f);
            agentManager.ForceEndEpisodeForAllAgents();
        }
    }
    public override void OnEpisodeBegin()
    {
        if(randomizeStartPosition)
            transform.position = new Vector3(Random.Range(-4.4f, 4.4f), 1, Random.Range(1.5f, -9f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(buddyAgent.position);
        sensor.AddObservation(targetObject.position);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveZ = actions.ContinuousActions[1];

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }

    // Kontrol agennya pakai WASD.
    // Hanya untuk testing saja.
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
}

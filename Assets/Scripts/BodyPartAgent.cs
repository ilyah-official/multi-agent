using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
public class BodyPartAgent : Agent
{
    [SerializeField] private int team = -1; // ubah di properti
    [SerializeField] private string namaTim = "";
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private Transform buddy1;
    [SerializeField] private Transform Target1;

    private Vector3 startPosition;
    private RaycastHit hit;
    private bool isValid = false;
    [HideInInspector] public float moveX, moveZ;
    private void Start()
    {
        ValidateMultiAgent();
    }
    private void Update()
    {
        if (isValid)
        {
            Debug.DrawLine(transform.position, buddy1.position, Color.white);
            if (Physics.Raycast(transform.position, buddy1.position - transform.position, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
                {
                    Debug.DrawLine(transform.position, buddy1.position, Color.green);
                    SetReward(+5f);
                    //EndEpisode();
                    Debug.Log(namaTim + " Berhasil Mencapai Target");
                    GoalManager.timGoal = team;
                    if (team == 1)
                    {
                        GoalManager.totalRewardTimBiru += 5;
                    }
                    else if (team == 2)
                    {
                        GoalManager.totalRewardTimKuning += 5;
                    }
                    GoalManager.currentEpisode += 1;
                }
                else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Agent"))
                {
                    Debug.DrawLine(transform.position, buddy1.position, Color.red);
                    SetReward(-1f);
                    EndEpisode();
                    if (team == 1)
                    {
                        GoalManager.totalRewardTimBiru -= 1;
                    }
                    else if (team == 2)
                    {
                        GoalManager.totalRewardTimKuning -= 1;
                    }
                    GoalManager.currentEpisode += 1;
                }
            }
            if (GoalManager.timGoal != team && GoalManager.timGoal != -1)
            {
                Debug.Log(gameObject.name + " has completed " + this.CompletedEpisodes + " episodes");
                SetReward(-1f);
                EndEpisode();
                if (team == 1)
                {
                    GoalManager.totalRewardTimBiru -= 1;
                }
                else if (team == 2)
                {
                    GoalManager.totalRewardTimKuning -= 1;
                }
                GoalManager.currentEpisode += 1;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.DrawLine(transform.position, buddy1.position, Color.red);
            SetReward(-1f);
            EndEpisode();
            if (team == 1)
            {
                GoalManager.totalRewardTimBiru -= 1;
            }
            else if (team == 2)
            {
                GoalManager.totalRewardTimKuning -= 1;
            }
            GoalManager.currentEpisode += 1;
        }
    }
    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(Random.Range(-4.4f, 4.4f), 1, Random.Range(1.5f, -9f));
        Target1.position = new Vector3(Random.Range(-4, 4), 1, Random.Range(1.3f, -8f));
        GoalManager.timGoal = -1;
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position);
        sensor.AddObservation(buddy1.position);
        sensor.AddObservation(Target1.position);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        moveX = actions.ContinuousActions[0];
        moveZ = actions.ContinuousActions[1];

        transform.position += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }
    // hanya dipakai ketika tidak training
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }
    // Tidak bisa hanya 1 agent, karena min 2 untuk
    // jadi satu garis
    private void ValidateMultiAgent()
    {
        if (buddy1 == null)
        {
            Debug.LogError("Agent tidak memiliki teman...");
        }
        else
        {
            isValid = true;
            startPosition = transform.position;
        }
    }
}

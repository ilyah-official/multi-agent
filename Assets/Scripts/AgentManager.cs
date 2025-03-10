using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : MonoBehaviour
{
    // Settings
    [SerializeField] public BodyPartAgent[] team_blue;
    [SerializeField] public BodyPartAgent[] team_yellow;
    [SerializeField] private Transform target;
    [SerializeField] private bool RandomizeAgentPosition = true;
    [SerializeField] private LineRenderer yellowLine, blueLine;
    public bool isBuildMode = false;

    // Skor
    [HideInInspector] public string currentEpisode;
    [HideInInspector] public int totalRewardBiru, totalRewardKuning;

    // Internal variables
    private RaycastHit tali;
    private int result = 0;
    private bool call_end = false;
    private void Start()
    {
        if (!isBuildMode)
        {
            InitializeAgents();
        }
    }
    public void InitializeAgents()
    {
        totalRewardBiru = 0;
        totalRewardKuning = 0;
        team_blue[0].buddyAgent = team_blue[1].transform;
        team_blue[0].targetObject = target;
        team_blue[0].agentManager = this;
        team_blue[0].opposingAgent1 = team_yellow[0].transform;
        team_blue[0].opposingAgent2 = team_yellow[1].transform;

        team_blue[1].buddyAgent = team_blue[0].transform;
        team_blue[1].targetObject = target;
        team_blue[1].agentManager = this;
        team_blue[1].opposingAgent1 = team_yellow[0].transform;
        team_blue[1].opposingAgent2 = team_yellow[1].transform;

        team_yellow[0].buddyAgent = team_yellow[1].transform;
        team_yellow[0].targetObject = target;
        team_yellow[0].agentManager = this;
        team_yellow[0].opposingAgent1 = team_blue[0].transform;
        team_yellow[0].opposingAgent2 = team_blue[1].transform;

        team_yellow[1].buddyAgent = team_yellow[0].transform;
        team_yellow[1].targetObject = target;
        team_yellow[1].agentManager = this;
        team_yellow[1].opposingAgent1 = team_blue[0].transform;
        team_yellow[1].opposingAgent2 = team_blue[1].transform;
    }
    // Update is called once per frame
    void Update()
    {
        if (!isBuildMode)
        {
            currentEpisode = team_blue[0].CompletedEpisodes.ToString(); // semua agen telah dilakukan sinkronisasi episode, jadi setiap agen pasti memiliki episode yg sama

            call_end = false;
            result = 0;
            SetAgentStartPositionBehaviour(RandomizeAgentPosition);
            if (team_blue != null && team_blue.Length == 2)
            {
                if (team_yellow != null && team_yellow.Length == 2)
                {
                    // BLUE
                    result = InitializeBond(team_blue[0].transform.position, team_blue[1].transform.position, blueLine);
                    if (result == 1)
                    {
                        team_blue[0].SetReward(+5f);
                        team_blue[1].SetReward(+5f);
                        totalRewardBiru += 5;
                        team_yellow[0].SetReward(-1f);
                        team_yellow[1].SetReward(-1f);
                        totalRewardKuning -= 1;
                        call_end = true;
                    }
                    else if (result == -1)
                    {
                        team_blue[0].SetReward(-1f);
                        team_blue[1].SetReward(-1f);
                        call_end = true;
                    }

                    // YELLOW
                    result = InitializeBond(team_yellow[0].transform.position, team_yellow[1].transform.position, yellowLine);
                    if (result == 1)
                    {
                        team_yellow[0].SetReward(+5f);
                        team_yellow[1].SetReward(+5f);
                        totalRewardKuning += 5;
                        team_blue[0].SetReward(-1f);
                        team_blue[1].SetReward(-1f);
                        totalRewardBiru -= 1;
                        call_end = true;
                    }
                    else if (result == -1)
                    {
                        team_yellow[0].SetReward(-1f);
                        team_yellow[1].SetReward(-1f);
                        totalRewardKuning -= 1;
                        call_end = true;
                    }
                }
                else
                {
                    // BLUE
                    result = InitializeBond(team_blue[0].transform.position, team_blue[1].transform.position, blueLine);
                    if (result == 1)
                    {
                        team_blue[0].SetReward(+5f);
                        team_blue[1].SetReward(+5f);
                        totalRewardBiru += 5;
                        call_end = true;
                    }
                    else if (result == -1)
                    {
                        team_blue[0].SetReward(-1f);
                        team_blue[1].SetReward(-1f);
                        totalRewardBiru -= 1;
                        call_end = true;
                    }
                }
            }
            else if (team_yellow != null && team_yellow.Length == 2)
            {
                // YELLOW
                result = InitializeBond(team_yellow[0].transform.position, team_yellow[1].transform.position, yellowLine);
                if (result == 1)
                {
                    team_yellow[0].SetReward(+5f);
                    team_yellow[1].SetReward(+5f);
                    totalRewardKuning += 5;
                    call_end = true;
                }
                else if (result == -1)
                {
                    team_yellow[0].SetReward(-1f);
                    team_yellow[1].SetReward(-1f);
                    totalRewardKuning -= 1;
                    call_end = true;
                }
            }

            if (call_end)
            {
                ForceEndEpisodeForAllAgents();
            }
        }
    }

    public void ForceEndEpisodeForAllAgents()
    {
        if (team_blue != null && team_blue.Length == 2)
        {
            if (team_yellow != null && team_yellow.Length == 2)
            {
                // BLUE & YELLOW
                team_blue[0].EndEpisode();
                team_blue[1].EndEpisode();
                team_yellow[0].EndEpisode();
                team_yellow[1].EndEpisode();
            }
            else
            {
                // BLUE
                team_blue[0].EndEpisode();
                team_blue[1].EndEpisode();
            }
        }
        else if (team_yellow != null && team_yellow.Length == 2)
        {
            // YELLOW
            team_yellow[0].EndEpisode();
            team_yellow[1].EndEpisode();
        }
        target.position = new Vector3(Random.Range(-4, 4), 1, Random.Range(1.3f, -8f));
    }

    public void SetAgentStartPositionBehaviour(bool value)
    {
        team_blue[0].randomizeStartPosition = value;
        team_blue[1].randomizeStartPosition = value;
        team_yellow[0].randomizeStartPosition = value;
        team_yellow[1].randomizeStartPosition = value;
    }

    /// <summary>
    /// Melakukan raycast dari satu agen ke temannya.
    /// </summary>
    /// <param name="a">Posisi agen pertama</param>
    /// <param name="b">Posisi agen kedua</param>
    /// <returns>Sebuah integer, 1 = tali kena target, -1 = kena selain target, 0 = tali tidak kena benda apapun</returns>
    private int InitializeBond(Vector3 a, Vector3 b, LineRenderer line)
    {
        line.SetPosition(0, a);
        line.SetPosition(1, b);
        if (Physics.Raycast(a, b - a, out tali))
        {
            if (tali.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                return 1;
            }
            else if (tali.collider.gameObject.layer != LayerMask.NameToLayer("Agent") && tali.collider.gameObject.layer != LayerMask.NameToLayer("Obstacle"))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        else if (Physics.Raycast(b, a - b, out tali))
        {
            if (tali.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
            {
                return 1;
            }
            else if (tali.collider.gameObject.layer != LayerMask.NameToLayer("Agent") && tali.collider.gameObject.layer != LayerMask.NameToLayer("Obstacle"))
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            return 0;
        }
    }
}

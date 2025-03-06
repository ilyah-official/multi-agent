# Team-Based Competitive Multi Agent System
A system that enables interaction between one agent and another using reinforcement learning (MLAgents Unity). The user manual is available in the file named "Manual_Guide.pdf" and can be downloaded in the release v1.0 section.
![Screenshot](Thumbnail.png)

# Main Features
1. Two teams: Yellow and Blue
2. Collaboration system within a team (interaction occurs among agents in the same team)
3. Competition system between two teams (interaction occurs among all agents)
4. Punishment system
5. Reinforcement learning

# Flow Chart
![Screenshot](Architecture.png)

In general, this flowchart consists of three main parts. The first part is the preparation of two agent models. The second part involves training both agent models within the same environment. The training is conducted separately, so they do not immediately compete against each other. The third part is the testing phase. Model 1 and Model 2, which were trained separately in the second part, will be used in the same environment to determine which model performs best.

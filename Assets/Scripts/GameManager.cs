using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Barracuda.ONNX;
using System.IO;
using Unity.Barracuda;
using Unity.MLAgents.Policies;
public class GameManager : MonoBehaviour
{
    public GameObject mainPanel;
    public TMP_InputField yellow_path, blue_path;
    public AgentManager agentManager;
    public NNModel yellow, blue;
    public BehaviorParameters yellow_p1, yellow_p2, blue_p1, blue_p2;
    public void StartGame()
    {
        yellow = LoadNNModel(yellow_path.text, "AgentBehaviour", yellow_p1, yellow_p2);
        blue = LoadNNModel(blue_path.text, "AgentBehaviour", blue_p1, blue_p2);

        if (yellow != null && blue != null)
        {
            agentManager.InitializeAgents();

            agentManager.team_yellow[0].gameObject.SetActive(true);
            agentManager.team_yellow[1].gameObject.SetActive(true);
            agentManager.team_blue[0].gameObject.SetActive(true);
            agentManager.team_blue[1].gameObject.SetActive(true);
            agentManager.isBuildMode = false;
            mainPanel.gameObject.SetActive(false);
        }
    }

    NNModel LoadNNModel(string modelPath, string modelName, BehaviorParameters _param1, BehaviorParameters _param2)
    {
        var converter = new ONNXModelConverter(true);
        Model model = converter.Convert(modelPath);
        NNModelData modelData = ScriptableObject.CreateInstance<NNModelData>();
        using (var memoryStream = new MemoryStream())
        using (var writer = new BinaryWriter(memoryStream))
        {
            ModelWriter.Save(writer, model);
            modelData.Value = memoryStream.ToArray();
        }
        modelData.name = "AgentBehaviour";
        modelData.hideFlags = HideFlags.HideInHierarchy;
        NNModel result = ScriptableObject.CreateInstance<NNModel>();
        result.modelData = modelData;
        result.name = modelName;
        _param1.Model = result;
        _param2.Model = result;
        return result;
    }
}

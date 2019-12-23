using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Aircraft
{

	public class AircraftArea : MonoBehaviour
	{
		[Tooltip("The Path the race will take")]
    	public CinemachineSmoothPath racePath;

		[Tooltip("The prefab to use for checkpoints")]
		public GameObject checkpointPrefab;

		[Tooltip("The Prefab to use for the start/end checkpoint")]
		public GameObject finishCheckpointPrefab;

		[Tooltip("If true, enable training mode")]
		public bool trainingMode;

		public List<AircraftAgent> AircraftAgents {get;private set;}

		public List<GameObject> Checkpoints {get;private set; }

		public AircraftAcademy AircraftAcademy {get;private set;}

		///<sumarry>
		/// Actions to perform when the scripts wakes up
		/// </summary>

		private void Awake()
		{
			AircraftAgents = transform.GetComponentsInChildren<AircraftAgent>().ToList();
			Debug.Assert(AircraftAgents.Count > 0, "No AircraftAgents found");

			AircraftAcademy = FindObjectOfType<AircraftAcademy>();
		}

		//Set up the area
		private void Start()
		{
			Debug.Assert(racePath != null, "Race Path was not set");
			Checkpoints = new List<GameObject>();
			int numCheckPoints = (int)racePath.MaxUnit(CinemachinePathBase.PositionUnits.PathUnits);

			for(int i=0;i< numCheckPoints;i++)
			{
				//instantiate either a checkpoint or finish line checkpoint
				GameObject checkpoint;
				if(i == numCheckPoints -1) checkpoint = Instantiate<GameObject>(finishCheckpointPrefab);
				else checkpoint = Instantiate<GameObject>(checkpointPrefab);

				//Set the parent, position, and rotation
				checkpoint.transform.SetParent(racePath.transform);
				checkpoint.transform.localPosition = racePath.m_Waypoints[i].position;
				checkpoint.transform.rotation = racePath.EvaluateOrientationAtUnit(i, CinemachinePathBase.PositionUnits.PathUnits);

				//add the checkpoint to the list

				Checkpoints.Add(checkpoint);
			}
		}

		//Resets the position of an agent using its current NextCheckpointIndex, unless randomize is true,
		//then will pick a new random checkpoint
		public void ResetAgentPosition(AircraftAgent agent, bool randomize = false)
		{
			if(randomize)
			{
				//pick a new next checkpoint at random
				agent.NextCheckpointIndex = Random.Range(0, Checkpoints.Count);
			}

			// Set start position to the previous checkpoint
			int previousCheckpointIndex = agent.NextCheckpointIndex - 1;
			if(previousCheckpointIndex == -1) previousCheckpointIndex = Checkpoints.Count -1;

			float startPosition = racePath.FromPathNativeUnits(previousCheckpointIndex, CinemachinePathBase.PositionUnits.PathUnits);

			//Convert the position on the racepath to a position in 3d space
			Vector3 basePosition = racePath.EvaluatePosition(startPosition);

			//get the orientation at that position on the race path
			Quaternion orientation = racePath.EvaluateOrientation(startPosition);

			//calculate a horizontal offset so that agents are spread out
			Vector3 positionOffset = Vector3.right * (AircraftAgents.IndexOf(agent) - AircraftAgents.Count / 2f) * 10f;

			//set the aircraft position and rotation
			agent.transform.position = basePosition + orientation * positionOffset;
			agent.transform.rotation = orientation;
		}
	}
}
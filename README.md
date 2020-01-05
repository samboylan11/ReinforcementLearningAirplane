# ReinforcementLearningAirplane
Airplane learning to fly through a race track using Deep Reinforcement Learning and Unity(Using MLAgents 0.11.0). Each time the agent goes through the checkpoint prefab it is rewarded with +1, and each time it hits a rock or the map it gets punished with -1.

Built with C# and Python.

Dependencies: 
MLAgents, TensorFlow, InputSystem

Every video is done with the same neural network brain. I trained them with 4 different racetracks so they can generalize their knowledge (and prevent overfitting) to tracks they have never seen before.

Training the agent in a loop: 

![](LoopAircraftGIF.gif)

Training the agent in a Loop with sparse rewards:

![](LoopSparseAircraftGIF.gif)

Training the agent in a 8 loop:

![](8aircraft.gif)

Training the agent in a difficult course with obstacles:

![](HARDaircraftAgentGIF.gif)

Made By: Samuel Boylan-Sajous

Learned MLAgents code from https://www.udemy.com/course/ai-flight/ course

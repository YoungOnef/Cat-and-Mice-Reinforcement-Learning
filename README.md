# Cat and Mice - Reinforcement Learning (RL) Unity Game with AI

![Cat and Mice](https://github.com/YoungOnef/GameAI/assets/72264732/ca4bfe99-3127-4202-8b20-f78786f3157a)

## Introduction
This project focuses on using Reinforcement Learning (RL) as a machine learning technique to train agents to play a game in a grid world. RL allows an agent to learn from its decisions by interacting with the environment and trying to maximize cumulative rewards. The agent receives feedback in the form of rewards or penalties, guiding it to improve its actions over time. The project consists of two scenarios: Scenario 1, where the agent is an AI cat trying to catch mice, and Scenario 3, where the agent is an AI mouse trying to avoid the cat.

## Challenges and Limitations of Reinforcement Learning for Scenarios 1 and 3
1. Slow learning with unclear rewards: If the reward signal is not clear or takes a long time to receive, the agent may take a lot of time to learn what to do. Providing clearer and faster rewards can speed up the learning process.

2. Getting stuck in loops: The agent might get stuck in a loop if there are multiple possibilities, like going after two mice in opposite directions. Using additional rewards based on time taken can encourage faster collection.

3. Learning suboptimal strategies: The agent might learn inefficient strategies, like doing random moves in a circle to collect mice. Incorporating exploration policies can help the agent try different approaches.



https://github.com/YoungOnef/GameAI/assets/72264732/0c9d4767-f63f-47a4-b608-5fb6c220b51f



## Conclusion
The project successfully implements Reinforcement Learning to train agents to play a game in a grid world. RL proves to be effective in handling dynamic and uncertain environments, allowing agents to optimize their behavior over time. Challenges and limitations of RL were identified, and the comparison with other techniques favored the use of RL in these scenarios. Overall, the project demonstrates the suitability and effectiveness of Reinforcement Learning in training agents for complex game scenarios.

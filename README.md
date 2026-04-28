# GDIM33 Vertical Slice
## Milestone 1 Devlog
### Visual Scripting Graph
Right now, I'm using a VS graph to handle my UI manager. It works by having object variables for the different canvases and TMP objects in the scene, and having various custom events that update these UI elements. It is called by the game manager state machine to update text like the current task, timer, and accuracy displays. When the game state changes from the typing portion to the resource collection or vice versa, there's a custom event that turns on/off the appropriate canvases so only the relevant UI elements are shown. There's also a seperate canvas for the resource values, which is shown across the 2 game states (since you deplete resources while typing, and regain them during resource collection). 


### State Machine
[Updated Breakdown](https://docs.google.com/drawings/d/1FEoPs119xbTbxzRwz5_9L2jFhjPmUyWcokSxR4krJ34/edit?usp=sharing)

I updated the breakdown by more accurately writing out what the GameManager does, including it's 2 major states and what it does in each. I also added the UIManager (which wasn't there previously) and how it interacts with the GameManager. 


The State Machine in my project controls the game state. Because there are 2 major sections of the game, the typing and resource collecting, it makes it easier to handle these 2 states with the state machine. I can keep graphs specific to each seperated state. During the typing state, on enter it randomly selects some tasks. Then it spawns in the paper that has the task text on it, and sends the "current" task to the typing script and the UIManager to update. Everytime the player ejects a paper, it takes the accuracy calculated from the typing script and updates the accuracy score for the UIManager, and then sends in the next task. So on and so forth until all tasks in the list are done, which then triggers the state transition. On enter to the resource state, it switches the camera, the UI, and spawns in a semi random selection of resources into the center. There's always at least 1 of each, and then the rest is randomized. It starts up a timer as well which is updated through the UIManager. Once the timer is up, it switches back to the typing state and restarts. 

Since the state machine is the game manager, it communicates with almost every other part of the game. It communicates with the typing script, a script that intakes what the player is typing. The game manager sends the typing script the target text so it can accurately compare the strings and update the player's accuracy. The typing script detects when the player hits tab, which it then updates the game manager's overall accuracy score with that task's accuracy, as well as indicating that the task is done so that the game manager can send a new task to the UI and typing scripts, or move onto the resource state. Eventually, it'll also likely handle SFX and VFX once I implement that. 


## Milestone 2 Devlog
Milestone 2 Devlog goes here.
## Milestone 3 Devlog
Milestone 3 Devlog goes here.
## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- Cite any external assets used here!

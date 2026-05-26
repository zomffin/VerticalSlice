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
1. Implementation plan
The main feature to be added for this milestone is the NPCs during the resource period. These NPCs will grab items and take them to their own "bag" of items, where the item will be deleted (so the player can't get it).

Big Steps:
1. Basic functionality for a targetting + movement system 
    a. Using the movetowards method, have the npc move towards a static position
    b. Have a List of items, which the NPC randomly targets
   
2. State machine for targetting 
   a. Have the game manager send/update list of items
   b. Once the NPC collides with the targetted item, return back and drop item
   c. Remove the item from the list and delete the GameObject. 

2. 
To be completely honest, I forgot to do this breakdown ahead of time... However, it is sorta how I approach my problem solving anyways. I think the biggest issue with planning like this is I start noticing possible errors ahead of time and get too obsessed with them to move forward.

3. 
There's a lot of scripts and graphs talking between one another.... One of the main ones I added for this milestone involves the NPC. The NPC script contains a method to copy a list of gameobjects, which the gamemanager (the one spawning the resources) calls to send the list to. I ran into a lot of problems with null refs with this (because these resources get deleted by both the player and the NPC).
In my scripts, I use CustomEventTrigger rather than what was spoken about in class. I used it before we learned about it, so I ended up using it more.
<img width="1243" height="580" alt="image" src="https://github.com/user-attachments/assets/91adf83b-2fe4-428b-b559-e5fb42a38711" />
Here's one of the uses of a C# script method from my GameManager graph. This screenshot comes at the end of a full method that spawns the gameobjects, so its passing the reference to the list to the NPCs (there's only one currently but there will be more in the future, hence the foreach loop). 

5. 
I am heavily using scriptable objects in my Project. Currently, I have 2 types- one called "Events" (for future story events, currently there's only a tutorial) and "Tasks" (the main gameplay quest).
They're found in Assets/Assets/Tasks or StoryEvents. They're Tasks.cs,  TypingTask.cs, and Event.cs

Events have a list of associated tasks, if these tasks spawn one a time, as well as a bunch of info of how/when it's triggered- which game round, if it's triggered by a string detected within completed tasks, etc. 
Tasks themselves contain info for what the player has to type ("raw text"), the task name (appears in UI), a character count (for future difficulty scaling) as well as a second type of Task that has a raw text thats different from the display text (what appears on the task paper). 

## Milestone 3 Devlog
1. The shader I'm using for my game becomes obvious on a game over. If you want to find it quickly, just spam text into the typewriter and then delete a bunch (it's quickest to get rid of the whiteout).  It's a fullscreen shader, and it's a slight staticy effect that grows in intensity when a game over is triggered (but it's always present at a low opacity). It uses a static node that's first combined with the screen render, and then put into a lerp node with the screen render. The T value is by default low (0.1), but through a visual script the T value goes up to 4.0 so that the static effect overtakes the screen. It's also set to multiply, which causes the shader to darken the screen. I did this mainly because I didn't want the white parts of the static to lighten the screen, since I like the effect of the full darkening more (kind of like the player character is passing out or something?). There's also essentially a "copy" of this shader but specifically for the UI canvas (also for the gameover).
I used this article as reference to "animate" the static: [Medium Article](https://vintay.medium.com/create-a-white-noise-shader-with-shader-graph-in-unity-220dd9d24e92).
<img width="2543" height="1198" alt="image" src="https://github.com/user-attachments/assets/3f5447d6-1b0e-43cb-8901-1703824dd372" />

2. Most of my issues from playtesting are surrounding the game design and how information is conveyed. Right now, I've added an extra camera movement between the "typing" mode and an "overview" camera to the main typing portion. That way, it's much more obvious when you're doing one or the other (and eventually, I hope to also add some animations to the player's "hand" to continue pushing this). It would also help a lot to get more art and models implemented, but I think I'm going to have to make them since I have some specific needs for the models in particular. 
3. The main bit of new gameplay content is that the game now gets "harder" as rounds go by. First off, more of the "npcs" will spawn in as time passes, meaning you have to compete against more of them during the resource phase. You also start getting more tasks per round as you go along (currently there's no cap). I also implemented some audio, so there's a bit more atmosphere now. There's typing noises (but none for deleting or ejecting a page yet), some quiet noise, a song that will randomly play (I wanted to do something similar to minecraft where music only plays randomly- it won't play for the first minute but after that it has a random chance of triggering... except it triggers like instantly so I think my code is messed up), and some radio/white noise for game overs. 

## Milestone 4 Devlog
Milestone 4 Devlog goes here.
## Final Devlog
Final Devlog goes here.
## Open-source assets
- [Typewriter Model](https://vertexcat.itch.io/typewriter-set), textures are by me

### Audio
- [Ambience](https://freesound.org/people/Littleboot/sounds/147300/)
- [Music](https://pixabay.com/music/ambient-intense-horror-game-ambience-418842/)
- [Typing Sounds](https://freesound.org/people/BryanSaraiva/packs/44114/)


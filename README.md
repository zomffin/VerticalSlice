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

## Final Devlog
### 1. Core Gameplay Loop and Content, Vertical Slice
The core gameplay loop and content is the back and forth between typing tasks and collecting resources. The current goal is just to survive as long as possible by making as few errors as possible, and collecting as much resources as possible. There are 10+ different tasks and 3 different resources to manage, as well as a unique "tutorial" set of tasks that don't reappear in later gameplay. 

This slice illustrates the main gamne design and gameplay loop without actual story/puzzle progression. The full game would have a story, more tasks, and hopefully more complex tasks with small puzzles associated with them. I suppose in a way it's not exactly a vertical slice as there isn't much "horizontally" I imagine for this game, but instead deeper mechanics that build on what I already have so far. 



### 2. Renderring Effect
The renderring effect is activated when the player dies. First, it checks if a gameover is confirmed. In code, the typing script detects when a resource is depleted and activates a custom event in the state machine graph, which checks if there is more than 1 task remaining. This is mainly for an edge case if, somehow, the player runs out of materials just upon completing a task (Ex. running out of ink on the last character of a task), in which they should be allowed to go back for more resources and survive for a bit longer. Once this case is checked, then the renderring effect is triggered. A coroutine is started that increases the T value of a lerp node until it hits 4.0. At 4.0 it basically completely blacks out the screen, except for just a slight bit of noise in the center (which I don't mind). Then the proper gameover screen is displayed with the player stats. 

Image of the graph that activates/changes the T value of the shader. 
<img width="1939" height="791" alt="image" src="https://github.com/user-attachments/assets/b8e9bafd-1908-440e-90be-36fc9c87bc56" />




### 3. Planning

1. I don't really like the bubble diagrams, but I do like the task step break-downs.

The bubble break downs, in my experiences, tend to be kind of limiting unless you're only trying to visualize how different systems may interact. They quickly get messy as the systems get more complex and interconnected. I find it annoying when I start runnning out of room. If it's a physical/on paper, I can't do much other than try to restart and clean up the previous iteration, while digitally I have to go through resizing and moving everything around to compensate. It's a relatively small problem, but for me this sort of friction is enough to demotivate me so I try to streamline or avoid stuff like this. 

On the other hand, the task step break-downs align more with how I work/think. I find the task break-downs easier to adjust as things get more complex- adding a new line or number is much easier than having to reorganize an entire bubble break down. I also tend to take notes like this anyways, so it's something I'm used to. This also allows me to track progress easily, as well as add notes at relevant points/parts. For example, when I'm going through a task breakdown, if I get to a point that I'm not sure how I'm going to approach, I usually will come back ahead of time with ideas I think of or research I've done on how to do it. It also helps me to just get to work when tasks are broken down into smaller pieces like this, since it seems more digestible.


2. Breaking down a large project into smaller steps helps make the full scope of a project clear. I usually start ideas with something more nebulous or trying to capture a specific experience. As I start to break it down, it brings clarity to what the project will actually be like. In particular, I'm not very advanced at programming, so starting to break it down helps me understand if it's something I'll have to learn/research how to do, or something I can break down and figure out with my current knowledge. Having to learn new skills makes the scope much, much larger on an individual scale.

3. With this project, I definitely used the main pitch as part of my plan. It helped having each element of the game written out ahead of time in broad strokes so I knew what to aim for, then I would make my smaller task break downs. The pitch itself, in a way, was a breakdown. I had my general idea (a creepy typing game), but by breaking it down into each part (the game design, the visuals, the audio, etc.) made it more concrete and reachable. Along with these task breakdowns, I started a notepad for all the playtests. I sorted issues by either being a bug (some unintended thing that's broken) or a game design issue (some action or confusion I observe that needs addressing). A lot of the playtesting revealed game design issues, and occassionally bugs. I do something similar to a task breakdown. With bugs, I'll note down what the bug itself is, then some personal theories on what it is (so once I go to solve it, I have somewhere to start investigating), while with game design issues, I write down some ideas that may help, again, as a starting point.

I think it was an overall success. It's not the *most* organized, but it was organized enough for me to get through tasks and make consistent progress on the project. I think in the future, I'll continue operating this way, but I hope to refine it further and maybe find better ways to take and organize my notes. 

## Open-source assets
- [Typewriter Model](https://vertexcat.itch.io/typewriter-set), textures are by me

### Audio
- [Ambience](https://freesound.org/people/Littleboot/sounds/147300/)
- [Music](https://pixabay.com/music/ambient-intense-horror-game-ambience-418842/)
- [Typing Sounds](https://freesound.org/people/BryanSaraiva/packs/44114/)


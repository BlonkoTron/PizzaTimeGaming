# PizzaTimeGaming
 3dverdenProjekt

Programming of 3D Worlds Mini-Project Report
By Marcus Wenzel 

--Crazy Taxi but Worse--
The idea is to recreate the crazy taxi game. The main loop is driving a car from point A to Point B. Theres a timer that will count down and every time you get to a pickup point it will tell you were you need to go to and once you get that Drop-off location you will be awarded points. Rince and repeat until you run out of time. To make it harder then there should be cars that move around the map to create obstacles that the players has to avoid as to not lose time.
Objects in the game

--Player Car--

-Wheels-

Firstly, for the players car we have the wheels and this is where I used help from a tutorial (see reference 1). It is split into three parts that apply to all four wheels of the car which are made from multiple Raycast.
• Suspension
	o Adds force upwards on the cars’ rigid body to hold the car up and give it some springiness.
		- Based on stats defined in the unity inspector

• Acceleration / deceleration(SlowDown)
	o We apply a force either in the forward or backwards direction that the wheel is facing. The backwards one is based on the speed of the car. 
		-Using an animation curve to control the speed over time so you have quicker acceleration at the start compared to the end
• Turning
	o We simply turn the wheels but to avoid sliding we apply a force to the right of the wheel equal to the dot product of the velocity vector.
	o Uses an animation curve to reduce tire grip based on the car’s velocity. When you move fast you can’t turn as tight.

-Car Input Handler-
• This just assigns some values in the wheels based on the inputs received from the Player Input.

--Game Manager--
The game manager is a singleton that has a bunch of functions. Its singleton to make it easy for all scripts to access it.
• StartGame()
	o It calls other functions that enable stuff like the pickup points.
• StartDelivery() / EndDelivery()
	o These get called everytime you start and finish a “Delivery” it just enables the right game objects and adds time based on distance from the pickup point to the dropoff point. Then when you end the delivery then you get points based on distance from the pickup, and the pickup points are enabled again to start the loop again.
• GameTimer()
	o Game timer is an IEnumirator that will check if the timer has reached 0 and if it has it will end the game. If it hasn’t then it will wait 1 second and reduce the timer by 1. The same logic is applied to the countdown at the start of the game.
• Pickup Points / Dropoff Points / Car Waypoints
	o The Game Manager has an array for both pickup points, drop off points and the car waypoints. This was added manually to the array in the unity inspector. :^)
	o Other scripts access these arrays for various functions.
• GetDeliveryPoint()
	o Gets a random Dropoff point from the array.
	o Gets called by pickup points once they are triggered.
• EndGame()
	o Loads the main menu
	o Gets called when the timer has reached 0

--Pickup Points and Dropoff Points--
Both pickup points and dropoff points work in similar ways.
• If a player is inside its trigger and going slow enough then a function is called in the game manager.
• Pickup points also pick a random dropoff point from the dropoff point array that will be the chosen one to be enabled once the player triggers the pickup point.

--Score Controller--
• It is a singleton that has public functions for adding points when called.
	
--Ai Car Behavior-- 
• This is the scripts that the car agents use to travers the Navmesh. (See Reference 2.)
	o Gets a copy of the array of waypoints from the game manager.
	o Finds the closest way point -> moves to that waypoint. Once it gets close enough to the waypoint then it goes to the next waypoint in the array order.
	o Loops once it gets to the last waypoint in the array order.

--UI Controller--
• Changes the game UI to display points and time left.
	o Gets these from both the game manager and score controller
	o Also updates the countdown at the start of the game

--Arrow Controller--
• Points towards the drop-off point destination once the SetDestination() function is called.

--Menu Manager--
• Has functions that UI Buttons use for loading the main game scene and exiting the application.
	o Also displays the highscore accessed from the score controller.
--Music Manager--
• Will play a random song from the “musicTracks” array. When a song stops then it picks another random song. (might even be the same song again).

Timetable:
Game Features:	Time Spent:
Car Controls and Movement:	 5 Hours
Map Creation (Points Excluded):	 1 Hour
Pickup Points:			 1 Hours
Drop off Points: 		 0.5 Hours
Ai Car navigation and WayPoints: 2 Hours
Game UI and Main Menu		 1 Hour
Music Manager			 0.5 Hours

TOTAL TIME: 	11 Hours.

References:
1.	https://www.youtube.com/watch?v=CdPYlj5uZeI – For player car.
2.	https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.html - For the Ai cars

- Special Thanks to Benjamin Blichfeldt For the Amazing character model and Music

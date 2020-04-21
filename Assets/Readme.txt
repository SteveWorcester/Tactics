TacticsGame
Overview
	MVP Features
		Turn based, tactical combat gameplay
			Hotseat Multiplayer functionality - no AI
			Each friendly Character can take one move action and one attack action per player turn.
		Turn flow
			A simple text only “Start of turn” banner appears.  The banner has the controlling Player name (or number?) and the character identifier.  Banner then disappears.
			The active character is highlighted, and available options are presented.
			An action must be taken with that character (even if that option is “pass”?)
			Once actions have been expended, or pass has been selected, return to #1.
		Game Flow
			Players take turns for their Characters until only one team remains
		Playable character
			Character Active State
				On a characters turn (when their initiative value is the lowest), they are given a Move command and an Attack command.  They are the active character until these actions are taken (or until Pass is selected, forfeiting the remaining )
			Move
				Can be performed one time per turn
				Limited by a max range: 3
				Limited by a max height: 3
				Move can target a cell 1, 2 or 3 cells away from the currently occupied cell, and will move the active character to that position.
				Characters cannot move to an adjacent cell that is too high above their currently occupied cell.
				A character can only move to a cell if its height is equal to or less than the hight limit to their move.
				A character cannot move to a cell that is outside of their movement range.
				A character cannot move to a cell that has a height difference of greater than the max height from the previous cell.
				A character can move to a cell that is lower than the currently occupied cell, regardless of height.
			Attack
				Can be performed one time per turn.
				Limited by a max range: 1
				Attacks can be launched at a space on the board - and deal damage to targets occuping that space.
				Deals health damage to a target.
			Pass?
				Ends turn for that character.
		Game Field
			Pathable Terrain - characters can be directed to move over this type of terrain.
			Non-pathable terrain - characters will move up against this terrain, but cannot move through it.
	Gameplay Loops
		Post MVP
	Narrative
		Post MVP
		We need to have a flying pig that is only present in narrative scenes for some reason.
Appendices 
	Production Specific
		Git based issue reporting and task tracking
	Art Specific
		Placeholder Art until MVP is completed
	Design Specific
		Gameplay Field UI
			Game Field - Contains graphical representations of the terrain, characters, and reflects selected menu options
			Side UI Element - Contains status and health information for all fielded characters, colored to denote team, and ordered as a timer.
			Bottom UI Element - Contains selected character information with more detail than side UI element.
		Camera
			Static cam on Game Field
			Rotate functionality.
			tilt functionality?
		Menus
			Main Menu
			Character Status
		Character Jobs?  Classes?
			Scout
				Fast moving
				Tough to hit/damage (avoidant)
				Sensitive
			Bait
				Tough to hit/damage (armored)
				Battlefield manipulation
			Caster
				Ranged magic damage
				Area denial and positioning requirements
			Lancer
				Ranged physical damage
				Guide
			Sensitive
				Observant
				Crowd Controller
				Support
		Terrain
			General
				Terrain has a location X,Y location based on its position on the map.
				Terrain has a Height statistic based on its relative height.  
					Base height is 0.  
					a Height of 1 is 1 unit above.
					a Height of -1 is 1 unit below base height.
			Terrain Types
				Standard
					Costs 1 move to move over.
					Only one unit can occupy a cell at a time.
				Flooded
					Flooded cells block the Attack action.
				Difficult Terrain
					Costs 1 additional move to move off of that cell.  
					Moving onto a difficult terrain cell does not cost additional move.
References
	Mechanical Reference
		FFT
		Disgaea
	Artistic Reference
		Menus
		Characters
		Environments
	Narrative Reference
		???

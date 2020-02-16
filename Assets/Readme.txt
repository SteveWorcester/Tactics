TacticsGame
Overview
	MVP Features
		Turn based, tactical combat gameplay
			Hotseat Multiplayer functionality - no AI
			Each friendly Character can take one move action and one attack action per player turn.
		Turn flow
			“Start of turn” banner appears, notifying player that it’s their turn.  Banner then disappears.
			The active character is highlighted, and available options are presented.
			An action must be taken with that character (even if that option is “pass”?)
			Once actions have been expended, or pass has been selected, return to #1.
		Game Flow
			Players take turns for their Characters until only one team remains
		Playable character
			Character Active State
				Characters that have a move or attack left are highlighted, or have a different appearance.
			Move
				Can be performed one time per turn
				Limited by a max range: 3
				Can be stopped at any point up to the max range.
				Can be undone if an action (attack or ability) has not been performed afterwards.
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
		Gameplay Field
			Game Field - Contains graphical representations of the terrain, characters, and reflects selected menu options
			Side UI Element - Contains status and health information for all fielded characters, colored to denote team, and ordered as a timer.
			Bottom UI Element - Contains selected character information with more detail than side UI element.
		Camera
			Static cam on Game Field
			Rotate functionality.
			tilt functionality?
		Menus
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
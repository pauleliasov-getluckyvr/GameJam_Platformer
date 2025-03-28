Make GDD for 2d platformer (MVP) with the following mechanics:
Main mechanics description:
Character:
-Character movement: left-right, jump, double jump(optional, if achievement collected)
-Character actions: fire from different types of guns.
-Character starts at "Start point position"(adjustable in level builder)
-Level is considered passed when character reaches "End point position"
-Character can gather collectibles (coins, double jump boost, etc)
Character has 2 types of guns (can be extended, should be considered during architecture creation)
Character guns should "fly" above the character's head and rotate according to mouse position
Environment (game level):
The game level is composed of the following objects:
Ground
Collectibles (coins, double jump boost, etc, should be saleable)
Obstacles
wooden walls (for the Nail-Gun)
destructible objects
can be exploded by Bomb-Gun
can be destroyed by Nail-Gun or Bomb-Gun (like baloons with coins, etc)
Platforms
static {static platforms}
movable (platforms that can move in different directions (vertical, horizontal)). Platforms move like a "ping-pong": from right to left and vice versa.
Project should contain a "Level Builder" to preset all levels manually. Level builder should be a separate scene with the necessary functionality to adjust all game objects. After adjustments are done, we need functionality to save the level into Json. It can be an Editor button "Save lvl" with the field of level name.
Guns:
Gun rotation has a limitation of 180 degrees (can't rotate below player's head)
Nail gun: Strikes nails that can stick only in wooden walls and disappear on collision with any other types of obstacle. The character can jump on the stickied-in nail.
Bomb gun: Strikes with a delayed explosion. Bomb has all the necessary physics and can bounce out of walls. Bom explodes after 2 seconds or immediately by mouse "right click". Bomb can destroy only destructible objects.

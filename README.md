# MagnetRacers
All about magnets. Physics!!! Use the magnets pull and push properties in tandem to other magnets to zoom around the track. Made in Unity 5, C#. Top down racing game.

Magnet Racers is a game where four players control magnets as they race around a track. Whoever finishes 5 laps first wins. The trick is that players do not control the movement of the magnets directly; instead, players switch the charge of their magnet from positive to negative and vice versa, drawing themselves to points, or “pull poles,” at the corners of the map, and repelling those points to move to the next one.

Each MagnetRacer has their own magnetic field which interact with each other and the pull poles in the corner of the map. Each of this magnetic field for the racers have been defined in the script magnetscript(1-4), in the script it is defined to which gameobjects can the associated actor of the script associate it's magnetic properties with.

There also is another set of scripts which control the lap counter for the racers to figure out how many laps have been done and who wins.

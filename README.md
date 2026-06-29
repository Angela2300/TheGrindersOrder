# Project Title: The Grinder's Orders

\##Team Name: Goofy Devs

## Overview

A fast-paced top-down shooter where players take on the role of a butcher supplying meat to a demon bar. Players must fend off waves of human enemies using four different types of weapons, collect resources, and survive escalating difficulty within a tight time loop.

## Gameplay

### Loop

Harvest: Defeat humans to collect meat.
Sell: Enter the bar (“Sell” circle) to sell meat to earn coins.
Upgrade: Enter the shop (“Shop” icon circle) to use coins to upgrade player traits.
Loop: Progress through 5 levels of increasing difficulty.

### Combat Mechanics

Player Controls:
WASD to move
Left-click to fire
E to sell meat and open shop menu

Weapon Types:
Pistol: Starting weapon
SMG: Faster fire rate
Shotgun: Close range, deals burst damage
Grenade Launcher: Deals explosion damage, slow reload time

Enemy AI:
Standard Humans (Melee): Follow the player, deal damage when in range.
Ranged Humans (Shotgun): Keep distance, fire projectiles.
Tank Bombers: Charge towards the player and trigger a proximity explosion that deals radius damage. (Tip: Stay out of close range.)
Final Boss: Large range plus explosions.



### Economy (Bar and Shop)

Sell: Exchange raw meat for coins at the bar.
Buy/upgrade: Buy items at the shop to enhance player stats (speed, armor, fire rate, etc.)



### Extra Notes

Players cannot pass through boxes, but their bullets can. 

Bullets cannot pass through meat pickups, encouraging players to collect them.



## Contributions

### Team Member 1 - Thamu Daya Win (2501342)

Role: Enemy Stats, Game Play Designer (enemies \& level), System Designer,Play Tester  \& UI/UX
Tasks \& Contributions:
Created a Master Enemy Script adaptable to multiple enemy types
Set up enemy data, implemented enemies: Small, Medium, Bomber, Large
Configured enemy behavior via adjustable variables (speed, damage, health)
Enemy attack player
Developed Game Main Menu interface \& HUD system
Link player stats and enemy stats to apply damage and link with HUD
Reduced heart \& shields depending on the enemy attack
Added Bomber logic using distance-based explosion trigger
Created Enemies.csv Contributed to Player.csv
Added enemies sprite and UI Assets

### Team Member 2 -  Wong Tian Yun Angela (2502612)

Role: Player stats, Programmer, VFX, Scene Builder, UI/UX ,Play Tester \& System Designer
Tasks \& Contributions:
Built the Level Scene layout and visual structure (UI, Weapon, and Inventory)
Set up the Player data structure handover to teammates
Contributed to Inventory.csv \& Player.csv
Implemented Player Controller
Designed HUD system:
Health display (5 Hearts)
Inventory system (3 Weapon slots)
Integrated UI with gameplay variables from the programming
Configure Loot drop: meat \& weapons pickup.
Contributed to the weapon and enemy damage system,  enemy UI health bar by debugging, integrating, and refining damage-related scripts to assist teammate.

Integrated team members' scenes and scripts into a single game.
Added VFX for enemies \& weapons

### Team Member 3 - Koh Qin Xi (2500031)

Role: Gameplay Programmer, Systems Designer, Environment Artist, Play Tester, and UI/UX
Tasks \& Contributions:
Developed the Master Manager Script for core game coordination
Contributed to Weapons.csv
Built Weapon System with damage tiers and CSV-driven weapon stats
Implemented weapon input handling
Designed weapon switching using a single enum system
Implemented bullet firing behaviour for pistol, SMG, shotgun, and launcher
Focused on fast prototyping with instant state changes and simple combat flow
Created the environment art asset
Implemented obstacle collisions and movement blocking in the level



### Team Member 4 - Huang Xinjia Sam (2501491)

Role: Gameplay Systems Developer, System Designer, Sound Designer, Play Tester and UI/UX
Tasks \& Contributions:
Implemented the Coin Drop System, allowing enemies to drop coins upon defeat.
Contributed to Player.csv and Weapons.csv
Developed the Economy System for managing in-game currency, spending, and resource transactions.
Integrated the economy system across multiple gameplay features, connecting enemy rewards, shop purchases, selling mechanics, and player progression.
Built the Shop Trigger System, enabling players to buy upgrades and sell harvested meat.
Developed the Weapon Upgrade System, allowing players to upgrade weapon statistics through the shop using in-game currency.
Designed Loot system, allowing players to pick up enemy loot.
Implemented the Sell Circle interaction for exchanging harvested meat into coins.
Integrated background music and sound effects using the Audio Manager to provide gameplay and UI feedback
Assisted with gameplay balancing, economy tuning, and testing to ensure a smooth gameplay progression.

### Team Member 5 - Gan Qian Ting (2501556)

Role: Game Designer \& Systems Balancer, Level Designer \& UIUX
Tasks \& Contributions:
Developed Level Manager, Game Starter and Level Loader systems using CSV and text data to manage level layouts and seamless progression.
Created Level.csv.
Implemented and linked state-driven game conditions including player death, victory, out-of-time scenarios, weapon drop spawns between level and automatic progression when quotas are met for each level.
Engineered the Level Spawner and Enemy Manager systems, utilizing arrays to configure, balance and scale the timing, quantity and variety of enemy types across levels.
Linked back-end data to the UI text to ensure real-time, accurate tracking of the countdown timer, current level number, and economic metrics like meat collected and sold.

## Sources

### Sprites

Player: https://chatgpt.com/s/m\_6a41084a33a88191a4f6b09a32c82629
Enemy (Farmer): https://www.sprite-ai.art/gallery/stardew-valley-style-farmer-character-93ee
Enemy (Shotgun): https://favpng.com/png\_view/pixel-art-pixel-art-animation-soldier-png/CJqK0dXv
Enemy (Knight): https://www.vecteezy.com/png/73097080-pixelated-roman-knight-in-action
Enemy (Bomber): https://www.shutterstock.com/image-vector/pixel-art-soldier-character-icons-260nw-2180539463.jpg
Final Boss (Bartender): https://www.hiclipart.com/free-transparent-background-png-clipart-nfeva
Weapons: https://chatgpt.com/s/m\_6a4108526ea4819185a2f9847660d4d3
Player Bullet: https://chatgpt.com/s/m\_6a4108de728481919a5b9045d591836f
Enemy Bullet: https://chatgpt.com/s/m\_6a4108e9bfc48191910b1288a98fdfdb
Shop Icons (Generated with ChatGPT)

### UI

Inventory Slots: https://chatgpt.com/s/m\_6a41037c90a0819191092eb18efc8486
Shield: https://chatgpt.com/s/m\_6a410672b40081918a19b5191260897d
Heart: https://chatgpt.com/s/m\_6a4106c1d0d88191b1670f8a4c69cd63

### Fonts

Special Elite: https://fonts.google.com/specimen/Special+Elite?preview.script=Latn
Mokgech: https://www.fontspace.com/mokgech-font-f67928

### GitHub

https://github.com/Angela2300/TheGrindersOrder.git

### SFX and Music

SFX: https://getsoundly.com/
Music: Composed with https://www.bandlab.com/feed/trending


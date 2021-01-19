# Burger Truckers
Game Maker's Toolkit Game Jam 2020- "Out of Control"
Code, Design, Audio by Michael Freaney
Art by Ryan Springler

## Concept
Control a chef on a food truck- prepare orders for hungry customers, but the catch here is that the truck cannot stop moving! Get the orders prepared as you drive around the track and try to balance the two as best as you can.

## Notable Code
### Ticket System
To keep the game in an infinite loop, a progression system was designed to constantly replace and update order tickets when available. The player can have a maximum of 2 order tickets at any one time, and a timer is played to determine when the next one will be sent. If there are 2 tickets already active, then the timer is stopped after it runs out, a completed ticket is immediately replaced, and the timer is restarted. The timer's length is shortened for every ticket completed.
#### Simplifying development
Because this game jam was only 48 hours long, we decided to simplify development greatly by pulling the recipes for burgers from a csv file- this allowed us to store the data and add new ideas in a very short amount of time, so that we could spend more time on other, more pressing development tasks.
### Carrying/"Inventory" System
To be able to interact with appliances, the player is able to carry food items one at a time. Some small-scale OOP design allowed me to create "food" objects and display intuitive UI to make the player's experience simple and easy to follow. Several kinds of tables were prepared that have effects on whatever is placed (if it is compatible):
* Grill - cooks burger patties, chopped onions, chopped mushrooms
* Chop Table - when a choppable food item is placed on the chop table, it comes back chopped.
* Prep Table - placing ingredients on here will fill out orders- essentially the "submit" button.

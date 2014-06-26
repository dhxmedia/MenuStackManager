# MenuStackManager
Set of classes for managing a stack of menu items.
## Why?
While you can create individual menu items in libraries like tk2d, there isn't anything anything that ties them together in a way that automatically maintains order of elements. This provides a simple menu stack, where each menu layer is separately instantiated prefab. Every Push of the menu stack moves elements further back in the Z direction. This makes creating popups, and other layered elements much easier.

## How?
Create a GameObject with a Manager Component on it. Initialize by Pushing a prefab. 

It's recommended that the samples are looked at.

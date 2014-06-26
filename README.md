# MenuStackManager
Set of classes for managing a stack of menu items.
## Why?
While you can create individual menu items in libraries like tk2d, there isn't anything anything that ties them together in a way that automatically maintains order of elements. This provides a simple menu stack, where each menu layer is separately instantiated prefab. Every Push of the menu stack moves elements further back in the Z direction. This makes creating popups, and other layered elements much easier.

MenuStackManager also offers a method to create Intros/Outros on individual menu layers. Base layers also yield to Intros/Outros on child layers, meaning maintaining menu animation order is easy to do.

## How?
Create a GameObject with a Manager Component on it. Initialize by Pushing a prefab. 

It's recommended that the samples are looked at.
